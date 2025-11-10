using webProducts.Domain.Entities;

namespace webProducts.Application.Interfaces;

public interface ICartService
{
    Task AddOrUpdateCart(int userId, int productId, int quantity);
    Task<List<CartItem>> GetUserCart(int userId);
    Task RemoveCartItem(int userId, int productId);
    Task<Order> Checkout(int userId);
}