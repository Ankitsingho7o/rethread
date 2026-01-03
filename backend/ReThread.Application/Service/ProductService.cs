using ReThread.Application.DTOs.Search;
using ReThread.Domain.Common;
using ReThreaded.Application.DTOs.Products;
using ReThreaded.Application.Interfaces;
using ReThreaded.Domain.Entities;
using ReThreaded.Domain.Exceptions;

namespace ReThreaded.Application.Services
{

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductDto>> GetProductsAsync()
        {
            var products = await _productRepository.GetAvailableProductsAsync();

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                TransformationStory = p.TransformationStory,
                Price = p.Price,
                Size = p.Size,
                OriginalBrand = p.OriginalBrand,
                Condition = (int)p.Condition,
                StockQuantity = p.StockQuantity,
                IsFeatured = p.IsFeatured,
                ViewCount = p.ViewCount,
                Designer = new DesignerSummaryDto
                {
                    Id = p.Designer.Id,
                    StoreName = p.Designer.StoreName
                },
                Category = new CategorySummaryDto
                {
                    Id = p.Category.Id,
                    Name = p.Category.Name
                },
                Images = p.Images.Select(i => new ProductImageDto
                {
                    Id = i.Id,
                    ImageUrl = i.ImageUrl,
                    ImageType = (int)i.ImageType,
                    DisplayOrder = i.DisplayOrder
                })
                .OrderBy(i => i.DisplayOrder)
                .ToList()
            }).ToList();
        }

        public async Task<ProductDto?> GetProductByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdWithDetailsAsync(id);

            if (product == null)
                return null;

            return new ProductDto
            {
                Id = product.Id,
                Title = product.Title,
                Description = product.Description,
                TransformationStory = product.TransformationStory,
                Price = product.Price,
                Size = product.Size,
                OriginalBrand = product.OriginalBrand,
                Condition = (int)product.Condition,
                StockQuantity = product.StockQuantity,
                IsFeatured = product.IsFeatured,
                ViewCount = product.ViewCount,
                Designer = new DesignerSummaryDto
                {
                    Id = product.Designer.Id,
                    StoreName = product.Designer.StoreName
                },
                Category = new CategorySummaryDto
                {
                    Id = product.Category.Id,
                    Name = product.Category.Name
                },
                Images = product.Images.Select(i => new ProductImageDto
                {
                    Id = i.Id,
                    ImageUrl = i.ImageUrl,
                    ImageType = (int)i.ImageType,
                    DisplayOrder = i.DisplayOrder
                })
                .OrderBy(i => i.DisplayOrder)
                .ToList()
            };
        }

        public async Task<List<ProductDto>> GetFeaturedProductsAsync()
        {
            var products = await _productRepository.GetFeaturedProductsAsync(6);

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                TransformationStory = p.TransformationStory,
                Price = p.Price,
                Size = p.Size,
                OriginalBrand = p.OriginalBrand,
                Condition = (int)p.Condition,
                StockQuantity = p.StockQuantity,
                IsFeatured = p.IsFeatured,
                ViewCount = p.ViewCount,
                Designer = new DesignerSummaryDto
                {
                    Id = p.Designer.Id,
                    StoreName = p.Designer.StoreName
                },
                Category = new CategorySummaryDto
                {
                    Id = p.Category.Id,
                    Name = p.Category.Name
                },
                Images = p.Images.Select(i => new ProductImageDto
                {
                    Id = i.Id,
                    ImageUrl = i.ImageUrl,
                    ImageType = (int)i.ImageType,
                    DisplayOrder = i.DisplayOrder
                })
                .OrderBy(i => i.DisplayOrder)
                .ToList()
            }).ToList();
        }

        public async Task<List<ProductDto>> GetProductsByCategoryAsync(Guid categoryId)
        {
            var products = await _productRepository.GetByCategoryAsync(categoryId);

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                TransformationStory = p.TransformationStory,
                Price = p.Price,
                Size = p.Size,
                OriginalBrand = p.OriginalBrand,
                Condition = (int)p.Condition,
                StockQuantity = p.StockQuantity,
                IsFeatured = p.IsFeatured,
                ViewCount = p.ViewCount,
                Designer = new DesignerSummaryDto
                {
                    Id = p.Designer.Id,
                    StoreName = p.Designer.StoreName
                },
                Category = new CategorySummaryDto
                {
                    Id = p.Category.Id,
                    Name = p.Category.Name
                },
                Images = p.Images.Select(i => new ProductImageDto
                {
                    Id = i.Id,
                    ImageUrl = i.ImageUrl,
                    ImageType = (int)i.ImageType,
                    DisplayOrder = i.DisplayOrder
                })
                .OrderBy(i => i.DisplayOrder)
                .ToList()
            }).ToList();
        }

        public async Task<Guid> CreateAsync(CreateProductRequest request, Guid designerId)
        {
            var product = Product.Create(
                request.Title,
                request.Description,
                request.TransformationStory,
                request.Price,
                request.Size,
                request.Condition,
                designerId,
                request.CategoryId,
                request.StockQuantity,
                request.OriginalBrand
            );

            await _productRepository.AddAsync(product);
            return product.Id;
        }

        public async Task DeleteAsync(Guid productId, Guid designerId)
        {
            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
                throw new DomainException("Product not found");

            if (product.DesignerId != designerId)
                throw new DomainException("Not authorized to delete this product");

            product.SoftDelete();

            await _productRepository.UpdateAsync(product);
        }
        public async Task UpdateAsync(
        Guid productId,
        CreateProductRequest request,
        Guid designerId)
        {
            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
                throw new DomainException("Product not found");

            if (product.DesignerId != designerId)
                throw new DomainException("Not authorized");

            product.Update(
                request.Title,
                request.Description,
                request.TransformationStory,
                request.Price,
                request.Size,
                request.Condition,
                request.CategoryId,
                request.OriginalBrand
            );

            await _productRepository.UpdateAsync(product);
        }
        public async Task<PagedResult<ProductSearchResponseDto>> SearchAsync(
        ProductSearchRequest request)
        {
            var (products, totalCount) = await _productRepository.SearchAsync(
                request.Query,
                request.CategoryId,
                request.MinPrice,
                request.MaxPrice,
                request.OnlyAvailable,
                request.Page,
                request.PageSize);

            var items = products.Select(p => new ProductSearchResponseDto
            {
                Id = p.Id,
                Title = p.Title,
                Price = p.Price,
                Size = p.Size,
                IsAvailable = p.IsAvailable,
                ThumbnailImageUrl = p.Images
                    .OrderBy(i => i.DisplayOrder)
                    .FirstOrDefault()?.ImageUrl,
                DesignerStoreName = p.Designer.StoreName
            }).ToList();

            return new PagedResult<ProductSearchResponseDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };
        }
    }
}