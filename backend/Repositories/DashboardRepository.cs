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

        public async Task<int> GetTotalQuotationsAsync()
        {
            return await _db.Quotations.CountAsync(q => !q.IsDeleted);
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _db.Quotations
                .Where(q => !q.IsDeleted && q.Status != "Draft" && q.Status != "Rejected")
                .SumAsync(q => q.TotalAmount);
        }
    }
}
