using backend.Models;

namespace backend.Repositories
{
    public interface IQuotationRepository
    {
        Task<(int TotalItems, IEnumerable<object> Items)> GetPaginatedAsync(int page, int pageSize, string search, string email = "", string role = "");
        Task<Quotation?> GetByIdAsync(int id);
        Task<Customer?> GetCustomerByEmailAsync(string email);
        Task<bool> CustomerExistsAsync(int customerId);
        Task<Product?> GetProductByIdAsync(int productId);
        Task AddAsync(Quotation quotation);
        Task SaveChangesAsync();
    }
}
