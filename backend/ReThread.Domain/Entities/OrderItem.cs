using ReThreaded.Domain.Common;
using ReThreaded.Domain.Exceptions;

namespace ReThreaded.Domain.Entities;

public class OrderItem : BaseEntity
{
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public Guid DesignerId { get; private set; }
    public decimal Price { get; private set; } // Snapshot!

    // Navigation
    public Order Order { get; private set; }
    public Product Product { get; private set; }
    public DesignerProfile Designer { get; private set; }

    private OrderItem() { }

    public static OrderItem Create(
        Guid orderId,
        Guid productId,
        Guid designerId,
        decimal price)
    {
        if (orderId == Guid.Empty)
            throw new DomainException("Order ID is required");

        if (productId == Guid.Empty)
            throw new DomainException("Product ID is required");

        if (designerId == Guid.Empty)
            throw new DomainException("Designer ID is required");

        if (price <= 0)
            throw new DomainException("Price must be greater than 0");

        return new OrderItem
        {
            OrderId = orderId,
            ProductId = productId,
            DesignerId = designerId,
            Price = price // SNAPSHOT!
        };
    }
}