using webProducts.Application.Interfaces;
using webProducts.Domain.Entities;
using webProducts.Domain.Interfaces;

namespace webProducts.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repo;
    public ProductService(IProductRepository repo) => _repo = repo;

    public async Task<Product> CreateAsync(Product p) => await _repo.AddAsync(p);
    public async Task<List<Product>> ListAsync() => await _repo.ListAllAsync();
    public async Task<Product?> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
    public async Task<bool> UpdateAsync(Product p)
    {
        var existing = await _repo.GetByIdAsync(p.Id);
        if (existing == null) return false;
        existing.Name = p.Name;
        existing.Description = p.Description;
        existing.Price = p.Price;
        existing.Stock = p.Stock;
        await _repo.UpdateAsync(existing);
        return true;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) return false;
        await _repo.DeleteAsync(existing);
        return true;
    }
}