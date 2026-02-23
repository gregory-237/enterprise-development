using CarRental.Application.Contracts.Dto;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace CarRental.Generator.Host.Messaging;

/// <summary>
/// Публикует пакеты договоров аренды в обменник RabbitMQ
/// </summary>
public class RentalPublisher
{
    private readonly IModel _channel;
    private readonly ILogger<RentalPublisher> _logger;
    private readonly string _exchangeName;

    public RentalPublisher(
        IConnection connection,
        IConfiguration configuration,
        ILogger<RentalPublisher> logger)
    {
        _logger = logger;
        _exchangeName = configuration.GetSection("RabbitMQ")["ExchangeName"]
            ?? throw new KeyNotFoundException("RabbitMQ:ExchangeName is missing");

        _channel = connection.CreateModel();
        _channel.ExchangeDeclare(_exchangeName, ExchangeType.Fanout, durable: true);

        _logger.LogInformation("RentalPublisher initialized, exchange={exchange}", _exchangeName);
    }

    /// <summary>
    /// Отправить пакет договоров аренды в RabbitMQ
    /// </summary>
    /// <param name="batch">Пакет DTO для отправки</param>
    public void Publish(IList<RentalEditDto> batch)
    {
        if (batch is null || batch.Count == 0)
        {
            _logger.LogWarning("Publish called with empty batch, skipping");
            return;
        }

        var msgId = Guid.NewGuid().ToString();

        try
        {
            var json = JsonSerializer.Serialize(batch);
            var body = Encoding.UTF8.GetBytes(json);

            var props = _channel.CreateBasicProperties();
            props.Persistent = true;
            props.MessageId  = msgId;
            props.ContentType = "application/json";

            _channel.BasicPublish(
                exchange: _exchangeName,
                routingKey: "",
                basicProperties: props,
                body: body);

            _logger.LogInformation("Published batch msgId={msgId} count={count} to exchange={exchange}",
                msgId, batch.Count, _exchangeName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish batch msgId={msgId} count={count}", msgId, batch.Count);
            throw;
        }
    }
}
