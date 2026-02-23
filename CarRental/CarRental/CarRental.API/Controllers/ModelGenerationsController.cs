using AutoMapper;
using CarRental.Application.Contracts.Dto;
using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRental.API.Controllers;

/// <summary>
/// CRUD-операции над справочником поколений моделей
/// </summary>
[ApiController]
[Route("api/model-generations")]
public class ModelGenerationsController(
    IRepository<ModelGeneration> repo,
    IMapper mapper) : ControllerBase
{
    /// <summary>Получить список всех поколений</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ModelGenerationGetDto>>> GetAll()
    {
        var items = await repo.GetAllAsync(q => q.Include(mg => mg.Model));
        return Ok(items.Select(mapper.Map<ModelGenerationGetDto>));
    }

    /// <summary>Получить поколение по идентификатору</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ModelGenerationGetDto>> GetById(int id)
    {
        var item = await repo.GetByIdAsync(id, q => q.Include(mg => mg.Model));
        return item is null ? NotFound() : Ok(mapper.Map<ModelGenerationGetDto>(item));
    }

    /// <summary>Создать поколение</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<ModelGenerationGetDto>> Create([FromBody] ModelGenerationEditDto dto)
    {
        var entity = mapper.Map<ModelGeneration>(dto);
        var created = await repo.AddAsync(entity);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, mapper.Map<ModelGenerationGetDto>(created));
    }

    /// <summary>Обновить поколение</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ModelGenerationGetDto>> Update(int id, [FromBody] ModelGenerationEditDto dto)
    {
        var existing = await repo.GetByIdAsync(id);
        if (existing is null) return NotFound();

        mapper.Map(dto, existing);
        var updated = await repo.UpdateAsync(existing);
        return Ok(mapper.Map<ModelGenerationGetDto>(updated));
    }

    /// <summary>Удалить поколение</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await repo.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
