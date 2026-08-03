using backend.Models;

namespace backend.Services
{
    public interface IProductService
    {
        Task<object> GetProductsPaginatedAsync(int page, int pageSize, string search);
        Task<Product> CreateProductAsync(Product product, string username);
        Task<(bool Success, string Message, Product? UpdatedProduct)> UpdateProductAsync(int id, Product updateModel, string username);
        Task<(bool Success, string Message)> DeleteProductAsync(int id, string username);
    }
}
