using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.Models;
using backend.Services;
using System.Security.Claims;

namespace backend.Controllers
{
    [Route("api/customers")]
    [ApiController]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomers([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string search = "")
        {
            var result = await _customerService.GetCustomersPaginatedAsync(page, pageSize, search);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var username = User.FindFirstValue(ClaimTypes.Email) ?? "Unknown";

            var createdCustomer = await _customerService.CreateCustomerAsync(customer, username);

            return CreatedAtAction(nameof(GetCustomers), new { id = createdCustomer.CustomerId }, createdCustomer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] Customer updateModel)
        {
            var username = User.FindFirstValue(ClaimTypes.Email) ?? "Unknown";
            
            var result = await _customerService.UpdateCustomerAsync(id, updateModel, username);
            if (!result.Success)
                return NotFound(new { Message = result.Message });

            return Ok(result.UpdatedCustomer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var username = User.FindFirstValue(ClaimTypes.Email) ?? "Unknown";

            var result = await _customerService.DeleteCustomerAsync(id, username);
            if (!result.Success)
                return NotFound(new { Message = result.Message });

            return NoContent();
        }
    }
}
