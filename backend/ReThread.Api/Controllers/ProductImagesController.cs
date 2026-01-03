using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ReThread.Api.Controllers
{
    [ApiController]
    [Route("api/products/{productId}/images")]
    public class ProductImagesController : ControllerBase
    {
        private readonly IProductImageService _service;

        public ProductImagesController(IProductImageService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddImage(
            Guid productId,
            AddProductImageRequest request)
        {
            await _service.AddAsync(productId, request);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetImages(Guid productId)
        {
            var images = await _service.GetAsync(productId);
            return Ok(images);
        }

        [HttpDelete("{imageId}")]
        public async Task<IActionResult> Delete(Guid imageId)
        {
            await _service.DeleteAsync(imageId);
            return NoContent();
        }
    }

}
