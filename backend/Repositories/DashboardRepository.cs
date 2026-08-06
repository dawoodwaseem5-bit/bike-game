using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _db;

        public DashboardRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<int> GetTotalCustomersAsync()
        {
            return await _db.Customers.CountAsync(c => !c.IsDeleted);
        }

        public async Task<int> GetTotalProductsAsync()
        {
            return await _db.Products.CountAsync(p => !p.IsDeleted);
        }

        public async Task<int> GetTotalQuotationsAsync(string email = "", string role = "")
        {
            var query = _db.Quotations.Where(q => !q.IsDeleted);
            if (role == "Customer" && !string.IsNullOrEmpty(email))
            {
                query = query.Where(q => q.Customer != null && q.Customer.Email == email);
            }
            return await query.CountAsync();
        }

        public async Task<decimal> GetTotalRevenueAsync(string email = "", string role = "")
        {
            var query = _db.Quotations
                .Where(q => !q.IsDeleted && q.Status == "Approved");
            
            if (role == "Customer" && !string.IsNullOrEmpty(email))
            {
                query = query.Where(q => q.Customer != null && q.Customer.Email == email);
            }
            
            return await query.SumAsync(q => (decimal?)q.TotalAmount) ?? 0;
        }

        public async Task<Dictionary<string, int>> GetQuotationsByStatusAsync(string email = "", string role = "")
        {
            var query = _db.Quotations.Where(q => !q.IsDeleted);

            if (role == "Customer" && !string.IsNullOrEmpty(email))
            {
                query = query.Where(q => q.Customer != null && q.Customer.Email == email);
            }

            var grouped = await query
                .GroupBy(q => q.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            var result = new Dictionary<string, int>
            {
                ["Pending"] = 0,
                ["Approved"] = 0,
                ["Rejected"] = 0,
                ["Draft"] = 0
            };

            foreach (var item in grouped)
            {
                var status = string.IsNullOrWhiteSpace(item.Status) ? "Draft" : item.Status;
                if (result.ContainsKey(status))
                    result[status] = item.Count;
                else
                    result[status] = item.Count;
            }

            return result;
        }
    }
}
