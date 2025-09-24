using Application.DTOs._Common;
using Application.DTOs.Product;
using Application.Intefraces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ProductService : IProductService
    {
        public Task<ApiResponse> CreateProductAsync(NewProductDto newProduct)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> DeleteProductAsync(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> GetAllProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> GetProductByIdAsync(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> GetProductsByCategoryAsync(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> SearchProductsAsync(string searchTerm)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse> UpdateProductAsync(int productId, NewProductDto updatedProduct)
        {
            throw new NotImplementedException();
        }
    }
}
