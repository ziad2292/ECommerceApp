using Application.DTOs._Common;
using Application.DTOs.Account;
using Application.DTOs.ShoppingCart;
using Application.Intefraces._Common;
using Application.Intefraces.IServices;
using Application.Intefraces.Repositories;
using Domain.Entities;
using Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAccountService _accountService;

        public ShoppingCartService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, IAccountService accountService)
        {  
            _unitOfWork = unitOfWork;
            _accountService = accountService;
            _currentUserService = currentUserService;
        }

        public async Task<ApiResponse> AddShoppingCartItem(SetShoppingCartItem item)
        {
            var validationResponse = await ValidateDto(item.ShoppingCartId, item.ProductId, item.Quantity);

            if (!validationResponse.IsSuccess)
                return validationResponse;

            ShoppingCartItem newItem = new ShoppingCartItem()
            {
                ProductId = item.ProductId,
                ShoppingCartId = item.ShoppingCartId,
                Quantity = item.Quantity,
            };

            await _unitOfWork.ShoppingCartItems.AddAsync(newItem);
            await _unitOfWork.CommitAsync();

            return new ApiResponse()
            {
                IsSuccess = true,
                Message = "Item added successfully"
            };
        }

        public async Task<ApiResponse> CreateShoppingCart(Guid userId)
        {
            var userExistResponse = _accountService.GetById(userId).Result;
            if (!userExistResponse.IsSuccess)
                return userExistResponse;

            ShoppingCart shoppingCart = new ShoppingCart()
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "AUTO"
            };

            await _unitOfWork.ShoppingCarts.AddAsync(shoppingCart);
            await _unitOfWork.CommitAsync();

            return new ApiResponse()
            {
                IsSuccess = true,
                Message = "Cart created successfully"
            };


        }

        public async Task<ApiResponse> DeleteShoppingCartItem(Guid itemId)
        {
            var ShoppingCartItem = await _unitOfWork.ShoppingCartItems.GetByIdAsync(itemId);

            if (ShoppingCartItem == null)
                return new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "Shopping Item doesn't exist"
                };

            await _unitOfWork.ShoppingCartItems.DeleteAsync(ShoppingCartItem);
            await _unitOfWork.CommitAsync();

            return new ApiResponse()
            {
                IsSuccess = true,
                Message = "Item deleted Succesfully"
            };
        }

        public async Task<ApiResponse> EmptyShoppingCart(Guid CartId)
        {
            var validationResponse = await ValidateDto(cartId: CartId);
            if (!validationResponse.IsSuccess)
                return validationResponse;

            await _unitOfWork.ShoppingCartItems.EmptyShoppingCartAsync(CartId);
            return new ApiResponse()
            {
                IsSuccess = true,
                Message = "Cart Emptied Successfully"
            };

        }

        public async Task<ApiResponse> GetShoppingCartByUserId(Guid? userId)
        {
            Guid Id;
            if(userId == null)
            {
                Guid.TryParse(_currentUserService.UserId, out Id);
            }
            else
            {
                var userExistResponse = await _accountService.GetById((Guid)userId);
                if (!userExistResponse.IsSuccess)
                    return userExistResponse;
                else
                {
                    ApiResponse<UserDto> ResponseDto= (ApiResponse<UserDto>)userExistResponse;
                    Id = ResponseDto.Data!.Id;
                }
            }

            ShoppingCart? cart = await _unitOfWork.ShoppingCarts.GetByUserIdAsync(Id);

            if (cart == null)
            {
                return new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "Shopping Cart doesn't exist"
                };
            }

            return new ApiResponse<ShoppingCart>()
            {
                IsSuccess = true,
                Message = "Shopping Cart returned succesfully",
                Data = cart
            };
        }

        public async Task<ApiResponse> UpdateShoppingCartItemQuantity(Guid itemId, int quantity)
        {
            var shoppingCartItem = await _unitOfWork.ShoppingCartItems.GetByIdAsync(itemId);

            if (shoppingCartItem == null)
                new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "Shopping Cart Item doesn't exist"
                };

            var validationResponse = await ValidateDto(productId: shoppingCartItem!.ProductId, quantity: quantity);
            if (!validationResponse.IsSuccess)
                return validationResponse;

            shoppingCartItem.Quantity = quantity;

            await _unitOfWork.ShoppingCartItems.UpdateAsync(shoppingCartItem);
            await _unitOfWork.CommitAsync();

            return new ApiResponse()
            {
                IsSuccess = true,
                Message = "Item updated successfully"
            };
        }

        public async Task<ApiResponse> ValidateDto(Guid? cartId = null, Guid? productId = null, int? quantity = null)
        {
            if(cartId != null)
            {
                var shoppingCart = await _unitOfWork.ShoppingCarts.GetByIdAsync(cartId);
                if (shoppingCart == null)
                    return new ApiResponse()
                    {
                        IsSuccess = false,
                        Message = "Shopping Cart doesn't exists"
                    };
            }

            if (productId != null)
            {
                var product = await _unitOfWork.Products.GetByIdAsync(productId);
                if (product == null)
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        Message = "Product doesn't exist"
                    };

                if (quantity != null && quantity > product.Stock)
                    return new ApiResponse()
                    {
                        IsSuccess = false,
                        Message = "Product stock is insufficient"
                    };
            }

            
            if(cartId != null && productId != null)
                if (await _unitOfWork.ShoppingCartItems.ShoppingItemExistsAsync((Guid)cartId, (Guid)productId))
                    return new ApiResponse()
                    {
                        IsSuccess = false,
                        Message = "Item already added"
                    };

            

            return new ApiResponse()
            {
                IsSuccess = true,
                Message = "Dto is validated"
            };
        }
    }
}
