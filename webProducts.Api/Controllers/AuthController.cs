using Microsoft.AspNetCore.Mvc;
using webProducts.Application.Dto;
using webProducts.Application.Interfaces;

namespace webProducts.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    public AuthController(IAuthService auth) { _auth = auth; }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest r)
    {
        try
        {
            var user = await _auth.RegisterAsync(r.UserName, r.Email, r.Password);
            return Ok(new { user.Id, user.UserName, user.Email });
        }
        catch (Exception ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest r)
    {
        var token = await _auth.LoginAsync(r.EmailOrUser, r.Password);
        if (token == null) return Unauthorized();
        return Ok(new { token });
    }
}