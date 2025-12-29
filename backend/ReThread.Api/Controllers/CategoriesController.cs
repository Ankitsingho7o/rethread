using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReThreaded.Infrastructure.Persistence;

namespace ReThreaded.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CategoriesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/categories
    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _context.Categories
            .Where(c => c.IsActive)
            .OrderBy(c => c.DisplayOrder)
            .Select(c => new
            {
                c.Id,
                c.Name,
                c.Description,
                c.IconUrl,
                ProductCount = c.Products.Count(p => p.IsAvailable)
            })
            .ToListAsync();

        return Ok(categories);
    }

    // GET: api/categories/{id}/products
    [HttpGet("{id}/products")]
    public async Task<IActionResult> GetCategoryProducts(Guid id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
            return NotFound();

        var products = await _context.Products
            .Where(p => p.CategoryId == id && p.IsAvailable)
            .Include(p => p.Designer)
            .Include(p => p.Images)
            .Select(p => new
            {
                p.Id,
                p.Title,
                p.Price,
                p.Size,
                Designer = new { p.Designer.StoreName },
                Images = p.Images
                    .Where(i => i.ImageType == Domain.Enums.ProductImageType.Primary)
                    .Select(i => new { i.ImageUrl })
                    .FirstOrDefault()
            })
            .ToListAsync();

        return Ok(new
        {
            Category = new { category.Id, category.Name },
            Products = products
        });
    }
}