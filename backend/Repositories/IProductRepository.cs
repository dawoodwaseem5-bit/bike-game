using backend.Models;

namespace backend.Repositories
{
    public interface IProductRepository
    {
        Task<(int TotalItems, IEnumerable<Product> Items)> GetPaginatedAsync(int page, int pageSize, string search);
        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task SaveChangesAsync();
    }
}
