using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using backend.Data;
using backend.Models;
using System.Security.Claims;

namespace backend.Controllers
{
    [Route("api/customers")]
    [ApiController]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CustomersController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string search = "")
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
        public async Task<IActionResult> CreateCustomer([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var username = User.FindFirstValue(ClaimTypes.Email) ?? "Unknown";

            customer.CreatedAt = DateTime.UtcNow;
            customer.CreatedBy = username;

            _db.Customers.Add(customer);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomers), new { id = customer.CustomerId }, customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] Customer updateModel)
        {
            var customer = await _db.Customers.FirstOrDefaultAsync(c => c.CustomerId == id && !c.IsDeleted);
            if (customer == null)
                return NotFound(new { Message = "Customer not found." });

            customer.Name = updateModel.Name;
            customer.Email = updateModel.Email;
            customer.Address = updateModel.Address;
            customer.Company = updateModel.Company;
            customer.IsActive = updateModel.IsActive;

            var username = User.FindFirstValue(ClaimTypes.Email) ?? "Unknown";
            customer.UpdatedAt = DateTime.UtcNow;
            customer.UpdatedBy = username;

            await _db.SaveChangesAsync();

            return Ok(customer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _db.Customers.FirstOrDefaultAsync(c => c.CustomerId == id && !c.IsDeleted);
            if (customer == null)
                return NotFound(new { Message = "Customer not found." });

            var username = User.FindFirstValue(ClaimTypes.Email) ?? "Unknown";
            customer.IsDeleted = true;
            customer.UpdatedAt = DateTime.UtcNow;
            customer.UpdatedBy = username;

            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
