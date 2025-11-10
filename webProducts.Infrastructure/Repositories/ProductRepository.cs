using Microsoft.EntityFrameworkCore;
using webProducts.Domain.Entities;
using webProducts.Domain.Interfaces;
using webProducts.Infrastructure.Data;

namespace webProducts.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _db;
    public ProductRepository(AppDbContext db) => _db = db;

    public Task<Product?> GetByIdAsync(int id) => _db.Products.FindAsync(id).AsTask();
    public Task<List<Product>> ListAllAsync() => _db.Products.ToListAsync();
    public async Task<Product> AddAsync(Product p) { _db.Products.Add(p); await _db.SaveChangesAsync(); return p; }
    public async Task UpdateAsync(Product p) { _db.Products.Update(p); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(Product p) { _db.Products.Remove(p); await _db.SaveChangesAsync(); }
}