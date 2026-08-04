using backend.DTOs;

namespace backend.Services
{
    public interface IProductService
    {
        Task<object> GetProductsPaginatedAsync(int page, int pageSize, string search);
        Task<ProductResponseDto> CreateProductAsync(ProductCreateDto dto, string username);
        Task<(bool Success, string Message, ProductResponseDto? UpdatedProduct)> UpdateProductAsync(int id, ProductUpdateDto dto, string username);
        Task<(bool Success, string Message)> DeleteProductAsync(int id, string username);
    }
}
