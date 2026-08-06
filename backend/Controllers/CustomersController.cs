using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.Services;
using backend.DTOs;
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

        private string GetEmail() => User.FindFirst(ClaimTypes.Email)?.Value ?? "system";

        [HttpGet("me")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetMyProfile()
        {
            var result = await _customerService.GetMyProfileAsync(GetEmail());
            if (!result.Success)
                return NotFound(new { Message = result.Message });

            return Ok(result.Profile);
        }

        [HttpPut("me")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] CustomerProfileUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _customerService.UpdateMyProfileAsync(GetEmail(), dto);
            if (!result.Success)
                return NotFound(new { Message = result.Message });

            return Ok(result.UpdatedCustomer);
        }

        [HttpGet]
        [Authorize(Roles = "Manager,SalesRep")]
        public async Task<IActionResult> GetCustomers([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string search = "")
        {
            var result = await _customerService.GetCustomersPaginatedAsync(page, pageSize, search);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Manager,SalesRep")]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdCustomer = await _customerService.CreateCustomerAsync(dto, GetEmail());

            return CreatedAtAction(nameof(GetCustomers), new { id = createdCustomer.CustomerId }, createdCustomer);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager,SalesRep")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerUpdateDto dto)
        {
            var result = await _customerService.UpdateCustomerAsync(id, dto, GetEmail());
            if (!result.Success)
                return NotFound(new { Message = result.Message });

            return Ok(result.UpdatedCustomer);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager,SalesRep")]
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
