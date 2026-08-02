using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using System.Security.Claims;

namespace backend.Controllers
{
    [Route("api/quotations")]
    [ApiController]
    [Authorize]
    public class QuotationsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public QuotationsController(ApplicationDbContext db)
        {
            _db = db;
        }

        private string GetEmail() => User.FindFirst(ClaimTypes.Email)?.Value ?? "system";

        [HttpGet]
        public async Task<IActionResult> GetQuotations([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string search = "")
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

            return Ok(new { totalItems, page, pageSize, totalPages = (int)Math.Ceiling((double)totalItems / pageSize), data });
        }

        public class QuotationCreateDto
        {
            public int CustomerId { get; set; }
            public decimal TaxRate { get; set; }
            public DateTime? ValidUntil { get; set; }
            public List<QuotationItemDto> Items { get; set; } = new();
        }

        public class QuotationItemDto
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
            public decimal DiscountPercent { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuotation([FromBody] QuotationCreateDto dto)
        {
            if (!await _db.Customers.AnyAsync(c => c.CustomerId == dto.CustomerId && !c.IsDeleted))
                return NotFound("Customer not found.");

            var quotation = new Quotation
            {
                CustomerId = dto.CustomerId,
                TaxRate = dto.TaxRate,
                ValidUntil = dto.ValidUntil,
                Status = "Draft",
                QuotationNumber = $"QT-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0,4)}",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = GetEmail()
            };

            decimal subTotal = 0;
            decimal totalDiscount = 0;

            foreach (var item in dto.Items)
            {
                var product = await _db.Products.FindAsync(item.ProductId);
                if (product == null) continue;

                decimal itemSubtotal = product.UnitPrice * item.Quantity;
                decimal itemDiscountAmount = itemSubtotal * (item.DiscountPercent / 100m);
                
                subTotal += itemSubtotal;
                totalDiscount += itemDiscountAmount;

                quotation.QuotationItems.Add(new QuotationItem
                {
                    ProductId = product.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.UnitPrice,
                    DiscountPercent = item.DiscountPercent,
                    DiscountAmount = itemDiscountAmount,
                    TaxRate = dto.TaxRate,
                    LineTotal = itemSubtotal - itemDiscountAmount,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = GetEmail()
                });
            }

            quotation.SubTotal = subTotal;
            quotation.DiscountAmount = totalDiscount;
            decimal amountAfterDiscount = subTotal - totalDiscount;
            quotation.TaxAmount = amountAfterDiscount * (dto.TaxRate / 100m);
            quotation.TotalAmount = amountAfterDiscount + quotation.TaxAmount;

            _db.Quotations.Add(quotation);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetQuotations), new { id = quotation.QuotationId }, quotation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            var quotation = await _db.Quotations.FirstOrDefaultAsync(q => q.QuotationId == id && !q.IsDeleted);
            if (quotation == null) return NotFound();

            quotation.Status = status;
            quotation.UpdatedAt = DateTime.UtcNow;
            quotation.UpdatedBy = GetEmail();
            await _db.SaveChangesAsync();

            return Ok(quotation);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuotation(int id)
        {
            var quotation = await _db.Quotations.FirstOrDefaultAsync(q => q.QuotationId == id && !q.IsDeleted);
            if (quotation == null) return NotFound();

            quotation.IsDeleted = true;
            quotation.UpdatedAt = DateTime.UtcNow;
            quotation.UpdatedBy = GetEmail();
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
