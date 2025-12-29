
namespace ReThread.Application.DTOs.Products
{
    public class ProductImageDto
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int ImageType { get; set; }
        public int DisplayOrder { get; set; }
    }

}