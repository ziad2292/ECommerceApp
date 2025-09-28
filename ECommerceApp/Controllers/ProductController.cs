using Application.DTOs._Common;
using Application.DTOs.Product;
using Application.Intefraces.IServices;
using Domain.Entities;
using ECommerceApp.Controllers._Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Controllers
{
    public class ProductController : CustomControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("add")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> AddProduct(SetProductDto product)
        {
            //Model binding Vaidation
            if (ModelState.IsValid == false)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiResponse<IEnumerable<string>>
                {
                    IsSuccess = false,
                    Message = "Validation errors occurred.",
                    Data = errors
                });
            }

            return Ok(await _productService.CreateProductAsync(product));
        }

        [HttpDelete("delete/{productId}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteProduct(Guid productId)
        {
            var response = await _productService.DeleteProductAsync(productId);

            return response.Message switch
            {
                "Product already doesn't exists" => NotFound(response),
                "Product deleted successfully" => Ok(response),
                _ => BadRequest(response)
            };
        }

        [HttpGet("get/{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductById(Guid productId)
        {
            var response = await _productService.GetProductByIdAsync(productId);

            return response.Message switch
            {
                "Product doesn't exist" => NotFound(response),
                "Product fetched from database" => Ok(response),
                _ => BadRequest(response)
            };
        }

        [HttpPut("update/{productId}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> UpdateProduct(Guid productId, SetProductDto product)
        {
            //Model binding Vaidation
            if (ModelState.IsValid == false)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiResponse<IEnumerable<string>>
                {
                    IsSuccess = false,
                    Message = "Validation errors occurred.",
                    Data = errors
                });
            }

            var response = await _productService.UpdateProductAsync(productId, product);

            return response.Message switch
            {
                "Product doesn't exist" => NotFound(response),
                "Product updated successfully" => Ok(response),
                _ => BadRequest(response)
            };
        }

        [HttpGet("search")] 
        [AllowAnonymous]
        public async Task<IActionResult> SearchProducts([FromQuery] SearchFilterDto filter)
        {
            //Model binding Vaidation
            if (ModelState.IsValid == false)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiResponse<IEnumerable<string>>
                {
                    IsSuccess = false,
                    Message = "Validation errors occurred.",
                    Data = errors
                });
            }

            return Ok(await _productService.SearchProductsAsync(filter));

        }
    }
}
