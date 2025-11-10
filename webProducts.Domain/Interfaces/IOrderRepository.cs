using webProducts.Domain.Entities;

namespace webProducts.Domain.Interfaces;

public interface IOrderRepository
{
    Task<Order> AddAsync(Order o);
    Task<List<Order>> GetByUserAsync(int userId);
}