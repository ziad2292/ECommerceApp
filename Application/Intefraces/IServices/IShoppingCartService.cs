using Application.DTOs._Common;
using Application.DTOs.ShoppingCart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Intefraces.IServices
{
    public interface IShoppingCartService
    {
        Task<ApiResponse> CreateShoppingCartAsync(Guid userId);
        Task<ApiResponse> AddShoppingCartItemAsync(SetShoppingCartItemDto item);
        Task<ApiResponse> UpdateShoppingCartItemQuantityAsync(Guid itemId, int quantity);
        Task<ApiResponse> DeleteShoppingCartItemAsync(Guid itemId);
        Task<ApiResponse> GetShoppingCartByUserIdAsync(Guid? userId = null);
        Task<ApiResponse> EmptyShoppingCartAsync();
    }
}
