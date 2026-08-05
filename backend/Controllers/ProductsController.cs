using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.Models;
using backend.Services;
using backend.DTOs;
using System.Security.Claims;

namespace backend.Controllers
{
    [Route("api/products")]
    [ApiController]
    [Authorize(Roles = "Manager,SalesRep")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        private string GetEmail() => User.FindFirst(ClaimTypes.Email)?.Value ?? "system";

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string search = "")
        {
            var result = await _productService.GetProductsPaginatedAsync(page, pageSize, search);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdProduct = await _productService.CreateProductAsync(dto, GetEmail());

            return CreatedAtAction(nameof(GetProducts), new { id = createdProduct.ProductId }, createdProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDto dto)
        {
            var result = await _productService.UpdateProductAsync(id, dto, GetEmail());
            if (!result.Success)
                return NotFound(new { Message = result.Message });

            return Ok(result.UpdatedProduct);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var username = User.FindFirstValue(ClaimTypes.Email) ?? "Unknown";

            var result = await _productService.DeleteProductAsync(id, username);
            if (!result.Success)
                return NotFound(new { Message = result.Message });

            return NoContent();
        }
    }
}
