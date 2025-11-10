using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webProducts.Domain.Entities;
using webProducts.Domain.Interfaces;

namespace webProducts.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _repo;
    public UsersController(IUserRepository repo) { _repo = repo; }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _repo.ListAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var u = await _repo.GetByIdAsync(id);
        if (u == null) return NotFound();
        return Ok(u);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] User update)
    {
        var u = await _repo.GetByIdAsync(id);
        if (u == null) return NotFound();
        u.Email = update.Email;
        u.UserName = update.UserName;
        u.Role = update.Role;
        await _repo.UpdateAsync(u);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var u = await _repo.GetByIdAsync(id);
        if (u == null) return NotFound();
        await _repo.DeleteAsync(u);
        return NoContent();
    }
}