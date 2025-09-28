using Application.DTOs._Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Intefraces.IServices
{
    public interface ICategoryService
    {
        Task<ApiResponse> AddCategory(string Name);
        Task<ApiResponse> DeleteCategory(int id);
        Task<ApiResponse> UpdateCategory(int id, string Name);
        Task<ApiResponse> GetCategory(int id);
        Task<ApiResponse> GetAllCategories();
    }
}
