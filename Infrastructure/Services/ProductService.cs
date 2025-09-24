using Application.DTOs._Common;
using Application.DTOs.Product;
using Application.Intefraces._Common;
using Application.Intefraces.IServices;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public ProductService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ApiResponse> CreateProductAsync(NewProductDto newProduct)
        {
            Product product = new Product()
            {
                Name = newProduct.Name,
                Description = newProduct.Description,
                Price = newProduct.Price,
                ImageUrl = newProduct.ImageUrl,
                CategoryId = newProduct.CategoryId,
                Stock = newProduct.Stock,
                CreatedAt = DateTime.Now,
                CreatedBy = _currentUserService.UserId

            };
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.CommitAsync();

            return new ApiResponse()
            {
                IsSuccess = true,
                Message = "Product Created Successfully"
            };
        }

        public async Task<ApiResponse> DeleteProductAsync(int productId)
        {
            Product? product = await _unitOfWork.Products.GetByIdAsync(productId);
            if (product == null)
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Product already doesn't exists"
                };
            await _unitOfWork.Products.DeleteAsync(product);
            await _unitOfWork.CommitAsync();

            return new ApiResponse()
            {
                IsSuccess = true,
                Message = "Product deleted successfully"
            };
        }

        public async Task<ApiResponse> GetAllProductsAsync()
        {
            var products = await _unitOfWork.Products.GetAllAsync();

            var productDtos = products.Select(p => new GetProductDto
            {
                Name = p.Name!,
                Description = p.Description!,
                ImageUrl = p.ImageUrl,
                Price = p.Price,
                Stock = p.Stock,
                CategoryId = p.CategoryId
            });

            return new ApiResponse<IEnumerable<GetProductDto>>()
            {
                IsSuccess = true,
                Message = "Fetched all products from database",
                Data = productDtos
            };
        }

        public async Task<ApiResponse> GetProductByIdAsync(int productId)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(productId);

            if (product == null)
                return new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "Product doesn't exist"
                };

            return new ApiResponse<Product>()
            {
                IsSuccess = true,
                Message = "Product fetched from database",
                Data = product
            };
        }

        public async Task<ApiResponse> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _unitOfWork.Products.GetProductsByCategoryAsync(categoryId);

            var productDtos = products.Select(p => new GetProductDto
            {
                Name = p.Name!,
                Description = p.Description!,
                ImageUrl = p.ImageUrl,
                Price = p.Price,
                Stock = p.Stock,
                CategoryId = p.CategoryId
            });

            return new ApiResponse<IEnumerable<GetProductDto>>()
            {
                IsSuccess = true,
                Message = "Fetched all products from this category",
                Data = productDtos
            };
        }

        public async Task<ApiResponse> GetProductsByPriceRange(decimal minPrice, decimal maxPrice)
        {
            var products = await _unitOfWork.Products.GetProductsByPriceRangeAsync(minPrice, maxPrice);

            var productDtos = products.Select(p => new GetProductDto
            {
                Name = p.Name!,
                Description = p.Description!,
                ImageUrl = p.ImageUrl,
                Price = p.Price,
                Stock = p.Stock,
                CategoryId = p.CategoryId
            });

            return new ApiResponse<IEnumerable<GetProductDto>>()
            {
                IsSuccess = true,
                Message = "Fetched all products in this price range",
                Data = productDtos
            };
        }

        public async Task<ApiResponse> SearchProductsAsync(string searchTerm)
        {
            var products = await _unitOfWork.Products.GetProductsByNameAsync(searchTerm);

            var productDtos = products.Select(p => new GetProductDto
            {
                Name = p.Name!,
                Description = p.Description!,
                ImageUrl = p.ImageUrl,
                Price = p.Price,
                Stock = p.Stock,
                CategoryId = p.CategoryId
            });

            return new ApiResponse<IEnumerable<GetProductDto>>()
            {
                IsSuccess = true,
                Message = "Fetched all products matching your search",
                Data = productDtos
            };

        }

        public async Task<ApiResponse> UpdateProductAsync(int productId, NewProductDto updatedProduct)
        {
            Product? oldProduct = await _unitOfWork.Products.GetByIdAsync(productId);
            if (oldProduct == null)
                return new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "Product doesn't exist"
                };

            oldProduct.Name = updatedProduct.Name;
            oldProduct.Description = updatedProduct.Description;
            oldProduct.ImageUrl = updatedProduct.ImageUrl;
            oldProduct.Price = updatedProduct.Price;
            oldProduct.Stock = updatedProduct.Stock;
            oldProduct.CategoryId = updatedProduct.CategoryId;

            await _unitOfWork.Products.UpdateAsync(oldProduct);
            await _unitOfWork.CommitAsync();

            return new ApiResponse()
            {
                IsSuccess = true,
                Message = "Product updated successfully"
            };
        }
    }
}
