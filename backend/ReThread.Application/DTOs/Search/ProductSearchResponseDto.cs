using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReThread.Application.DTOs.Search
{
    public class ProductSearchResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
        public string Size { get; set; } = null!;
        public bool IsAvailable { get; set; }
        public string? ThumbnailImageUrl { get; set; }
        public string DesignerStoreName { get; set; } = null!;
    }
}
