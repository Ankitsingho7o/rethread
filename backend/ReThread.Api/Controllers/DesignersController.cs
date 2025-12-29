using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ReThreaded.Domain.Entities;
using ReThreaded.Infrastructure.Persistence;
using System.ComponentModel.Design;
using System.Net.Sockets;
using static System.Collections.Specialized.BitVector32;

namespace ReThreaded.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DesignersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DesignersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/designers
    [HttpGet]
    public async Task<IActionResult> GetDesigners()
    {
        var designers = await _context.DesignerProfiles
            .Include(d => d.Products)
            .Select(d => new
            {
                d.Id,
                d.StoreName,
                d.Bio,
                d.BannerImageUrl,
                d.TotalSales,
                d.AverageRating,
                ProductCount = d.Products.Count(p => p.IsAvailable)
            })
            .OrderByDescending(d => d.TotalSales)
            .ToListAsync();

        return Ok(designers);
    }

    // GET: api/designers/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDesigner(Guid id)
    {
        var designer = await _context.DesignerProfiles
            .Where(d => d.Id == id)
            .Select(d => new
            {
                d.Id,
                d.StoreName,
                d.Bio,
                d.BannerImageUrl,
                d.InstagramHandle,
                d.WebsiteUrl,
                d.TotalSales,
                d.AverageRating,
                Products = d.Products
                    .Where(p => p.IsAvailable)
                    .Select(p => new
                    {
                        p.Id,
                        p.Title,
                        p.Price,
                        p.Size,
                        MainImage = p.Images
                            .Where(i => i.ImageType == Domain.Enums.ProductImageType.Primary)
                            .Select(i => i.ImageUrl)
                            .FirstOrDefault()
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();

        if (designer == null)
            return NotFound();

        return Ok(designer);
    }
}
