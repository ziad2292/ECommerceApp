using Application.DTOs._Common;
using Application.DTOs.Account;
using Application.DTOs.ShoppingCart;
using Application.DTOs.ShoppingCartDTOs;
using Application.Intefraces._Common;
using Application.Intefraces.IServices;
using Domain.Entities;

namespace Application.Services
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

        public async Task<ApiResponse> AddShoppingCartItemAsync(SetShoppingCartItemDto item)
        {
            var cart = await GetCurrentUserCartAsync();
            if (cart == null)
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Shopping Cart doesn't exist"
                };

            var validationResponse = await ValidateDto(cart.Id, item.ProductId, item.Quantity);
            if (!validationResponse.IsValid)
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = validationResponse.Message!
                };

            ShoppingCartItem newItem = new ShoppingCartItem
            {
                ProductId = item.ProductId,
                ShoppingCartId = cart.Id,
                Quantity = item.Quantity,
            };

            await _unitOfWork.ShoppingCartItems.AddAsync(newItem);
            await _unitOfWork.CommitAsync();

            return new ApiResponse
            {
                IsSuccess = true,
                Message = "Item added successfully"
            };
        }

        public async Task<ApiResponse> CreateShoppingCartAsync(Guid userId)
        {
            var userExistResponse = await _accountService.GetById(userId);
            if (!userExistResponse.IsSuccess)
                return userExistResponse;

            var existingCart = await _unitOfWork.ShoppingCarts.GetByUserIdAsync(userId);
            if (existingCart != null)
                return new ApiResponse
                {
                    IsSuccess = true,
                    Message = "Cart already exists"
                };

            ShoppingCart shoppingCart = new ShoppingCart
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "AUTO"
            };

            await _unitOfWork.ShoppingCarts.AddAsync(shoppingCart);
            await _unitOfWork.CommitAsync();

            return new ApiResponse
            {
                IsSuccess = true,
                Message = "Cart created successfully"
            };
        }

        public async Task<ApiResponse> DeleteShoppingCartItemAsync(Guid itemId)
        {
            var cart = await GetCurrentUserCartAsync();
            if (cart == null)
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Shopping Cart doesn't exist"
                };

            var shoppingCartItem = await _unitOfWork.ShoppingCartItems.GetByIdAsync(itemId);
            if (shoppingCartItem == null || shoppingCartItem.ShoppingCartId != cart.Id)
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Shopping Item doesn't exist"
                };

            await _unitOfWork.ShoppingCartItems.DeleteAsync(shoppingCartItem);
            await _unitOfWork.CommitAsync();

            return new ApiResponse
            {
                IsSuccess = true,
                Message = "Item deleted Succesfully"
            };
        }

        public async Task<ApiResponse> EmptyShoppingCartAsync()
        {
            var cart = await GetCurrentUserCartAsync();
            if (cart == null)
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Shopping Cart doesn't exist"
                };

            await _unitOfWork.ShoppingCartItems.EmptyShoppingCartAsync(cart.Id);
            await _unitOfWork.CommitAsync();

            return new ApiResponse
            {
                IsSuccess = true,
                Message = "Cart Emptied Successfully"
            };
        }

        public async Task<ApiResponse> GetShoppingCartByUserIdAsync(Guid? userId = null)
        {
            Guid id;
            if (userId == null)
            {
                if (!Guid.TryParse(_currentUserService.UserId, out id))
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        Message = "User is not authenticated"
                    };
            }
            else
            {
                var userExistResponse = await _accountService.GetById((Guid)userId);
                if (!userExistResponse.IsSuccess)
                    return userExistResponse;

                ApiResponse<UserDto> responseDto = (ApiResponse<UserDto>)userExistResponse;
                id = responseDto.Data!.Id;
            }

            ShoppingCart? cart = await _unitOfWork.ShoppingCarts.GetByUserIdAsync(id);
            if (cart == null)
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Shopping Cart doesn't exist"
                };

            ICollection<GetShoppingCartItemDto> itemsDto = new HashSet<GetShoppingCartItemDto>();
            foreach (ShoppingCartItem item in cart.Items)
            {
                GetShoppingCartItemDto newItem = new GetShoppingCartItemDto
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ShoppingCartId = item.ShoppingCartId,
                    Quantity = item.Quantity
                };

                itemsDto.Add(newItem);
            }

            ShoppingCartDto cartDto = new ShoppingCartDto
            {
                Id = cart.Id,
                Items = itemsDto,
                UserId = cart.UserId
            };

            return new ApiResponse<ShoppingCartDto>
            {
                IsSuccess = true,
                Message = "Shopping Cart returned succesfully",
                Data = cartDto
            };
        }

        public async Task<ApiResponse> UpdateShoppingCartItemQuantityAsync(Guid itemId, int quantity)
        {
            var cart = await GetCurrentUserCartAsync();
            if (cart == null)
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Shopping Cart doesn't exist"
                };

            var shoppingCartItem = await _unitOfWork.ShoppingCartItems.GetByIdAsync(itemId);
            if (shoppingCartItem == null || shoppingCartItem.ShoppingCartId != cart.Id)
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Shopping Cart Item doesn't exist"
                };

            var validationResponse = await ValidateDto(productId: shoppingCartItem.ProductId, quantity: quantity);
            if (!validationResponse.IsValid)
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = validationResponse.Message!
                };

            shoppingCartItem.Quantity = quantity;

            await _unitOfWork.ShoppingCartItems.UpdateAsync(shoppingCartItem);
            await _unitOfWork.CommitAsync();

            return new ApiResponse
            {
                IsSuccess = true,
                Message = "Item updated successfully"
            };
        }

        public async Task<ValidationResultDto> ValidateDto(Guid? cartId = null, Guid? productId = null, int? quantity = null)
        {
            var result = new ValidationResultDto { IsValid = false };

            ShoppingCart? shoppingCart = null;
            Product? product = null;

            if (cartId != null)
            {
                shoppingCart = await _unitOfWork.ShoppingCarts.GetByIdAsync(cartId);
                if (shoppingCart == null)
                    return new ValidationResultDto { Message = "Shopping Cart doesn't exist" };
            }

            if (productId != null)
            {
                product = await _unitOfWork.Products.GetByIdAsync(productId);
                if (product == null)
                    return new ValidationResultDto { Message = "Product doesn't exist" };

                if (quantity != null && (quantity > product.Stock || quantity < 1))
                    return new ValidationResultDto { Message = "Product stock is insufficient" };
            }

            if (cartId != null && productId != null)
            {
                if (await _unitOfWork.ShoppingCartItems.ShoppingItemExistsAsync((Guid)cartId, (Guid)productId))
                    return new ValidationResultDto { Message = "Item already added" };
            }

            result.IsValid = true;
            result.ShoppingCart = shoppingCart;
            result.Product = product;
            return result;
        }

        private async Task<ShoppingCart?> GetCurrentUserCartAsync()
        {
            if (!Guid.TryParse(_currentUserService.UserId, out var userId))
                return null;

            return await _unitOfWork.ShoppingCarts.GetByUserIdAsync(userId);
        }
    }
}
