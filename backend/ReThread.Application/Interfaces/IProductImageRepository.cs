using ReThreaded.Domain.Entities;

public interface IProductImageRepository
{
    Task<List<ProductImage>> GetByProductIdAsync(Guid productId);
    Task<ProductImage?> GetByIdAsync(Guid id);
    Task AddAsync(ProductImage image);
    Task DeleteAsync(ProductImage image);
}
