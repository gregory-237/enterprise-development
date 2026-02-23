using AutoMapper;
using CarRental.Application.Contracts.Dto;
using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Controllers;

/// <summary>
/// Аналитические запросы по данным проката
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AnalyticsController(
    IRepository<Rental>          rentalRepo,
    IRepository<Car>             carRepo,
    IRepository<Client>          clientRepo,
    IRepository<ModelGeneration> generationRepo,
    IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Клиенты, арендовавшие ТС указанной модели, отсортированные по ФИО
    /// </summary>
    [HttpGet("clients-by-model")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ClientGetDto>>> GetClientsByModel([FromQuery] string modelName)
    {
        var query = rentalRepo.GetQueryable(q => q
            .Include(r => r.Car)
                .ThenInclude(c => c!.ModelGeneration)
                    .ThenInclude(mg => mg!.Model)
            .Include(r => r.Client));

        var clients = await query
            .Where(r => r.Car!.ModelGeneration!.Model!.Name == modelName)
            .Select(r => r.Client)
            .Distinct()
            .OrderBy(c => c!.FullName)
            .ToListAsync();

        return Ok(clients.Select(mapper.Map<ClientGetDto>));
    }

    /// <summary>
    /// Автомобили, находящиеся в аренде на указанный момент
    /// </summary>
    [HttpGet("currently-rented")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CarGetDto>>> GetCurrentlyRented([FromQuery] DateTime currentDate)
    {
        var activeCarIds = await rentalRepo.GetQueryable()
            .Where(r => r.RentalDate.AddHours(r.RentalHours) > currentDate)
            .Select(r => r.CarId)
            .Distinct()
            .ToListAsync();

        var cars = await carRepo.GetQueryable()
            .Where(c => activeCarIds.Contains(c.Id))
            .Include(c => c.ModelGeneration)
                .ThenInclude(mg => mg!.Model)
            .ToListAsync();

        return Ok(cars.Select(mapper.Map<CarGetDto>));
    }

    /// <summary>
    /// Топ-5 наиболее часто арендуемых автомобилей
    /// </summary>
    [HttpGet("top-rented-cars")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CarRentalCountDto>>> GetTopRentedCars()
    {
        var stats = await rentalRepo.GetQueryable()
            .GroupBy(r => r.CarId)
            .Select(g => new { CarId = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .ToListAsync();

        var ids = stats.Select(s => s.CarId).ToList();
        var cars = await carRepo.GetQueryable()
            .Where(c => ids.Contains(c.Id))
            .Include(c => c.ModelGeneration)
                .ThenInclude(mg => mg!.Model)
            .ToListAsync();

        var dict = cars.ToDictionary(c => c.Id);
        var result = stats
            .Where(s => dict.ContainsKey(s.CarId))
            .Select(s => new CarRentalCountDto(mapper.Map<CarGetDto>(dict[s.CarId]), s.Count));

        return Ok(result);
    }

    /// <summary>
    /// Число аренд для каждого автомобиля в парке
    /// </summary>
    [HttpGet("rentals-per-car")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CarRentalCountDto>>> GetRentalsPerCar()
    {
        var counts = await rentalRepo.GetQueryable()
            .GroupBy(r => r.CarId)
            .Select(g => new { CarId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.CarId, x => x.Count);

        var cars = await carRepo.GetQueryable()
            .Include(c => c.ModelGeneration)
                .ThenInclude(mg => mg!.Model)
            .ToListAsync();

        var result = cars
            .Select(c => new CarRentalCountDto(
                mapper.Map<CarGetDto>(c),
                counts.GetValueOrDefault(c.Id, 0)))
            .OrderByDescending(x => x.RentalCount);

        return Ok(result);
    }

    /// <summary>
    /// Топ-5 клиентов по суммарной стоимости аренды
    /// </summary>
    [HttpGet("top-clients-by-amount")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ClientRentalAmountDto>>> GetTopClientsByAmount()
    {
        var rentals = await rentalRepo.GetQueryable()
            .Select(r => new { r.ClientId, r.CarId, r.RentalHours })
            .ToListAsync();

        var carPrices = await carRepo.GetQueryable()
            .Join(generationRepo.GetQueryable(),
                  c => c.ModelGenerationId,
                  g => g.Id,
                  (c, g) => new { CarId = c.Id, g.RentalPricePerHour })
            .ToDictionaryAsync(x => x.CarId, x => x.RentalPricePerHour);

        var topStats = rentals
            .GroupBy(r => r.ClientId)
            .Select(g => new
            {
                ClientId    = g.Key,
                TotalAmount = g.Sum(r => r.RentalHours * carPrices.GetValueOrDefault(r.CarId, 0))
            })
            .OrderByDescending(x => x.TotalAmount)
            .Take(5)
            .ToList();

        var topIds = topStats.Select(s => s.ClientId).ToList();
        var clients = await clientRepo.GetQueryable()
            .Where(c => topIds.Contains(c.Id))
            .ToDictionaryAsync(c => c.Id);

        var result = topStats
            .Where(s => clients.ContainsKey(s.ClientId))
            .Select(s => new ClientRentalAmountDto(
                mapper.Map<ClientGetDto>(clients[s.ClientId]),
                s.TotalAmount));

        return Ok(result);
    }
}
