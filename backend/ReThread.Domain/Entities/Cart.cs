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
        if (userId == Guid.Empty)
            throw new DomainException("User ID is required");

        return new Cart
        {
            UserId = userId
        };
    }

    public void AddItem(Product product)
    {
        if (product == null)
            throw new DomainException("Product is required");

        if (!product.IsAvailable)
            throw new DomainException("Product is not available");

        // Check if already in cart (unique items!)
        if (_items.Any(i => i.ProductId == product.Id))
            throw new DomainException("Product already in cart");

        var cartItem = CartItem.Create(Id, product.Id);
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

    public decimal GetTotal(IEnumerable<Product> products)
    {
        return products
            .Where(p => _items.Any(i => i.ProductId == p.Id))
            .Sum(p => p.Price);
    }
}