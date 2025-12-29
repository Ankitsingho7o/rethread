
using ReThreaded.Application.DTOs.Products;

public interface IProductImageService
{
    Task AddAsync(Guid productId, AddProductImageRequest request);
    Task<List<ProductImageDto>> GetAsync(Guid productId);
    Task DeleteAsync(Guid imageId);
}
