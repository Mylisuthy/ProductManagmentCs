using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webProducts.Application.Dto;
using webProducts.Application.Interfaces;
using webProducts.Domain.Entities;

namespace webProducts.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _svc;
    public ProductsController(IProductService svc) { _svc = svc; }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Product p)
    {
        var created = await _svc.CreateAsync(p);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpGet]
    public async Task<IActionResult> List() => Ok(await _svc.ListAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var p = await _svc.GetByIdAsync(id);
        if (p == null) return NotFound();
        return Ok(p);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateDto dto)
    {
        var existing = await _svc.GetByIdAsync(id);
        if (existing == null) return NotFound();
        existing.Name = dto.Name ?? existing.Name;
        existing.Description = dto.Description ?? existing.Description;
        existing.Price = dto.Price ?? existing.Price;
        existing.Stock = dto.Stock ?? existing.Stock;
        var ok = await _svc.UpdateAsync(existing);
        if (!ok) return StatusCode(500);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _svc.DeleteAsync(id);
        if (!ok) return NotFound();
        return NoContent();
    }
}