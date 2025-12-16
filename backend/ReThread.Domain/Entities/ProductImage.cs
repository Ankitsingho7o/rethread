using ReThreaded.Domain.Common;
using ReThreaded.Domain.Enums;
using ReThreaded.Domain.Exceptions;

namespace ReThreaded.Domain.Entities;

public class ProductImage : BaseEntity
{
    public Guid ProductId { get; private set; }
    public string ImageUrl { get; private set; }
    public ProductImageType ImageType { get; private set; }
    public int DisplayOrder { get; private set; }

    // Navigation
    public Product Product { get; private set; }

    private ProductImage() { }

    public static ProductImage Create(
        Guid productId,
        string imageUrl,
        ProductImageType imageType,
        int displayOrder)
    {
        if (productId == Guid.Empty)
            throw new DomainException("Product ID is required");

        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new DomainException("Image URL is required");

        return new ProductImage
        {
            ProductId = productId,
            ImageUrl = imageUrl,
            ImageType = imageType,
            DisplayOrder = displayOrder
        };
    }

    public void UpdateDisplayOrder(int order)
    {
        DisplayOrder = order;
        SetUpdatedAt();
    }
}