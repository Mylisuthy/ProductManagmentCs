using Microsoft.EntityFrameworkCore;
using webProducts.Domain.Entities;
using webProducts.Domain.Interfaces;
using webProducts.Infrastructure.Data;

namespace webProducts.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;
    public UserRepository(AppDbContext db) => _db = db;

    public Task<User?> GetByIdAsync(int id) => _db.Users.Include(u => u.Orders).FirstOrDefaultAsync(u => u.Id == id);
    public Task<User?> GetByEmailAsync(string email) => _db.Users.FirstOrDefaultAsync(u => u.Email == email);
    public Task<User?> GetByUserNameAsync(string username) => _db.Users.FirstOrDefaultAsync(u => u.UserName == username);
    public async Task<User> AddAsync(User u) { _db.Users.Add(u); await _db.SaveChangesAsync(); return u; }
    public async Task UpdateAsync(User u) { _db.Users.Update(u); await _db.SaveChangesAsync(); }
    public async Task DeleteAsync(User u) { _db.Users.Remove(u); await _db.SaveChangesAsync(); }
    public Task<List<User>> ListAllAsync() => _db.Users.AsNoTracking().ToListAsync();
}