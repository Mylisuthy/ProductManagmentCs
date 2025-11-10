using webProducts.Domain.Entities;

namespace webProducts.Domain.Interfaces;

public interface ICartRepository
{
    Task<List<CartItem>> GetByUserAsync(int userId);
    Task<CartItem?> GetByUserAndProduct(int userId, int productId);
    Task AddAsync(CartItem item);
    Task UpdateAsync(CartItem item);
    Task DeleteAsync(CartItem item);
    Task ClearUserCart(int userId);
}