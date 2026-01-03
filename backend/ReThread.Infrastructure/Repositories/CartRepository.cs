using Microsoft.EntityFrameworkCore;
using ReThread.Application.Interfaces;
using ReThreaded.Domain.Entities;
using ReThreaded.Infrastructure.Persistence;
namespace ReThreaded.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cart?> GetByUserIdAsync(Guid userId)
        {
            return await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task AddAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
        }

        public Task UpdateAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            return Task.CompletedTask;
        }
    }
}