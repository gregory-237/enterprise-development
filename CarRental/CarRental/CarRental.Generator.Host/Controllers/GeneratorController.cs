using CarRental.Application.Contracts.Dto;
using CarRental.Generator.Host.Generator;
using CarRental.Generator.Host.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Generator.Host.Controllers;

/// <summary>
/// Контроллер генератора договоров аренды.
/// Генерирует тестовые данные и публикует их в RabbitMQ.
/// </summary>
/// <param name="publisher">Публикатор сообщений RabbitMQ</param>
/// <param name="logger">Логгер</param>
[ApiController]
[Route("api/[controller]")]
public class GeneratorController(
    RentalPublisher publisher,
    ILogger<GeneratorController> logger) : ControllerBase
{
    /// <summary>
    /// Сгенерировать договоры и отправить их в RabbitMQ пакетами
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
            var items = RentalGenerator.Generate(totalCount);

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
