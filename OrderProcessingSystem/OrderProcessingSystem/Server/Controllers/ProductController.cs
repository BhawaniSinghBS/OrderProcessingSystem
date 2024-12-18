using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderProcessingSystem.Shared.Constants;
using OrderProcessingSystemApplication.ProductService;
using OrderProcessingSystemInfrastructure.DataBase.Entities;
using Serilog;
using System.Reflection;

namespace OrderProcessingSystem.Controllers
{
    //[Authorize] //currently diabled
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet(ApiEndPoints.GetAllProducts)]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                if (products == null || !products.Any())
                    return NotFound();

                return Ok(products);
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName} ---- {ex.Message} ------";
                Log.Error(ex, exLocationAndMessage);
                return StatusCode(500, "An error occurred while fetching products.");
            }
        }

        [HttpGet(ApiEndPoints.GetProductById)]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                    return NotFound();

                return Ok(product);
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName} ---- {ex.Message} ------";
                Log.Error(ex, exLocationAndMessage);
                return StatusCode(500, "An error occurred while fetching the product.");
            }
        }

        [HttpPost(ApiEndPoints.AddProduct)]
        public async Task<IActionResult> AddProduct([FromBody] ProductEntity product)
        {
            try
            {
                if (product == null)
                    return BadRequest("Invalid product data.");

                var result = await _productService.AddProductAsync(product);
                if (result)
                    return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);

                return StatusCode(500, "Error adding product.");
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName} ---- {ex.Message} ------";
                Log.Error(ex, exLocationAndMessage);
                return StatusCode(500, "An error occurred while adding the product.");
            }
        }

        [HttpPut(ApiEndPoints.UpdateProduct)]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductEntity product)
        {
            try
            {
                if (id != product.Id)
                    return BadRequest("Product ID mismatch.");

                var result = await _productService.UpdateProductAsync(product);
                if (result)
                    return NoContent();

                return StatusCode(500, "Error updating product.");
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName} ---- {ex.Message} ------";
                Log.Error(ex, exLocationAndMessage);
                return StatusCode(500, "An error occurred while updating the product.");
            }
        }

        [HttpDelete(ApiEndPoints.DeleteProduct)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var result = await _productService.DeleteProductAsync(id);
                if (result)
                    return NoContent();

                return StatusCode(500, "Error deleting product.");
            }
            catch (Exception ex)
            {
                string className = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? $"{TextMessages.UnknownClassText}";
                string methodName = MethodBase.GetCurrentMethod()?.Name ?? $"{TextMessages.UnknownMethodText}";
                string exLocationAndMessage = $"{TextMessages.ClassNameText} : {className}  -- {TextMessages.FunctionNameText} : {methodName} ---- {ex.Message} ------";
                Log.Error(ex, exLocationAndMessage);
                return StatusCode(500, "An error occurred while deleting the product.");
            }
        }
    }
}
