using AutoMapper;
using CarRental.Application.Contracts.Dto;
using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace CarRental.Infrastructure.Messaging;

/// <summary>
/// Фоновый сервис — потребитель сообщений из очереди RabbitMQ.
/// Получает пакеты договоров аренды и сохраняет их в базу данных.
/// </summary>
public class RentalQueueConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<RentalQueueConsumer> _logger;
    private readonly IMapper _mapper;
    private readonly string _exchangeName;
    private readonly string _queueName;
    private IConnection? _connection;
    private IModel? _channel;

    public RentalQueueConsumer(
        IConnectionFactory connectionFactory,
        IServiceScopeFactory scopeFactory,
        IConfiguration configuration,
        ILogger<RentalQueueConsumer> logger,
        IMapper mapper)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _mapper = mapper;
        _exchangeName = configuration.GetSection("RabbitMQ")["ExchangeName"]
            ?? throw new KeyNotFoundException("RabbitMQ:ExchangeName is missing");
        _queueName = configuration.GetSection("RabbitMQ")["QueueName"]
            ?? throw new KeyNotFoundException("RabbitMQ:QueueName is missing");

        InitializeConnection(connectionFactory);
    }

    private void InitializeConnection(IConnectionFactory factory)
    {
        const int maxRetries = 10;
        const int delayMs = 3000;

        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(_exchangeName, ExchangeType.Fanout, durable: true);
                _channel.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false);
                _channel.QueueBind(_queueName, _exchangeName, routingKey: "");
                _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                _logger.LogInformation("Connected to RabbitMQ, exchange={exchange}, queue={queue}",
                    _exchangeName, _queueName);
                return;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "RabbitMQ connection attempt {attempt}/{max} failed, retrying in {delay}ms",
                    attempt, maxRetries, delayMs);

                if (attempt == maxRetries)
                    throw;

                Thread.Sleep(delayMs);
            }
        }
    }

    /// <summary>
    /// Запускает цикл потребления сообщений из очереди RabbitMQ
    /// </summary>
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.Register(() => _logger.LogInformation("RentalQueueConsumer is stopping"));

        if (_channel is null)
        {
            _logger.LogError("RabbitMQ channel is not initialized");
            return Task.CompletedTask;
        }

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (_, ea) =>
        {
            string? msgId = null;
            try
            {
                msgId = ea.BasicProperties?.MessageId;
                var json = Encoding.UTF8.GetString(ea.Body.Span);
                var batch = JsonSerializer.Deserialize<IList<RentalEditDto>>(json);

                if (batch is null || batch.Count == 0)
                {
                    _logger.LogWarning("Received empty batch, msgId={msgId}", msgId);
                    _channel.BasicAck(ea.DeliveryTag, multiple: false);
                    return;
                }

                _logger.LogInformation("Processing message msgId={msgId} with {count} contracts", msgId, batch.Count);
                await ProcessBatchAsync(batch, msgId, stoppingToken);
                _channel.BasicAck(ea.DeliveryTag, multiple: false);

                _logger.LogInformation("Acknowledged message msgId={msgId}", msgId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process message msgId={msgId}", msgId);
                _channel.BasicNack(ea.DeliveryTag, multiple: false, requeue: false);
            }
        };

        _channel.BasicConsume(_queueName, autoAck: false, consumer);
        _logger.LogInformation("Started consuming from queue {queue}", _queueName);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Обрабатывает пакет DTO: проверяет ссылочную целостность и сохраняет валидные записи
    /// </summary>
    private async Task ProcessBatchAsync(
        IList<RentalEditDto> batch,
        string? msgId,
        CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var rentalRepo = scope.ServiceProvider.GetRequiredService<IRepository<Rental>>();
        var carRepo    = scope.ServiceProvider.GetRequiredService<IRepository<Car>>();
        var clientRepo = scope.ServiceProvider.GetRequiredService<IRepository<Client>>();

        var carIds    = batch.Select(r => r.CarId).Distinct().ToList();
        var clientIds = batch.Select(r => r.ClientId).Distinct().ToList();

        var validCarIds = (await carRepo.GetQueryable()
            .Where(c => carIds.Contains(c.Id))
            .Select(c => c.Id)
            .ToListAsync(cancellationToken)).ToHashSet();

        var validClientIds = (await clientRepo.GetQueryable()
            .Where(c => clientIds.Contains(c.Id))
            .Select(c => c.Id)
            .ToListAsync(cancellationToken)).ToHashSet();

        foreach (var dto in batch)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (!validCarIds.Contains(dto.CarId) || !validClientIds.Contains(dto.ClientId))
            {
                _logger.LogWarning(
                    "Skipping contract in msgId={msgId}: CarId={carId} or ClientId={clientId} not found",
                    msgId, dto.CarId, dto.ClientId);
                continue;
            }

            try
            {
                var rental = _mapper.Map<Rental>(dto);
                await rentalRepo.AddAsync(rental);
                _logger.LogInformation("Saved rental from msgId={msgId} CarId={carId} ClientId={clientId}",
                    msgId, dto.CarId, dto.ClientId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving rental from msgId={msgId} CarId={carId} ClientId={clientId}",
                    msgId, dto.CarId, dto.ClientId);
            }
        }
    }

    public override void Dispose()
    {
        try { _channel?.Close(); } catch { /* ignore */ }
        try { _connection?.Close(); } catch { /* ignore */ }
        base.Dispose();
    }
}
