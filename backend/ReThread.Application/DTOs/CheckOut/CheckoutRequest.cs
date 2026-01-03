using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReThread.Application.DTOs.CheckOut
{
    public class CheckoutRequest
    {
        public List<Guid> ProductIds { get; set; } = new();
        public string ShippingAddress { get; set; } = string.Empty;
    }
}
