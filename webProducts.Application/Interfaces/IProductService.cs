using webProducts.Domain.Entities;

namespace webProducts.Application.Interfaces;

public interface IProductService
{
    Task<Product> CreateAsync(Product p);
    Task<List<Product>> ListAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<bool> UpdateAsync(Product p);
    Task<bool> DeleteAsync(int id);
}