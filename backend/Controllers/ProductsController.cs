using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using backend.Data;
using backend.Models;
using System.Security.Claims;

namespace backend.Controllers
{
    [Route("api/products")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ProductsController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string search = "")
        {
            var query = _db.Products.Where(p => !p.IsDeleted);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Name.Contains(search));
            }

            var totalItems = await query.CountAsync();
            var items = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                Data = items
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var username = User.FindFirstValue(ClaimTypes.Email) ?? "Unknown";

            product.CreatedAt = DateTime.UtcNow;
            product.CreatedBy = username;

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProducts), new { id = product.ProductId }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product updateModel)
        {
            var product = await _db.Products.FirstOrDefaultAsync(p => p.ProductId == id && !p.IsDeleted);
            if (product == null)
                return NotFound(new { Message = "Product not found." });

            product.Name = updateModel.Name;
            product.UnitPrice = updateModel.UnitPrice;
            product.StockQuantity = updateModel.StockQuantity;
            product.IsActive = updateModel.IsActive;

            var username = User.FindFirstValue(ClaimTypes.Email) ?? "Unknown";
            product.UpdatedAt = DateTime.UtcNow;
            product.UpdatedBy = username;

            await _db.SaveChangesAsync();

            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _db.Products.FirstOrDefaultAsync(p => p.ProductId == id && !p.IsDeleted);
            if (product == null)
                return NotFound(new { Message = "Product not found." });

            var username = User.FindFirstValue(ClaimTypes.Email) ?? "Unknown";
            product.IsDeleted = true;
            product.UpdatedAt = DateTime.UtcNow;
            product.UpdatedBy = username;

            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
