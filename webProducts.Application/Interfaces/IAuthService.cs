using webProducts.Domain.Entities;

namespace webProducts.Application.Interfaces;

public interface IAuthService
{
    Task<User> RegisterAsync(string username, string email, string password, Role role = Role.User);
    Task<string?> LoginAsync(string emailOrUser, string password);
}