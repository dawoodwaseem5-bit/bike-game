using backend.DTOs;
using backend.Models;
using backend.Repositories;

namespace backend.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<object> GetProductsPaginatedAsync(int page, int pageSize, string search)
        {
            var (totalItems, items) = await _productRepository.GetPaginatedAsync(page, pageSize, search);

            return new
            {
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                Data = items
            };
        }

        public async Task<ProductResponseDto> CreateProductAsync(ProductCreateDto dto, string username)
        {
            var product = new Product
            {
                Name = dto.Name,
                UnitPrice = dto.UnitPrice,
                StockQuantity = dto.StockQuantity,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = username,
                IsActive = true,
                IsDeleted = false
            };

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();

            return new ProductResponseDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                UnitPrice = product.UnitPrice,
                StockQuantity = product.StockQuantity,
                IsActive = product.IsActive,
                CreatedAt = product.CreatedAt
            };
        }

        public async Task<(bool Success, string Message, ProductResponseDto? UpdatedProduct)> UpdateProductAsync(int id, ProductUpdateDto dto, string username)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return (false, "Product not found.", null);

            product.Name = dto.Name;
            product.UnitPrice = dto.UnitPrice;
            product.StockQuantity = dto.StockQuantity;
            product.IsActive = dto.IsActive;

            product.UpdatedAt = DateTime.UtcNow;
            product.UpdatedBy = username;

            await _productRepository.SaveChangesAsync();

            var responseDto = new ProductResponseDto
            {
                ProductId = product.ProductId,
                Name = product.Name,
                UnitPrice = product.UnitPrice,
                StockQuantity = product.StockQuantity,
                IsActive = product.IsActive,
                CreatedAt = product.CreatedAt
            };

            return (true, "Product updated successfully.", responseDto);
        }

        public async Task<(bool Success, string Message)> DeleteProductAsync(int id, string username)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return (false, "Product not found.");

            product.IsDeleted = true;
            product.UpdatedAt = DateTime.UtcNow;
            product.UpdatedBy = username;

            await _productRepository.SaveChangesAsync();

            return (true, "Product deleted successfully.");
        }
    }
}
