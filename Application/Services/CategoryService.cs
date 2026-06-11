using Application.DTOs._Common;
using Application.Intefraces._Common;
using Application.Intefraces.IServices;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ApiResponse> AddCategory(string CategoryName)
        {
            if (CategoryName.Length > 50)
                return new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "Category name is too long"
                };

            Category category = new Category() { Name = CategoryName };
            await _unitOfWork.Categories.AddAsync(category);
            try
            {
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                return new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "Category name already exists"
                };

            }

            return new ApiResponse()
            {
                IsSuccess = true,
                Message = "Category created successfully"
            };
        }

        public async Task<ApiResponse> DeleteCategory(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (category == null)
                return new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "Category already doesn't exists"
                };

            await _unitOfWork.Categories.DeleteAsync(category);
            await _unitOfWork.CommitAsync();

            return new ApiResponse()
            {
                IsSuccess = true,
                Message = "Category deleted successfully"
            };
        }

        public async Task<ApiResponse> GetAllCategories()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();

            return new ApiResponse<IEnumerable<Category>>()
            {
                IsSuccess = true,
                Message = "Categories returned successfully",
                Data = categories
            };
        }

        public async Task<ApiResponse> GetCategory(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (category == null)
                return new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "Category doesn't exist"
                };

            return new ApiResponse<Category>()
            {
                IsSuccess = true,
                Message = "Category returned successfully",
                Data = category
            };
        }

        public async Task<ApiResponse> UpdateCategory(int id, string CategoryName)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (category == null)
                return new ApiResponse()
                {
                    IsSuccess = false,
                    Message = "Category doesn't exist"
                };

            category.Name = CategoryName;

            await _unitOfWork.Categories.UpdateAsync(category);
            await _unitOfWork.CommitAsync();

            return new ApiResponse()
            {
                IsSuccess = true,
                Message = "Category updated successfully"
            };
        }
    }
}
