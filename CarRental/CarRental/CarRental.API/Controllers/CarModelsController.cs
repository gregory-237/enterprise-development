using AutoMapper;
using CarRental.Application.Contracts.Dto;
using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers;

/// <summary>
/// CRUD-операции над справочником моделей автомобилей
/// </summary>
[ApiController]
[Route("api/car-models")]
public class CarModelsController(
    IRepository<CarModel> repo,
    IMapper mapper) : ControllerBase
{
    /// <summary>Получить список всех моделей</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CarModelGetDto>>> GetAll()
    {
        var items = await repo.GetAllAsync();
        return Ok(items.Select(mapper.Map<CarModelGetDto>));
    }

    /// <summary>Получить модель по идентификатору</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CarModelGetDto>> GetById(int id)
    {
        var item = await repo.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(mapper.Map<CarModelGetDto>(item));
    }

    /// <summary>Создать новую модель</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<CarModelGetDto>> Create([FromBody] CarModelEditDto dto)
    {
        var entity = mapper.Map<CarModel>(dto);
        var created = await repo.AddAsync(entity);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, mapper.Map<CarModelGetDto>(created));
    }

    /// <summary>Обновить модель</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CarModelGetDto>> Update(int id, [FromBody] CarModelEditDto dto)
    {
        var existing = await repo.GetByIdAsync(id);
        if (existing is null) return NotFound();

        mapper.Map(dto, existing);
        var updated = await repo.UpdateAsync(existing);
        return Ok(mapper.Map<CarModelGetDto>(updated));
    }

    /// <summary>Удалить модель</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await repo.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
