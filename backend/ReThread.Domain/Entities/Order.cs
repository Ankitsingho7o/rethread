using ReThreaded.Domain.Common;
using ReThreaded.Domain.Enums;
using ReThreaded.Domain.Exceptions;

namespace ReThreaded.Domain.Entities;

public class Order : BaseEntity
{
    public string OrderNumber { get; private set; }
    public Guid UserId { get; private set; }
    public decimal TotalAmount { get; private set; }
    public OrderStatus Status { get; private set; }
    public string? PaymentIntentId { get; private set; }
    public string ShippingAddress { get; private set; }
    public string? TrackingNumber { get; private set; }
    public DateTime? PaidAt { get; private set; }
    public DateTime? ShippedAt { get; private set; }
    public DateTime? DeliveredAt { get; private set; }

    // Navigation
    public User User { get; private set; }

    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    private Order() { }

    public static Order Create(Guid userId, string shippingAddress)
    {
        if (userId == Guid.Empty)
            throw new DomainException("User ID is required");

        if (string.IsNullOrWhiteSpace(shippingAddress))
            throw new DomainException("Shipping address is required");

        return new Order
        {
            OrderNumber = GenerateOrderNumber(),
            UserId = userId,
            ShippingAddress = shippingAddress.Trim(),
            Status = OrderStatus.Pending,
            TotalAmount = 0
        };
    }

    public void AddProduct(Product product, Guid designerId)
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException("Cannot modify order after pending");

        if (!product.IsAvailable)
            throw new DomainException("Product not available");

        var orderItem = OrderItem.Create(Id, product.Id, designerId, product.Price);
        _items.Add(orderItem);

        RecalculateTotal();
        SetUpdatedAt();
    }

    public void MarkAsPaid(string paymentIntentId)
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException("Order already processed");

        if (string.IsNullOrWhiteSpace(paymentIntentId))
            throw new DomainException("Payment intent ID is required");

        Status = OrderStatus.Paid;
        PaymentIntentId = paymentIntentId;
        PaidAt = DateTime.UtcNow;
        SetUpdatedAt();
    }

    public void MarkAsProcessing()
    {
        if (Status != OrderStatus.Paid)
            throw new DomainException("Order must be paid first");

        Status = OrderStatus.Processing;
        SetUpdatedAt();
    }

    public void MarkAsShipped(string trackingNumber)
    {
        if (Status != OrderStatus.Paid && Status != OrderStatus.Processing)
            throw new DomainException("Cannot ship unpaid order");

        if (string.IsNullOrWhiteSpace(trackingNumber))
            throw new DomainException("Tracking number is required");

        Status = OrderStatus.Shipped;
        TrackingNumber = trackingNumber.Trim();
        ShippedAt = DateTime.UtcNow;
        SetUpdatedAt();
    }

    public void MarkAsDelivered()
    {
        if (Status != OrderStatus.Shipped)
            throw new DomainException("Order not shipped yet");

        Status = OrderStatus.Delivered;
        DeliveredAt = DateTime.UtcNow;
        SetUpdatedAt();
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Shipped || Status == OrderStatus.Delivered)
            throw new DomainException("Cannot cancel shipped/delivered order");

        Status = OrderStatus.Cancelled;
        SetUpdatedAt();
    }

    private void RecalculateTotal()
    {
        TotalAmount = _items.Sum(i => i.Price);
    }

    private static string GenerateOrderNumber()
    {
        return $"RT-{DateTime.UtcNow:yyyy}-{Guid.NewGuid().ToString()[..8].ToUpper()}";
    }
}