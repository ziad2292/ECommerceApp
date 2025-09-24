using Application.DTOs._Common;
using Application.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Intefraces.IServices
{
    public interface IProductService
    {
        Task<ApiResponse> GetAllProductsAsync();
        Task<ApiResponse> GetProductByIdAsync(int productId);
        Task<ApiResponse> CreateProductAsync(NewProductDto newProduct);
        Task<ApiResponse> UpdateProductAsync(int productId, NewProductDto updatedProduct);
        Task<ApiResponse> DeleteProductAsync(int productId);
        Task<ApiResponse> GetProductsByCategoryAsync(int categoryId);
        Task<ApiResponse> SearchProductsAsync(string searchTerm);
        Task<ApiResponse> GetProductsByPriceRange(decimal minPrice, decimal maxPrice);
    }
}
