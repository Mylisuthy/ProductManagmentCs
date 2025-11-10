namespace webProducts.Application.Dto;

public record RegisterRequest(string UserName, string Email, string Password);
public record LoginRequest(string EmailOrUser, string Password);
public record AuthResponse(string Token);