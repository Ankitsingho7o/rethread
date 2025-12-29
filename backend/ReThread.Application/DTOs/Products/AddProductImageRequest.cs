using ReThreaded.Domain.Enums;

public class AddProductImageRequest
{
    public string ImageUrl { get; set; } = string.Empty;
    public ProductImageType ImageType { get; set; }
    public int DisplayOrder { get; set; }
}
