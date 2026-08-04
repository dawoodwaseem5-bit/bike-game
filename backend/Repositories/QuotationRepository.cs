using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class QuotationRepository : IQuotationRepository
    {
        private readonly ApplicationDbContext _db;

        public QuotationRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<(int TotalItems, IEnumerable<object> Items)> GetPaginatedAsync(int page, int pageSize, string search)
        {
            var query = _db.Quotations
                .Include(q => q.Customer)
                .Where(q => !q.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(q => q.QuotationNumber.Contains(search) || (q.Customer != null && q.Customer.Name.Contains(search)));
            }

            var totalItems = await query.CountAsync();
            var data = await query
                .OrderByDescending(q => q.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(q => new {
                    q.QuotationId,
                    q.QuotationNumber,
                    q.Status,
                    q.TotalAmount,
                    q.CreatedAt,
                    CustomerName = q.Customer != null ? q.Customer.Name : "Unknown"
                })
                .ToListAsync();

            return (totalItems, data);
        }

        public async Task<Quotation?> GetByIdAsync(int id)
        {
            return await _db.Quotations.FirstOrDefaultAsync(q => q.QuotationId == id && !q.IsDeleted);
        }

        public async Task<bool> CustomerExistsAsync(int customerId)
        {
            return await _db.Customers.AnyAsync(c => c.CustomerId == customerId && !c.IsDeleted);
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            return await _db.Products.FindAsync(productId);
        }

        public async Task AddAsync(Quotation quotation)
        {
            await _db.Quotations.AddAsync(quotation);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
