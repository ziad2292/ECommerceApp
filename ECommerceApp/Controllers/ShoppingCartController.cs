using Application.DTOs._Common;
using Application.DTOs.ShoppingCart;
using Application.Intefraces.IServices;
using ECommerceApp.Controllers._Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace ECommerceApp.Controllers
{
    [AllowAnonymous]
    public class ShoppingCartController : CustomControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpPost("add-item")]
        public async Task<IActionResult> AddToCart(SetShoppingCartItemDto itemDto)
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

            var response = await _shoppingCartService.AddShoppingCartItemAsync(itemDto);

            return response.Message switch
            {
                "Item added successfully" => Ok(response),
                _ => BadRequest(response)
            };
        }

        [HttpDelete("delete-item")]
        public async Task<IActionResult> DeleteItem(Guid itemId)
        {
            var response = await _shoppingCartService.DeleteShoppingCartItemAsync(itemId);

            return response.Message switch
            {
                "Item deleted Succesfully" => Ok(response),
                _ => BadRequest(response)
            };
        }

        [HttpDelete("empty-cart")]
        public async Task<IActionResult> EmptyCart(Guid cartId)
        {
            var response = await _shoppingCartService.EmptyShoppingCartAsync(cartId);

            return response.Message switch
            {
                "Cart Emptied Successfully" => Ok(response),
                _ => BadRequest(response)
            };
        }

        [HttpGet("get-cart")]
        public async Task<IActionResult> GetCurrentUserCart()
        {
            var response = await _shoppingCartService.GetShoppingCartByUserIdAsync();

            return response.Message switch
            {
                "Shopping Cart returned succesfully" => Ok(response),
                _ => BadRequest(response)
            };
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get-cart/{userId}")]
        public async Task<IActionResult> GetUserCart([FromRoute] Guid userId)
        {
            var response = await _shoppingCartService.GetShoppingCartByUserIdAsync(userId);

            return response.Message switch
            {
                "Shopping Cart returned succesfully" => Ok(response),
                _ => BadRequest(response)
            };
        }

        [HttpPut("update-quantity")]
        public async Task<IActionResult> UpdateItemQuantity(Guid itemId, int quantity)
        {
            var response = await _shoppingCartService.UpdateShoppingCartItemQuantityAsync(itemId, quantity);

            return response.Message switch
            {
                "Item updated successfully" => Ok(response),
                _ => BadRequest(response)
            };
        }
    }
}
