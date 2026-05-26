namespace APBD8.Controllers;

using Microsoft.AspNetCore.Mvc;
using APBD8.DTOs;
using APBD8.Services;


[ApiController]
[Route("api/pcs")]
public class PCsController : ControllerBase
{
    private readonly IPCService _pcService;

    public PCsController(IPCService pcService)
    {
        _pcService = pcService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var pcs = await _pcService.GetAllAsync();
        return Ok(pcs);
    }

    [HttpGet("{id:int}/components")]
    public async Task<IActionResult> GetComponentsByPCId(int id)
    {
        var components = await _pcService.GetComponentsByPCIdAsync(id);

        if (components is null)
            return NotFound();

        return Ok(components);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PCRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var createdPc = await _pcService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetComponentsByPCId),
            new { id = createdPc.Id },
            createdPc
        );
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] PCRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var updatedPc = await _pcService.UpdateAsync(id, dto);

        if (updatedPc is null)
            return NotFound();

        return Ok(updatedPc);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _pcService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}