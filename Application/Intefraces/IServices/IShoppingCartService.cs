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
        Task<ApiResponse> CreateShoppingCart(Guid userId);
        Task<ApiResponse> AddShoppingCartItem(SetShoppingCartItem item);
        Task<ApiResponse> UpdateShoppingCartItemQuantity(Guid itemId, int quantity);
        Task<ApiResponse> DeleteShoppingCartItem(Guid itemId);
        Task<ApiResponse> GetShoppingCartByUserId(Guid? userId);
        Task<ApiResponse> EmptyShoppingCart(Guid CartId);
    }
}
