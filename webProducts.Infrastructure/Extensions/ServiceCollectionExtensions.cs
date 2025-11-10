using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using webProducts.Domain.Interfaces;
using webProducts.Infrastructure.Data;
using webProducts.Infrastructure.Repositories;

namespace webProducts.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var conn = configuration.GetConnectionString("DefaultConnection") ?? "Server=localhost;Database=productsdb;Uid=root;Pwd=root;";
        services.AddDbContext<AppDbContext>(opt => opt.UseMySql(conn, ServerVersion.AutoDetect(conn)));
        // Repositorios concretos
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        return services;
    }
}