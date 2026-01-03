using ReThreaded.Application.DTOs.Products;
using ReThreaded.Application.Interfaces;
using ReThreaded.Domain.Entities;
using ReThreaded.Domain.Exceptions;

namespace ReThreaded.Application.Service
{
    public class ProductImageService : IProductImageService
    {
        private readonly IProductImageRepository _imageRepo;
        private readonly IProductRepository _productRepo;

        public ProductImageService(
            IProductImageRepository imageRepo,
            IProductRepository productRepo)
        {
            _imageRepo = imageRepo;
            _productRepo = productRepo;
        }

        public async Task AddAsync(Guid productId, AddProductImageRequest request)
        {
            var product = await _productRepo.GetByIdAsync(productId);
            if (product == null)
                throw new DomainException("Product not found");

            var image = ProductImage.Create(
                productId,
                request.ImageUrl,
                request.ImageType,
                request.DisplayOrder
            );

            await _imageRepo.AddAsync(image);
        }

        public async Task<List<ProductImageDto>> GetAsync(Guid productId)
        {
            var images = await _imageRepo.GetByProductIdAsync(productId);

            return images.Select(i => new ProductImageDto
            {
                Id = i.Id,
                ImageUrl = i.ImageUrl,
                ImageType = (int)i.ImageType,
                DisplayOrder = i.DisplayOrder
            }).ToList();
        }

        public async Task DeleteAsync(Guid imageId)
        {
            var image = await _imageRepo.GetByIdAsync(imageId);
            if (image == null)
                throw new DomainException("Image not found");

            await _imageRepo.DeleteAsync(image);
        }
    }
}