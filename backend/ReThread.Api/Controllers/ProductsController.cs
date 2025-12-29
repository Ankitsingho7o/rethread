using Microsoft.AspNetCore.Mvc;
using ReThreaded.Application.DTOs.Products;
using ReThreaded.Application.Interfaces;

namespace ReThreaded.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    // GET: api/products
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _productService.GetProductsAsync();
        return Ok(products);
    }

    // GET: api/products/featured
    [HttpGet("featured")]
    public async Task<IActionResult> GetFeaturedProducts()
    {
        var products = await _productService.GetFeaturedProductsAsync();
        return Ok(products);
    }

    // GET: api/products/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(Guid id)
    {
        var product = await _productService.GetProductByIdAsync(id);

        if (product == null)
            return NotFound();

        return Ok(product);
    }


    [HttpPost]
    public async Task<IActionResult> CreateProduct(CreateProductRequest request)
    {
        // TEMP until auth is added
        var designerId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        var productId = await _productService.CreateAsync(request, designerId);

        return CreatedAtAction(nameof(GetProduct), new { id = productId }, null);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        // TEMP until auth is added
        var designerId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        await _productService.DeleteAsync(id, designerId);

        return NoContent(); // 204
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(
    Guid id,
    CreateProductRequest request)
    {
        var designerId = Guid.Parse("11111111-1111-1111-1111-111111111111"); // TEMP

        await _productService.UpdateAsync(id, request, designerId);

        return NoContent();
    }

}