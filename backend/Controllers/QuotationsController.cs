using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Services;
using System.Security.Claims;

namespace backend.Controllers
{
    [Route("api/quotations")]
    [ApiController]
    [Authorize]
    public class QuotationsController : ControllerBase
    {
        private readonly IQuotationService _quotationService;

        public QuotationsController(IQuotationService quotationService)
        {
            _quotationService = quotationService;
        }

        private string GetEmail() => User.FindFirst(ClaimTypes.Email)?.Value ?? "system";

        [HttpGet]
        public async Task<IActionResult> GetQuotations([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string search = "")
        {
            var result = await _quotationService.GetQuotationsPaginatedAsync(page, pageSize, search);
            return Ok(result);
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
            var result = await _quotationService.CreateQuotationAsync(dto, GetEmail());
            if (!result.Success)
                return NotFound(new { Message = result.Message });

            return CreatedAtAction(nameof(GetQuotations), new { id = result.Quotation!.QuotationId }, result.Quotation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            var result = await _quotationService.UpdateQuotationStatusAsync(id, status, GetEmail());
            if (!result.Success) return NotFound();

            return Ok(result.UpdatedQuotation);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuotation(int id)
        {
            var result = await _quotationService.DeleteQuotationAsync(id, GetEmail());
            if (!result.Success) return NotFound();

            return NoContent();
        }
    }
}
