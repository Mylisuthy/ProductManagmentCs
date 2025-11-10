using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using webProducts.Application.Interfaces;
using webProducts.Domain.Entities;
using webProducts.Domain.Interfaces;

namespace webProducts.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly IConfiguration _config;
    public AuthService(IUserRepository userRepo, IConfiguration config)
    {
        _userRepo = userRepo;
        _config = config;
    }

    public async Task<User> RegisterAsync(string username, string email, string password, Role role = Role.User)
    {
        var exists = await _userRepo.GetByEmailAsync(email);
        if (exists != null) throw new Exception("Email already used");
        var user = new User
        {
            UserName = username,
            Email = email,
            Password = BCrypt.Net.BCrypt.HashPassword(password),
            Role = role
        };
        await _userRepo.AddAsync(user);
        return user;
    }

    public async Task<string?> LoginAsync(string emailOrUser, string password)
    {
        User? user = null;
        if (emailOrUser.Contains("@")) user = await _userRepo.GetByEmailAsync(emailOrUser);
        else user = await _userRepo.GetByUserNameAsync(emailOrUser);
        if (user == null) return null;
        if (!BCrypt.Net.BCrypt.Verify(password, user.Password)) return null;

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>
        {
            new Claim("id", user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };
        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(6),
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}