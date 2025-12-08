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
        private readonly ICategoryService _categoryService;

        public ProductService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService, ICategoryService categoryService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _categoryService = categoryService;
        }

        public async Task<ApiResponse> CreateProductAsync(SetProductDto newProduct)
        {
            if (!await isCategory(newProduct.CategoryId))
                return new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "Category doesn't exist"
                };

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

        public async Task<ApiResponse> DeleteProductAsync(Guid productId)
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
                Id = p.Id,
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

        public async Task<ApiResponse> GetProductByIdAsync(Guid productId)
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
                Id = p.Id,
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

        public async Task<ApiResponse> GetProductsByPriceRange(decimal? minPrice, decimal? maxPrice)
        {
            minPrice ??= 0;
            maxPrice ??= int.MaxValue;

            var products = await _unitOfWork.Products.GetProductsByPriceRangeAsync(minPrice, maxPrice);

            var productDtos = products.Select(p => new GetProductDto
            {
                Id = p.Id,
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

        public async Task<ApiResponse> SearchProductsByNameAsync(string searchTerm)
        {
            var products = await _unitOfWork.Products.GetProductsByNameAsync(searchTerm);

            var productDtos = products.Select(p => new GetProductDto
            {
                Id = p.Id,
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

        public async Task<ApiResponse> UpdateProductAsync(Guid productId, SetProductDto updatedProduct)
        {
            if (!await isCategory(updatedProduct.CategoryId))
                return new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "Category doesn't exist"
                };

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

        public async Task<ApiResponse> SearchProductsAsync(SearchFilterDto filter)
        {
            var allProducts = await _unitOfWork.Products.GetAllAsync();
            var query = allProducts.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(p => p.Name!.ToLower().Contains(filter.Name.ToLower()));
            }

            if (filter.CategoryId != null)
            {
                query = query.Where(p => p.CategoryId == filter.CategoryId);
            }

            if (filter.MinPrice != null && filter.MaxPrice != null)
            {
                query = query.Where(p => p.Price >= filter.MinPrice && p.Price <= filter.MaxPrice);
            }

            var products = query.ToList();

            var productDtos = products.Select(p => new GetProductDto
            {
                Id = p.Id,
                Name = p.Name!,
                Description = p.Description!,
                Price = p.Price,
                CategoryId = p.CategoryId,
                Stock = p.Stock,
                ImageUrl = p.ImageUrl
            });

            return new ApiResponse<IEnumerable<GetProductDto>>
            {
                IsSuccess = true,
                Message = products.Any() ? "Products found" : "No products match the search filters",
                Data = productDtos
            };


        }

        public async Task<bool> isCategory(int CategoryId)
        {
            var result = await _categoryService.GetCategory(CategoryId);
            return result.IsSuccess;
        }
    }
}
