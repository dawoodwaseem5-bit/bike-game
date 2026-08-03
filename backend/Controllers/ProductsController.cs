using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.Models;
using backend.Services;
using System.Security.Claims;

namespace backend.Controllers
{
    [Route("api/products")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string search = "")
        {
            var result = await _productService.GetProductsPaginatedAsync(page, pageSize, search);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var username = User.FindFirstValue(ClaimTypes.Email) ?? "Unknown";

            var createdProduct = await _productService.CreateProductAsync(product, username);

            return CreatedAtAction(nameof(GetProducts), new { id = createdProduct.ProductId }, createdProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product updateModel)
        {
            var username = User.FindFirstValue(ClaimTypes.Email) ?? "Unknown";

            var result = await _productService.UpdateProductAsync(id, updateModel, username);
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
