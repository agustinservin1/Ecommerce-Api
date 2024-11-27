using Application.Interfaces;
using Application.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet("ById/{id}")]

        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetProductById(id);
            return Ok(product);
        }
        [HttpGet("AllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }
        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct([FromBody]CreateProductRequest createProductRequest)
        {
            await _productService.CreateProduct(createProductRequest);
            return NoContent();
        }
        [HttpPost("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody]UpdateProductRequest updateProductRequest)
        {
            await _productService.UpdateProduct(id, updateProductRequest);
            return NoContent();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct (int id)
        {
            await _productService.DeleteProduct(id);
            return NoContent();
        }
    }
}
