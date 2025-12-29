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

    public int StockQuantity { get; private set; }
    public bool IsAvailable { get; private set; }
    public bool IsFeatured { get; private set; }
    public bool IsDeleted { get; private set; }

    public int ViewCount { get; private set; }
    public DateTime? SoldAt { get; private set; }

    // Navigation
    public DesignerProfile Designer { get; private set; }
    public Category Category { get; private set; }

    private readonly List<ProductImage> _images = new();
    public IReadOnlyCollection<ProductImage> Images => _images.AsReadOnly();

    private Product() { }

    // -------------------- CREATE --------------------

    public static Product Create(
        string title,
        string description,
        string transformationStory,
        decimal price,
        string size,
        ProductCondition condition,
        Guid designerId,
        Guid categoryId,
        int stockQuantity = 1,
        string? originalBrand = null)
    {
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

        if (stockQuantity < 1)
            throw new DomainException("Stock quantity must be at least 1");

        var product = new Product
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
            StockQuantity = stockQuantity,
            IsFeatured = false,
            IsDeleted = false,
            ViewCount = 0
        };

        product.RecalculateAvailability();
        return product;
    }

    // -------------------- UPDATE CORE DETAILS --------------------

    public void Update(
        string title,
        string description,
        string transformationStory,
        decimal price,
        string size,
        ProductCondition condition,
        Guid categoryId,
        string? originalBrand)
    {
        if (IsDeleted)
            throw new DomainException("Deleted product cannot be updated");

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

        if (categoryId == Guid.Empty)
            throw new DomainException("Category ID is required");

        Title = title.Trim();
        Description = description.Trim();
        TransformationStory = transformationStory.Trim();
        Price = price;
        Size = size.Trim().ToUpperInvariant();
        Condition = condition;
        CategoryId = categoryId;
        OriginalBrand = originalBrand?.Trim();

        SetUpdatedAt();
    }

    // -------------------- STOCK --------------------

    public void UpdateStock(int newQuantity)
    {
        if (IsDeleted)
            throw new DomainException("Deleted product stock cannot be changed");

        if (newQuantity < 0)
            throw new DomainException("Stock cannot be negative");

        StockQuantity = newQuantity;
        RecalculateAvailability();

        if (!IsAvailable)
            IsFeatured = false;

        SetUpdatedAt();
    }

    // -------------------- FEATURED --------------------

    public void MarkAsFeatured()
    {
        if (IsDeleted)
            throw new DomainException("Deleted products cannot be featured");

        if (!IsAvailable)
            throw new DomainException("Only available products can be featured");

        if (!HasRequiredImages())
            throw new DomainException("Product must have required images to be featured");

        IsFeatured = true;
        SetUpdatedAt();
    }

    public void RemoveFromFeatured()
    {
        IsFeatured = false;
        SetUpdatedAt();
    }

    // -------------------- IMAGES --------------------

    public void AddImage(string imageUrl, ProductImageType imageType, int displayOrder)
    {
        if (IsDeleted)
            throw new DomainException("Cannot add images to deleted product");

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

        if (!HasRequiredImages())
            IsFeatured = false;

        SetUpdatedAt();
    }

    public bool HasRequiredImages()
    {
        return _images.Any(i => i.ImageType == ProductImageType.Primary)
            && _images.Any(i => i.ImageType == ProductImageType.Gallery);
    }

    // -------------------- VIEW & DELETE --------------------

    public void IncrementViewCount()
    {
        if (IsDeleted) return;
        ViewCount++;
    }

    public void SoftDelete()
    {
        IsDeleted = true;
        IsFeatured = false;
        SetUpdatedAt();
    }

    // -------------------- INTERNAL --------------------

    private void RecalculateAvailability()
    {
        IsAvailable = StockQuantity > 0;

        if (!IsAvailable && SoldAt == null)
            SoldAt = DateTime.UtcNow;

        if (IsAvailable)
            SoldAt = null;
    }
}
