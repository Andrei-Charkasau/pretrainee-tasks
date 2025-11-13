using Microsoft.AspNetCore.Mvc;
using Inno_Shop_Cherkasov.Models;
using Inno_Shop_Cherkasov.Services;
using Inno_Shop_Cherkasov.DtoModels;

namespace Inno_Shop_Cherkasov.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<ActionResult<Product>> GetProductAsync(int id)
        {
            var product = await _productService.GetAsync(id);
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult> CreateProductAsync(ProductDto productDto)
        {
            await _productService.CreateAsync(productDto);
            return Ok("Product was created successfully. [+]");
        }

        [HttpPatch]
        public async Task<ActionResult<string>> UpdateProductAsync(int id, ProductDto productDto)
        {
            await _productService.UpdateAsync(id, productDto);
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult<string>> DeleteProductAsync(int id)
        {
            await _productService.DeleteAsync(id);
            return NoContent();
        }
    }
}
