using ReThreaded.Domain.Common;
using ReThreaded.Domain.Exceptions;

namespace ReThreaded.Domain.Entities;

public class CartItem : BaseEntity
{
    public Guid CartId { get; private set; }
    public Guid ProductId { get; private set; }

    // Navigation
    public Cart Cart { get; private set; }
    public Product Product { get; private set; }

    private CartItem() { }

    public static CartItem Create(Guid cartId, Guid productId)
    {
        if (cartId == Guid.Empty)
            throw new DomainException("Cart ID is required");

        if (productId == Guid.Empty)
            throw new DomainException("Product ID is required");

        return new CartItem
        {
            CartId = cartId,
            ProductId = productId
        };
    }
}