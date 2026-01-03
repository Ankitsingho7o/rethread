using ReThread.Application.Interfaces;
using ReThreaded.Application.Interfaces;
using ReThreaded.Domain.Entities;
using ReThreaded.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReThread.Application.Service
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Order> CheckoutAsync(
            Guid userId,
            IReadOnlyList<Guid> productIds,
            string shippingAddress)
        {
            if (productIds == null || !productIds.Any())
                throw new DomainException("No products selected for checkout");

            var cart = await _cartRepository.GetByUserIdAsync(userId);
            if (cart == null)
                throw new DomainException("Cart not found");

            var order = Order.Create(userId, shippingAddress);

            foreach (var cartItem in cart.Items.Where(i => productIds.Contains(i.ProductId)))
            {
                var product = await _productRepository.GetByIdAsync(cartItem.ProductId);
                if (product == null)
                    throw new DomainException("Product not found");

                if (product.StockQuantity < cartItem.Quantity)
                    throw new DomainException("Insufficient stock");

                // Snapshot price & add to order
                order.AddProduct(
                    product,
                    product.DesignerId,
                    cartItem.Quantity);

                // Reduce stock
                product.DecreaseStock(cartItem.Quantity);

                // Remove purchased item from cart
                cart.RemoveItem(cartItem.ProductId);
            }

            await _orderRepository.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return order;
        }
    }
}
