using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InnoShop.Core.Services.Services;
using InnoShop.Core.Models;
using InnoShop.Core.DtoModels;
using System.Security.Claims;

namespace InnoShop.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProductsAsync()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<Product>> GetProductAsync(int productId)
        {
            var product = await _productService.GetAsync(productId);
            return Ok(product);
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult> CreateProductAsync(ProductDto productDto)
        {
            await _productService.CreateAsync(productDto);
            return Ok("Product was created successfully. [+]");
        }

        [HttpPatch]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<string>> UpdateProductAsync(int id, ProductDto productDto)
        {
            await _productService.UpdateAsync(id, productDto);
            return NoContent();
        }

        [HttpDelete]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<string>> DeleteProductAsync(int id)
        {
            await _productService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("search")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> SearchProducts([FromQuery] ProductFilterDto filter)
        {
            var result = await _productService.SearchProductsAsync(filter);
            return Ok(result);
        }
    }
}
