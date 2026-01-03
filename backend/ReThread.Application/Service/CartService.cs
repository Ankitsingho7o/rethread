using ReThread.Application.Interfaces;
using ReThreaded.Application.Interfaces;
using ReThreaded.Domain.Entities;
using ReThreaded.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReThreaded.Application.Service
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        //private readonly IOrderService _orderService; // later
        private readonly IUnitOfWork _unitOfWork;

        public CartService(
            ICartRepository cartRepository,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork
           )
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            //_orderService = orderService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Cart> GetCartAsync(Guid userId)
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);

            if (cart == null)
            {
                cart = Cart.Create(userId);
                await _cartRepository.AddAsync(cart);
                await _unitOfWork.SaveChangesAsync();
            }

            return cart;
        }

        public async Task AddItemAsync(Guid userId, Guid productId, int quantity)
        {
            var cart = await GetCartAsync(userId);

            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                throw new DomainException("Product not found");

            cart.AddItem(product, quantity);

            await _cartRepository.UpdateAsync(cart);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RemoveItemAsync(Guid userId, Guid productId)
        {
            var cart = await GetCartAsync(userId);

            if (cart.UserId != userId)
                throw new DomainException("Unauthorized access to cart");

            cart.RemoveItem(productId);

            await _cartRepository.UpdateAsync(cart);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ClearCartAsync(Guid userId)
        {
            var cart = await GetCartAsync(userId);

            cart.Clear();

            await _cartRepository.UpdateAsync(cart);
            await _unitOfWork.SaveChangesAsync();
        }

        //public async Task<Order> CheckoutAsync(Guid userId)
        //{
        //    var cart = await GetCartAsync(userId);

        //    if (!cart.Items.Any())
        //        throw new DomainException("Cart is empty");

        //    // OrderService will:
        //    // - Recheck stock
        //    // - Calculate price dynamically
        //    // - Reduce stock
        //    // - Create Order
        //   //var order = await _orderService.CreateOrderFromCartAsync(cart);
          
        //    cart.Clear();

        //    await _cartRepository.UpdateAsync(cart);
        //    await _unitOfWork.SaveChangesAsync();

        //    return order;
        //}
    }

}
