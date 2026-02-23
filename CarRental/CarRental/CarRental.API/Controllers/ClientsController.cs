using AutoMapper;
using CarRental.Application.Contracts.Dto;
using CarRental.Domain.Entities;
using CarRental.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers;

/// <summary>
/// CRUD-операции над клиентами
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ClientsController(
    IRepository<Client> repo,
    IMapper mapper) : ControllerBase
{
    /// <summary>Получить список всех клиентов</summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ClientGetDto>>> GetAll()
    {
        var items = await repo.GetAllAsync();
        return Ok(items.Select(mapper.Map<ClientGetDto>));
    }

    /// <summary>Получить клиента по идентификатору</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClientGetDto>> GetById(int id)
    {
        var item = await repo.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(mapper.Map<ClientGetDto>(item));
    }

    /// <summary>Создать нового клиента</summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<ClientGetDto>> Create([FromBody] ClientEditDto dto)
    {
        var entity = mapper.Map<Client>(dto);
        var created = await repo.AddAsync(entity);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, mapper.Map<ClientGetDto>(created));
    }

    /// <summary>Обновить данные клиента</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClientGetDto>> Update(int id, [FromBody] ClientEditDto dto)
    {
        var existing = await repo.GetByIdAsync(id);
        if (existing is null) return NotFound();

        mapper.Map(dto, existing);
        var updated = await repo.UpdateAsync(existing);
        return Ok(mapper.Map<ClientGetDto>(updated));
    }

    /// <summary>Удалить клиента</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await repo.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
