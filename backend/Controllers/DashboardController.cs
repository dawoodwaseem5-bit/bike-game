using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;

namespace backend.Controllers
{
    [Route("api/dashboard")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public DashboardController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var totalCustomers = await _db.Customers.CountAsync(c => !c.IsDeleted);
            var totalProducts = await _db.Products.CountAsync(p => !p.IsDeleted);
            var totalQuotations = await _db.Quotations.CountAsync(q => !q.IsDeleted);
            
            var totalRevenue = await _db.Quotations
                .Where(q => !q.IsDeleted && q.Status != "Draft" && q.Status != "Rejected")
                .SumAsync(q => q.TotalAmount);

            return Ok(new
            {
                totalCustomers,
                totalProducts,
                totalQuotations,
                totalRevenue
            });
        }
    }
}
