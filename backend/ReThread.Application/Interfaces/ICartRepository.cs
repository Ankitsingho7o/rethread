using ReThreaded.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReThread.Application.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart?> GetByUserIdAsync(Guid userId);
        Task AddAsync(Cart cart);
        Task UpdateAsync(Cart cart);
    }
}
