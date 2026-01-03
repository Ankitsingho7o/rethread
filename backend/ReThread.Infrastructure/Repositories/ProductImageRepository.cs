using Microsoft.EntityFrameworkCore;
using ReThreaded.Application.Interfaces;
using ReThreaded.Domain.Entities;
using ReThreaded.Infrastructure.Persistence;
namespace ReThreaded.Infrastructure.Repositories
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductImageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductImage>> GetByProductIdAsync(Guid productId)
        {
            return await _context.ProductImages
                .Where(i => i.ProductId == productId)
                .OrderBy(i => i.DisplayOrder)
                .ToListAsync();
        }

        public async Task<ProductImage?> GetByIdAsync(Guid id)
        {
            return await _context.ProductImages.FindAsync(id);
        }

        public async Task AddAsync(ProductImage image)
        {
            await _context.ProductImages.AddAsync(image);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ProductImage image)
        {
            _context.ProductImages.Remove(image);
            await _context.SaveChangesAsync();
        }
    }
}