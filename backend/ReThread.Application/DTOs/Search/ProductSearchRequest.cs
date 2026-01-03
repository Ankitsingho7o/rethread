using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReThread.Application.DTOs.Search
{
    
    public class ProductSearchRequest
    {
        public string? Query { get; set; }
        public Guid? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool OnlyAvailable { get; set; } = true;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
    }

}
