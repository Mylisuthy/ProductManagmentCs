using webProducts.Domain.Entities;

namespace webProducts.Domain.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(int id);
    Task<List<Product>> ListAllAsync();
    Task<Product> AddAsync(Product p);
    Task UpdateAsync(Product p);
    Task DeleteAsync(Product p);
}