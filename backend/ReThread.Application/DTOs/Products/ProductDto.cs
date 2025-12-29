namespace ReThreaded.Application.DTOs.Products;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string TransformationStory { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Size { get; set; } = string.Empty;
    public string? OriginalBrand { get; set; }
    public int Condition { get; set; }
    public int StockQuantity { get; set; }  
    public bool IsFeatured { get; set; }
    public int ViewCount { get; set; }

    public DesignerSummaryDto Designer { get; set; } = null!;
    public CategorySummaryDto Category { get; set; } = null!;
    public List<ProductImageDto> Images { get; set; } = new();
}

public class DesignerSummaryDto
{
    public Guid Id { get; set; }
    public string StoreName { get; set; } = string.Empty;
}

public class CategorySummaryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class ProductImageDto
{
    public Guid Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int ImageType { get; set; }
    public int DisplayOrder { get; set; }
}