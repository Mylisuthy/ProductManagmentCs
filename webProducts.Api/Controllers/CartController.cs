using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using webProducts.Application.Interfaces;

namespace webProducts.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/cart")]
public class CartController : ControllerBase
{
    private readonly ICartService _cart;
    public CartController(ICartService cart) { _cart = cart; }

    private int CurrentUserId => int.Parse(User.FindFirst("id")!.Value);

    [HttpGet]
    public async Task<IActionResult> GetCart() => Ok(await _cart.GetUserCart(CurrentUserId));

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddToCartRequest r)
    {
        await _cart.AddOrUpdateCart(CurrentUserId, r.ProductId, r.Quantity);
        return Ok();
    }

    [HttpPost("remove")]
    public async Task<IActionResult> Remove([FromBody] RemoveFromCartRequest r)
    {
        await _cart.RemoveCartItem(CurrentUserId, r.ProductId);
        return Ok();
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout()
    {
        var order = await _cart.Checkout(CurrentUserId);
        return Ok(order);
    }
}

public record AddToCartRequest(int ProductId, int Quantity);
public record RemoveFromCartRequest(int ProductId);