using ReThreaded.Domain.Common;
using ReThreaded.Domain.Enums;
using ReThreaded.Domain.Exceptions;

namespace ReThreaded.Domain.Entities;

public class Product : BaseEntity
{
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string TransformationStory { get; private set; }
    public decimal Price { get; private set; }
    public string? OriginalBrand { get; private set; }
    public string Size { get; private set; }
    public ProductCondition Condition { get; private set; }
    public Guid DesignerId { get; private set; }
    public Guid CategoryId { get; private set; }
    public bool IsAvailable { get; private set; }
    public bool IsFeatured { get; private set; }
    public int ViewCount { get; private set; }
    public DateTime? SoldAt { get; private set; }

    // Navigation
    public DesignerProfile Designer { get; private set; }
    public Category Category { get; private set; }

    private readonly List<ProductImage> _images = new();
    public IReadOnlyCollection<ProductImage> Images => _images.AsReadOnly();

    private Product() { }

    public static Product Create(
        string title,
        string description,
        string transformationStory,
        decimal price,
        string size,
        ProductCondition condition,
        Guid designerId,
        Guid categoryId,
        string? originalBrand = null)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Product title is required");

        if (title.Length > 200)
            throw new DomainException("Title too long (max 200 chars)");

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Description is required");

        if (string.IsNullOrWhiteSpace(transformationStory))
            throw new DomainException("Transformation story is required");

        if (price <= 0)
            throw new DomainException("Price must be greater than 0");

        if (string.IsNullOrWhiteSpace(size))
            throw new DomainException("Size is required");

        if (designerId == Guid.Empty)
            throw new DomainException("Designer ID is required");

        if (categoryId == Guid.Empty)
            throw new DomainException("Category ID is required");

        return new Product
        {
            Title = title.Trim(),
            Description = description.Trim(),
            TransformationStory = transformationStory.Trim(),
            Price = price,
            Size = size.Trim().ToUpperInvariant(),
            Condition = condition,
            DesignerId = designerId,
            CategoryId = categoryId,
            OriginalBrand = originalBrand?.Trim(),
            IsAvailable = true,
            IsFeatured = false,
            ViewCount = 0
        };
    }

    public void Update(
        string title,
        string description,
        string transformationStory,
        decimal price,
        string size,
        ProductCondition condition,
        string? originalBrand)
    {
        if (!IsAvailable)
            throw new DomainException("Cannot update sold product");

        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Product title is required");

        if (price <= 0)
            throw new DomainException("Price must be greater than 0");

        Title = title.Trim();
        Description = description.Trim();
        TransformationStory = transformationStory.Trim();
        Price = price;
        Size = size.Trim().ToUpperInvariant();
        Condition = condition;
        OriginalBrand = originalBrand?.Trim();
        SetUpdatedAt();
    }

    public void AddImage(string imageUrl, ProductImageType imageType, int displayOrder)
    {
        var image = ProductImage.Create(Id, imageUrl, imageType, displayOrder);
        _images.Add(image);
        SetUpdatedAt();
    }

    public void RemoveImage(Guid imageId)
    {
        var image = _images.FirstOrDefault(i => i.Id == imageId);
        if (image == null)
            throw new DomainException("Image not found");

        _images.Remove(image);
        SetUpdatedAt();
    }

    public void MarkAsSold()
    {
        if (!IsAvailable)
            throw new DomainException("Product already sold");

        // Validate has required images
        if (!_images.Any(i => i.ImageType == ProductImageType.Before))
            throw new DomainException("Product must have at least one Before image");

        if (!_images.Any(i => i.ImageType == ProductImageType.After))
            throw new DomainException("Product must have at least one After image");

        IsAvailable = false;
        SoldAt = DateTime.UtcNow;
        SetUpdatedAt();
    }

    public void MarkAsFeatured()
    {
        IsFeatured = true;
        SetUpdatedAt();
    }

    public void RemoveFromFeatured()
    {
        IsFeatured = false;
        SetUpdatedAt();
    }

    public void IncrementViewCount()
    {
        ViewCount++;
    }

    // Validation helper
    public bool HasRequiredImages()
    {
        return _images.Any(i => i.ImageType == ProductImageType.Before) &&
               _images.Any(i => i.ImageType == ProductImageType.After);
    }
}