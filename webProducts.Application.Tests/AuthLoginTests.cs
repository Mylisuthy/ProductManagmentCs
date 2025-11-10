using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using webProducts.Application.Services;
using webProducts.Domain.Entities;
using webProducts.Domain.Interfaces;
using Xunit;

public class AuthLoginTests
{
    [Fact]
    public async Task Login_Should_Return_Token_When_Credentials_Valid()
    {
        var mockRepo = new Mock<IUserRepository>();
        var user = new User { Id = 1, UserName = "juan", Email = "juan@mail.com", Password = BCrypt.Net.BCrypt.HashPassword("1234"), Role = Role.User };
        mockRepo.Setup(r => r.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);

        var inMemorySettings = new Dictionary<string, string> {
            {"Jwt:Key", "testsecretkey12345"},
            {"Jwt:Issuer", "testIssuer"}
        };
        var config = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();

        var auth = new AuthService(mockRepo.Object, config);
        var token = await auth.LoginAsync("juan@mail.com", "1234");

        Assert.NotNull(token);
    }
}