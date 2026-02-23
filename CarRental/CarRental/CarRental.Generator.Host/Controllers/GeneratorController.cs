using CarRental.Application.Contracts.Dto;
using CarRental.Generator.Host.Generator;
using CarRental.Generator.Host.Messaging;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace CarRental.Generator.Host.Controllers;

/// <summary>
/// Контроллер генератора договоров аренды.
/// Генерирует тестовые данные и публикует их в RabbitMQ.
/// </summary>
/// <param name="publisher">Публикатор сообщений RabbitMQ</param>
/// <param name="httpClientFactory">Фабрика HTTP-клиентов для запросов к API</param>
/// <param name="logger">Логгер</param>
[ApiController]
[Route("api/[controller]")]
public class GeneratorController(
    RentalPublisher publisher,
    IHttpClientFactory httpClientFactory,
    ILogger<GeneratorController> logger) : ControllerBase
{
    /// <summary>
    /// Сгенерировать договоры и отправить их в RabbitMQ пакетами.
    /// Идентификаторы автомобилей и клиентов берутся из базы данных через API.
    /// </summary>
    /// <param name="totalCount">Общее количество генерируемых DTO</param>
    /// <param name="batchSize">Размер одного пакета</param>
    /// <param name="delayMs">Задержка между пакетами (мс)</param>
    /// <param name="cancellationToken">Токен отмены</param>
    [HttpPost("rentals")]
    [ProducesResponseType(typeof(IList<RentalEditDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IList<RentalEditDto>>> GenerateRentals(
        [FromQuery] int totalCount,
        [FromQuery] int batchSize,
        [FromQuery] int delayMs,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("{method}: totalCount={total} batchSize={batch} delayMs={delay}",
            nameof(GenerateRentals), totalCount, batchSize, delayMs);

        if (totalCount is <= 0 or > 10000)
            return BadRequest("totalCount должно быть от 1 до 10 000");

        if (batchSize is <= 0 or > 1000)
            return BadRequest("batchSize должно быть от 1 до 1 000");

        try
        {
            var http = httpClientFactory.CreateClient("carrental-api");

            var cars = await http.GetFromJsonAsync<IList<CarGetDto>>(
                "/api/Cars", cancellationToken);
            var clients = await http.GetFromJsonAsync<IList<ClientGetDto>>(
                "/api/Clients", cancellationToken);

            if (cars is null || cars.Count == 0)
                return BadRequest("Не удалось получить список автомобилей из API");
            if (clients is null || clients.Count == 0)
                return BadRequest("Не удалось получить список клиентов из API");

            var carIds    = cars.Select(c => c.Id).ToList();
            var clientIds = clients.Select(c => c.Id).ToList();

            logger.LogInformation("Fetched {cars} cars and {clients} clients from API",
                carIds.Count, clientIds.Count);

            var items = RentalGenerator.Generate(totalCount, carIds, clientIds);

            foreach (var chunk in items.Chunk(batchSize))
            {
                cancellationToken.ThrowIfCancellationRequested();
                publisher.Publish([.. chunk]);
                await Task.Delay(delayMs, cancellationToken);
            }

            logger.LogInformation("{method} finished: sent {total} records in batches of {batch}",
                nameof(GenerateRentals), totalCount, batchSize);

            return Ok(items);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            logger.LogWarning("{method} was cancelled", nameof(GenerateRentals));
            return BadRequest("Запрос был отменён");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error in {method}", nameof(GenerateRentals));
            return StatusCode(500, $"{ex.Message}\n{ex.InnerException?.Message}");
        }
    }
}
