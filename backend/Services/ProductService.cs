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

        public async Task<Product> CreateProductAsync(Product product, string username)
        {
            product.CreatedAt = DateTime.UtcNow;
            product.CreatedBy = username;

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();

            return product;
        }

        public async Task<(bool Success, string Message, Product? UpdatedProduct)> UpdateProductAsync(int id, Product updateModel, string username)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                return (false, "Product not found.", null);

            product.Name = updateModel.Name;
            product.UnitPrice = updateModel.UnitPrice;
            product.StockQuantity = updateModel.StockQuantity;
            product.IsActive = updateModel.IsActive;

            product.UpdatedAt = DateTime.UtcNow;
            product.UpdatedBy = username;

            await _productRepository.SaveChangesAsync();

            return (true, "Product updated successfully.", product);
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
