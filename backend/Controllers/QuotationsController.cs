using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Services;
using backend.DTOs;
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
        private string GetRole() => User.FindFirst(ClaimTypes.Role)?.Value ?? "";

        [HttpGet]
        public async Task<IActionResult> GetQuotations([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string search = "")
        {
            var result = await _quotationService.GetQuotationsPaginatedAsync(page, pageSize, search, GetEmail(), GetRole());
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "SalesRep,Customer")]
        public async Task<IActionResult> CreateQuotation([FromBody] QuotationCreateDto dto)
        {
            var result = await _quotationService.CreateQuotationAsync(dto, GetEmail(), GetRole());
            if (!result.Success)
                return BadRequest(new { Message = result.Message });

            return CreatedAtAction(nameof(GetQuotations), new { id = result.Quotation!.QuotationId }, result.Quotation);
        }

        [HttpPut("{id}/finalize")]
        [Authorize(Roles = "SalesRep")]
        public async Task<IActionResult> FinalizeQuotation(int id, [FromBody] QuotationFinalizeDto dto)
        {
            var result = await _quotationService.FinalizeQuotationAsync(id, dto, GetEmail());
            if (!result.Success)
                return BadRequest(new { Message = result.Message });

            return Ok(result.UpdatedQuotation);
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            var result = await _quotationService.UpdateQuotationStatusAsync(id, status, GetEmail());
            if (!result.Success) return NotFound();

            return Ok(result.UpdatedQuotation);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteQuotation(int id)
        {
            var result = await _quotationService.DeleteQuotationAsync(id, GetEmail());
            if (!result.Success) return NotFound();

            return NoContent();
        }
    }
}
