using ReThreaded.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReThread.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task AddAsync(Order order);
        Task<Order?> GetByIdAsync(Guid orderId);
        Task<IReadOnlyList<Order>> GetByUserIdAsync(Guid userId);
    }

}
