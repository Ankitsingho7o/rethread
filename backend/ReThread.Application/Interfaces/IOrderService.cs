using ReThreaded.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReThread.Application.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CheckoutAsync(
            Guid userId,
            IReadOnlyList<Guid> productIds,
            string shippingAddress);
    }
}
