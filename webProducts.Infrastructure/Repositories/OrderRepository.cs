using Microsoft.EntityFrameworkCore;
using webProducts.Domain.Entities;
using webProducts.Domain.Interfaces;
using webProducts.Infrastructure.Data;

namespace webProducts.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _db;
    public OrderRepository(AppDbContext db) => _db = db;

    public async Task<Order> AddAsync(Order o) { _db.Orders.Add(o); await _db.SaveChangesAsync(); return o; }
    public Task<List<Order>> GetByUserAsync(int userId) => _db.Orders.Include(o => o.Items).ThenInclude(i => i.Product).Where(o => o.UserId == userId).ToListAsync();
}