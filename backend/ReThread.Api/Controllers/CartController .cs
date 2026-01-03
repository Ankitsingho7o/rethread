using Microsoft.AspNetCore.Mvc;
using ReThread.Application.Interfaces;

namespace ReThread.Api.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        
       
            private readonly ICartService _cartService;

            public CartController(ICartService cartService)
            {
                _cartService = cartService;
            }

            [HttpGet]
            public async Task<IActionResult> GetCart()
            {
                var userId = Guid.Parse("11111111-1111-1111-1111-111111111111"); // temp / middleware later
                var cart = await _cartService.GetCartAsync(userId);
                return Ok(cart);
            }

            [HttpPost("items")]
            public async Task<IActionResult> AddItem(Guid productId, int quantity)
            {
                var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
                await _cartService.AddItemAsync(userId, productId, quantity);
                return NoContent();
            }

            [HttpDelete("items/{productId}")]
            public async Task<IActionResult> RemoveItem(Guid productId)
            {
                var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
                await _cartService.RemoveItemAsync(userId, productId);
                return NoContent();
            }

            //[HttpPost("checkout")]
            //public async Task<IActionResult> Checkout()
            //{
            //    var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            //    var order = await _cartService.CheckoutAsync(userId);
            //    return Ok(order);
            //}
        }

    }

