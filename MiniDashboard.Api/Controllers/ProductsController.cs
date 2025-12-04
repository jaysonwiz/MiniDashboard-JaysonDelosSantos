using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;

namespace DaifukuWebAPI.Controllers
{
    [Route("[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductsService _productsService;
        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Create([FromBody] ProductAddRequest categoryAddRequest)
        {
            try
            {
                var newProduct = await _productsService.AddProduct(categoryAddRequest);
                return Ok(newProduct);
            }
            catch (ArgumentException aex)
            {
                return BadRequest(new { Error = aex.Message });
            }
            catch (Exception ex)
            {
                // Catch-all for unexpected errors
                return StatusCode(500, new { Error = "An unexpected error occurred", Details = ex.Message });
            }
        }
        [HttpPut]
        [Route("[action]/{productId}")]
        public async Task<IActionResult> Update([FromBody] ProductUpdateRequest productUpdateRequest, [FromRoute] Guid? productId)
        {
            try
            {
                if (productId == null)
                {
                    return BadRequest(new { Error = "Product Id route parameter should not be null" });
                }

                var updatedProduct = await _productsService.UpdateProduct(productUpdateRequest);
                return Ok(updatedProduct);
            }
            catch (ArgumentException aex)
            {
                return BadRequest(new { Error = aex.Message });
            }
            catch (Exception ex)
            {
                // Catch-all for unexpected errors
                return StatusCode(500, new { Error = "An unexpected error occurred", Details = ex.Message });
            }
        }
        [HttpGet]
        [Route("{productId}")]
        public async Task<IActionResult> GetProductByID([FromRoute] Guid? productId)
        {
            try
            {
                var products = await _productsService.GetProductByID(productId);
                return Ok(products);
            }
            catch (ArgumentException aex)
            {
                return BadRequest(new { Error = aex.Message });
            }
            catch (Exception ex)
            {
                // Catch-all for unexpected errors
                return StatusCode(500, new { Error = "An unexpected error occurred", Details = ex.Message });
            }
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await _productsService.GetAllProducts();
                return Ok(products);
            }
            catch (ArgumentException aex)
            {
                return BadRequest(new { Error = aex.Message });
            }
            catch (Exception ex)
            {
                // Catch-all for unexpected errors
                return StatusCode(500, new { Error = "An unexpected error occurred", Details = ex.Message });
            }
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            try
            {
                var products = await _productsService.GetProductBySearch(query);
                return Ok(products);
            }
            catch (ArgumentException aex)
            {
                return BadRequest(new { Error = aex.Message });
            }
            catch (Exception ex)
            {
                // Catch-all for unexpected errors
                return StatusCode(500, new { Error = "An unexpected error occurred", Details = ex.Message });
            }
        }
        [HttpDelete]
        [Route("[action]/{productId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid productId)
        {
            try
            {
                var isDeleted = await _productsService.DeleteProduct(productId);
                return Ok(isDeleted);
            }
            catch (ArgumentException aex)
            {
                return BadRequest(new { Error = aex.Message });
            }
            catch (Exception ex)
            {
                // Catch-all for unexpected errors
                return StatusCode(500, new { Error = "An unexpected error occurred", Details = ex.Message });
            }
        }
    }
}
