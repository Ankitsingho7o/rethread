using ReThread.Application.DTOs.Search;
using ReThread.Domain.Common;
using ReThreaded.Application.DTOs.Products;
using ReThreaded.Domain.Entities;

namespace ReThreaded.Application.Interfaces;

public interface IProductService
{
    Task<List<ProductDto>> GetProductsAsync();
    Task<ProductDto?> GetProductByIdAsync(Guid id);
    Task<List<ProductDto>> GetFeaturedProductsAsync();
    Task<List<ProductDto>> GetProductsByCategoryAsync(Guid categoryId);
    Task DeleteAsync(Guid productId, Guid designerId);
    Task<Guid> CreateAsync(CreateProductRequest request, Guid designerId);

    Task UpdateAsync(Guid productId, CreateProductRequest request, Guid designerId);
    Task<PagedResult<ProductSearchResponseDto>> SearchAsync(
            ProductSearchRequest request);
}

}
