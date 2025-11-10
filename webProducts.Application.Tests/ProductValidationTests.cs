using System.Threading.Tasks;
using webProducts.Domain.Entities;
using webProducts.Application.Services;
using Moq;
using webProducts.Domain.Interfaces;
using Xunit;

public class ProductValidationTests
{
    [Fact]
    public async Task Create_Product_Should_Succeed_When_Valid()
    {
        var repo = new Mock<IProductRepository>();
        repo.Setup(r => r.AddAsync(It.IsAny<Product>())).ReturnsAsync((Product p) => p);

        var service = new ProductService(repo.Object);
        var product = new Product { Name = "Test", Description = "ok", Price = 100, Stock = 10 };

        var created = await service.CreateAsync(product);

        Assert.Equal("Test", created.Name);
        repo.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
    }
}