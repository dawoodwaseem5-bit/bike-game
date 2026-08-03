using backend.Models;

namespace backend.Repositories
{
    public interface ICustomerRepository
    {
        Task<(int TotalItems, IEnumerable<Customer> Items)> GetPaginatedAsync(int page, int pageSize, string search);
        Task<Customer?> GetByIdAsync(int id);
        Task AddAsync(Customer customer);
        Task SaveChangesAsync();
    }
}
