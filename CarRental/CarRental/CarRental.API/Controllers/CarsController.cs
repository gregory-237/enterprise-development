using AutoMapper;
using CarRental.Application.Contracts.Dto;
using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Controllers;

/// <summary>
/// CRUD-операции над транспортными средствами
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CarsController(
    IRepository<Car> repo,
    IMapper mapper) : ControllerBase
{
    /// <summary>Получить список всех ТС с деталями поколения и модели</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CarGetDto>>> GetAll()
    {
        var items = await repo.GetAllAsync(q => q
            .Include(c => c.ModelGeneration)
                .ThenInclude(mg => mg!.Model));
        return Ok(items.Select(mapper.Map<CarGetDto>));
    }

    /// <summary>Получить ТС по идентификатору</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CarGetDto>> GetById(int id)
    {
        var item = await repo.GetByIdAsync(id, q => q
            .Include(c => c.ModelGeneration)
                .ThenInclude(mg => mg!.Model));
        return item is null ? NotFound() : Ok(mapper.Map<CarGetDto>(item));
    }

    /// <summary>Добавить новое ТС</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<CarGetDto>> Create([FromBody] CarEditDto dto)
    {
        var entity = mapper.Map<Car>(dto);
        var created = await repo.AddAsync(entity);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, mapper.Map<CarGetDto>(created));
    }

    /// <summary>Обновить данные ТС</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CarGetDto>> Update(int id, [FromBody] CarEditDto dto)
    {
        var existing = await repo.GetByIdAsync(id);
        if (existing is null) return NotFound();

        mapper.Map(dto, existing);
        var updated = await repo.UpdateAsync(existing);
        return Ok(mapper.Map<CarGetDto>(updated));
    }

    /// <summary>Удалить ТС</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await repo.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
