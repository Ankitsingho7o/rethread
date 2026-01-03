using ReThreaded.Domain.Common;
using ReThreaded.Domain.Exceptions;

namespace ReThreaded.Domain.Entities
{

    public class OrderItem : BaseEntity
    {
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public Guid DesignerId { get; private set; }
        public decimal UnitPrice { get; private set; } // snapshot
        public int Quantity { get; private set; }
        public decimal TotalPrice => UnitPrice * Quantity;
        public Order Order { get; private set; }
        public Product Product { get; private set; }
        public DesignerProfile Designer { get; private set; }
        private OrderItem() { }

        public static OrderItem Create(
            Guid orderId,
            Guid productId,
            Guid designerId,
            decimal unitPrice,
            int quantity)
        {
            if (orderId == Guid.Empty)
                throw new DomainException("Order ID is required");

            if (productId == Guid.Empty)
                throw new DomainException("Product ID is required");

            if (designerId == Guid.Empty)
                throw new DomainException("Designer ID is required");

            if (unitPrice <= 0)
                throw new DomainException("Invalid price");

            if (quantity <= 0)
                throw new DomainException("Quantity must be greater than zero");

            return new OrderItem
            {
                OrderId = orderId,
                ProductId = productId,
                DesignerId = designerId,
                UnitPrice = unitPrice,
                Quantity = quantity
            };
        }
    }
}