using ReThreaded.Domain.Entities;

namespace ReThreaded.Application.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetAvailableProductsAsync();
    Task<Product?> GetByIdWithDetailsAsync(Guid id);
    Task<List<Product>> GetFeaturedProductsAsync(int count);
    Task<List<Product>> GetByCategoryAsync(Guid categoryId);
    Task<Product?> GetByIdAsync(Guid id);
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Product product);
    Task<bool> ExistsAsync(Guid id);

    Task<(IReadOnlyList<Product>, int)> SearchAsync(
       string? query,
       Guid? categoryId,
       decimal? minPrice,
       decimal? maxPrice,
       bool onlyAvailable,
       int page,
       int pageSize);
}