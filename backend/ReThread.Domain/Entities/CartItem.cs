using ReThreaded.Domain.Common;
using ReThreaded.Domain.Exceptions;

namespace ReThreaded.Domain.Entities;

public class CartItem : BaseEntity
{
    public Guid CartId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }

    // Navigation
    public Cart Cart { get; private set; }
    public Product Product { get; private set; }

    private CartItem() { }

    public static CartItem Create(Guid cartId, Guid productId, int quantity)
    {
        if (quantity <= 0 || quantity > 2)
            throw new DomainException("Invalid quantity");

        return new CartItem
        {
            CartId = cartId,
            ProductId = productId,
            Quantity = quantity
        };
    }


}