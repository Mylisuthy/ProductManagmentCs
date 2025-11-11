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
        var localConnectionString = "Host=localhost;Port=5348;Database=productsdb;Username=postgres;Password=Qwe.123*;";
        
        var conn = configuration.GetConnectionString("DefaultConnection") ?? localConnectionString;
        
        services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql(conn));
        
        // Repositorios concretos
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        return services;
    }
}