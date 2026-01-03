using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReThread.Application.DTOs.CheckOut;
using ReThread.Application.Interfaces;
using ReThreaded.Infrastructure.Repositories;

namespace ReThread.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IOrderRepository  _orderRepository;

        public OrdersController(IOrderService orderService, IOrderRepository orderRepository)
        {
            _orderService = orderService;
            _orderRepository = orderRepository;
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout(CheckoutRequest request)
        {
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111"); // temp

            var order = await _orderService.CheckoutAsync(
                userId,
                request.ProductIds,
                request.ShippingAddress);

            return Ok(order);
        }


        [HttpGet("my")]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111"); // temp
            var orders = await _orderRepository.GetByUserIdAsync(userId);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null || order.UserId != userId)
                return NotFound();

            return Ok(order);
        }

    }
}
