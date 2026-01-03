using ReThreaded.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReThread.Application.Interfaces
{
    public interface ICartService
    {
        Task<Cart> GetCartAsync(Guid userId);
        Task AddItemAsync(Guid userId, Guid productId, int quantity);
        Task RemoveItemAsync(Guid userId, Guid productId);
        Task ClearCartAsync(Guid userId);
       // Task<Order> CheckoutAsync(Guid userId);
    }
}
