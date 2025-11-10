using webProducts.Application.Interfaces;
using webProducts.Domain.Entities;
using webProducts.Domain.Interfaces;

namespace webProducts.Application.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepo;
    private readonly IProductRepository _productRepo;
    private readonly IOrderRepository _orderRepo;

    public CartService(ICartRepository cartRepo, IProductRepository productRepo, IOrderRepository orderRepo)
    {
        _cartRepo = cartRepo;
        _productRepo = productRepo;
        _orderRepo = orderRepo;
    }

    public async Task AddOrUpdateCart(int userId, int productId, int quantity)
    {
        if (quantity <= 0) throw new Exception("Quantity must be greater than 0");
        var product = await _productRepo.GetByIdAsync(productId);
        if (product == null) throw new Exception("Product not found");
        if (product.Stock < quantity) throw new Exception("Not enough stock");

        var existing = await _cartRepo.GetByUserAndProduct(userId, productId);
        if (existing != null)
        {
            existing.Quantity = quantity;
            await _cartRepo.UpdateAsync(existing);
        }
        else
        {
            var ci = new CartItem { UserId = userId, ProductId = productId, Quantity = quantity };
            await _cartRepo.AddAsync(ci);
        }
    }

    public Task<List<CartItem>> GetUserCart(int userId) => _cartRepo.GetByUserAsync(userId);

    public async Task RemoveCartItem(int userId, int productId)
    {
        var existing = await _cartRepo.GetByUserAndProduct(userId, productId);
        if (existing == null) return;
        await _cartRepo.DeleteAsync(existing);
    }

    // Checkout: valida stock, aplica descuentos por cantidad y por monto, crea order, reduce stock y limpia carrito
    public async Task<Order> Checkout(int userId)
    {
        var cart = await _cartRepo.GetByUserAsync(userId);
        if (!cart.Any()) throw new Exception("Cart empty");

        // Validar stock
        foreach (var ci in cart)
        {
            var p = await _productRepo.GetByIdAsync(ci.ProductId);
            if (p == null) throw new Exception($"Product {ci.ProductId} not found");
            if (p.Stock < ci.Quantity) throw new Exception($"Not enough stock for product {p.Name}");
        }

        var order = new Order { UserId = userId, CreatedAt = DateTime.UtcNow };
        decimal total = 0;

        foreach (var ci in cart)
        {
            var p = await _productRepo.GetByIdAsync(ci.ProductId);
            if (p == null) continue;

            // Descuentos por cantidad
            decimal itemDiscountPercent = 0;
            if (ci.Quantity > 30) itemDiscountPercent = 0.30m;
            else if (ci.Quantity > 20) itemDiscountPercent = 0.20m;
            else if (ci.Quantity > 10) itemDiscountPercent = 0.10m;

            decimal itemTotal = p.Price * ci.Quantity;
            decimal itemDiscount = itemTotal * itemDiscountPercent;
            decimal itemFinal = itemTotal - itemDiscount;

            var oi = new OrderItem
            {
                ProductId = p.Id,
                Quantity = ci.Quantity,
                UnitPrice = p.Price,
                DiscountApplied = itemDiscount
            };
            order.Items.Add(oi);
            total += itemFinal;

            // reducir stock
            p.Stock -= ci.Quantity;
            await _productRepo.UpdateAsync(p);
        }

        // Descuento adicional por monto total
        decimal extraDiscountPercent = total > 100000m ? 0.10m : 0m;
        decimal extraDiscountAmount = total * extraDiscountPercent;
        order.Total = total - extraDiscountAmount;

        // aplicar descuento registrado en order (sumamos como OrderItem.DiscountApplied ya en items)
        // Guardar order
        var saved = await _orderRepo.AddAsync(order);

        // Limpiar carrito
        await _cartRepo.ClearUserCart(userId);

        return saved;
    }
}