using Microsoft.EntityFrameworkCore;
using webProducts.Domain.Entities;
using webProducts.Domain.Interfaces;
using webProducts.Infrastructure.Data;

namespace webProducts.Infrastructure.Repositories;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _db;
    public CartRepository(AppDbContext db) => _db = db;

    public Task<List<CartItem>> GetByUserAsync(int userId) => _db.CartItems.Include(ci => ci.Product).Where(ci => ci.UserId == userId).ToListAsync();
    public async Task<CartItem?> GetByUserAndProduct(int userId, int productId) => await _db.CartItems.FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId);
    public async Task AddAsync(CartItem ci) { _db.CartItems.Add(ci); await _db.SaveChangesAsync(); }
    public async Task UpdateAsync(CartItem ci) { _db.CartItems.Update(ci); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(CartItem ci) { _db.CartItems.Remove(ci); await _db.SaveChangesAsync(); }
    public async Task ClearUserCart(int userId) { var items = _db.CartItems.Where(ci => ci.UserId == userId); _db.CartItems.RemoveRange(items); await _db.SaveChangesAsync(); }
}