using ReThreaded.Domain.Common;
using ReThreaded.Domain.Exceptions;

namespace ReThreaded.Domain.Entities;

public class Cart : BaseEntity
{
    public Guid UserId { get; private set; }

    // Navigation
    public User User { get; private set; }

    private readonly List<CartItem> _items = new();
    public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();

    private Cart() { }

    public static Cart Create(Guid userId)
    {
        

        return new Cart
        {
            UserId = userId
        };
    }

    public void AddItem(Product product, int quantity)
    {
        if (product == null)
            throw new DomainException("Product is required");

        if (quantity <= 0 || quantity > 2)
            throw new DomainException("Quantity must be between 1 and 2");

        if (!product.IsAvailable)
            throw new DomainException("Product is not available");

        if (product.StockQuantity < quantity)
            throw new DomainException("Insufficient stock");

        if (product.DesignerId == UserId)
            throw new DomainException("You cannot add your own product to cart");

        if (_items.Any(i => i.ProductId == product.Id))
            throw new DomainException("Product already in cart");

        var cartItem = CartItem.Create(Id, product.Id, quantity);
        _items.Add(cartItem);
        SetUpdatedAt();
    }


    public void RemoveItem(Guid productId)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        if (item == null)
            throw new DomainException("Item not found in cart");

        _items.Remove(item);
        SetUpdatedAt();
    }

    public void Clear()
    {
        _items.Clear();
        SetUpdatedAt();
    }

    
}