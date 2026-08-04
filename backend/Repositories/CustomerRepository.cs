using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _db;

        public CustomerRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<(int TotalItems, IEnumerable<Customer> Items)> GetPaginatedAsync(int page, int pageSize, string search)
        {
            var query = _db.Customers.Where(c => !c.IsDeleted);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.Name.Contains(search) || c.Email.Contains(search) || (c.Company != null && c.Company.Contains(search)));
            }

            var totalItems = await query.CountAsync();
            var items = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (totalItems, items);
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _db.Customers.FirstOrDefaultAsync(c => c.CustomerId == id && !c.IsDeleted);
        }

        public async Task AddAsync(Customer customer)
        {
            await _db.Customers.AddAsync(customer);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
