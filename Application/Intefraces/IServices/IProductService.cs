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
        Task<ApiResponse> GetProductByIdAsync(Guid productId);
        Task<ApiResponse> CreateProductAsync(SetProductDto newProduct);
        Task<ApiResponse> UpdateProductAsync(Guid productId, SetProductDto updatedProduct);
        Task<ApiResponse> DeleteProductAsync(Guid productId);
        Task<ApiResponse> GetProductsByCategoryAsync(int categoryId);
        Task<ApiResponse> SearchProductsByNameAsync(string searchTerm);
        Task<ApiResponse> SearchProductsAsync(SearchFilterDto filter);
        Task<ApiResponse> GetProductsByPriceRange(decimal? minPrice, decimal? maxPrice);
    }
}
