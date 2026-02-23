using AutoMapper;
using CarRental.Application.Contracts.Dto;
using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Controllers;

/// <summary>
/// CRUD-операции над договорами аренды
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RentalsController(
    IRepository<Rental> rentalRepo,
    IRepository<Car> carRepo,
    IRepository<Client> clientRepo,
    IMapper mapper) : ControllerBase
{
    /// <summary>Получить список всех договоров аренды</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<RentalGetDto>>> GetAll()
    {
        var items = await rentalRepo.GetAllAsync(q => q
            .Include(r => r.Car)
                .ThenInclude(c => c!.ModelGeneration)
                    .ThenInclude(mg => mg!.Model)
            .Include(r => r.Client));
        return Ok(items.Select(mapper.Map<RentalGetDto>));
    }

    /// <summary>Получить договор аренды по идентификатору</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RentalGetDto>> GetById(int id)
    {
        var item = await rentalRepo.GetByIdAsync(id, q => q
            .Include(r => r.Car)
                .ThenInclude(c => c!.ModelGeneration)
                    .ThenInclude(mg => mg!.Model)
            .Include(r => r.Client));
        return item is null ? NotFound() : Ok(mapper.Map<RentalGetDto>(item));
    }

    /// <summary>Создать договор аренды</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RentalGetDto>> Create([FromBody] RentalEditDto dto)
    {
        var carExists = await carRepo.GetByIdAsync(dto.CarId);
        if (carExists is null)
            return BadRequest($"Автомобиль с Id={dto.CarId} не найден");

        var clientExists = await clientRepo.GetByIdAsync(dto.ClientId);
        if (clientExists is null)
            return BadRequest($"Клиент с Id={dto.ClientId} не найден");

        var entity = mapper.Map<Rental>(dto);
        var created = await rentalRepo.AddAsync(entity);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, mapper.Map<RentalGetDto>(created));
    }

    /// <summary>Обновить договор аренды</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RentalGetDto>> Update(int id, [FromBody] RentalEditDto dto)
    {
        var existing = await rentalRepo.GetByIdAsync(id);
        if (existing is null) return NotFound();

        mapper.Map(dto, existing);
        var updated = await rentalRepo.UpdateAsync(existing);
        return Ok(mapper.Map<RentalGetDto>(updated));
    }

    /// <summary>Удалить договор аренды</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await rentalRepo.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
