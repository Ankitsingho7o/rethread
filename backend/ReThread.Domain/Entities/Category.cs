using ReThreaded.Domain.Common;
using ReThreaded.Domain.Exceptions;

namespace ReThreaded.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string? IconUrl { get; private set; }
    public int DisplayOrder { get; private set; }
    public bool IsActive { get; private set; }

    // Navigation
    public ICollection<Product> Products { get; private set; }

    private Category()
    {
        Products = new List<Product>();
    }

    public static Category Create(string name, string? description, int displayOrder)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Category name is required");

        if (name.Length > 100)
            throw new DomainException("Category name too long (max 100 chars)");

        return new Category
        {
            Name = name.Trim(),
            Description = description?.Trim(),
            DisplayOrder = displayOrder,
            IsActive = true
        };
    }

    public void Update(string name, string? description, int displayOrder)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Category name is required");

        Name = name.Trim();
        Description = description?.Trim();
        DisplayOrder = displayOrder;
        SetUpdatedAt();
    }

    public void SetIcon(string iconUrl)
    {
        IconUrl = iconUrl;
        SetUpdatedAt();
    }

    public void Deactivate()
    {
        IsActive = false;
        SetUpdatedAt();
    }

    public void Activate()
    {
        IsActive = true;
        SetUpdatedAt();
    }
}