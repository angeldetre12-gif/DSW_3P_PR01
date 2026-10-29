using DSW_3P_EJ01.Models;
using DSW_3P_EJ01.Services;
using Microsoft.AspNetCore.Mvc;

namespace DSW_3P_EJ01.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class MisionesController(IMisionService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Mision>>> GetAll(CancellationToken cancellationToken)
    {
        var misiones = await service.GetAllAsync(cancellationToken);
        return Ok(misiones);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Mision>> GetById(int id, CancellationToken cancellationToken)
    {
        var mision = await service.GetByIdAsync(id, cancellationToken);
        return mision is null ? NotFound() : Ok(mision);
    }

    [HttpPost]
    public async Task<ActionResult<Mision>> Create(Mision mision, CancellationToken cancellationToken)
    {
        var creada = await service.CreateAsync(mision, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = creada.Id }, creada);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Mision mision, CancellationToken cancellationToken)
    {
        var actualizada = await service.UpdateAsync(id, mision, cancellationToken);
        return actualizada ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var eliminada = await service.DeleteAsync(id, cancellationToken);
        return eliminada ? NoContent() : NotFound();
    }
}
