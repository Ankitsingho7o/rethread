using ReThreaded.Domain.Enums;

namespace ReThreaded.Application.DTOs.Products;

public class CreateProductRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string TransformationStory { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Size { get; set; } = string.Empty;
    public ProductCondition Condition { get; set; }
    public int StockQuantity { get; set; }  
    public Guid CategoryId { get; set; }
    public string? OriginalBrand { get; set; }
}

