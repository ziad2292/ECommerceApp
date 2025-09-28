using Application.Intefraces.IServices;
using ECommerceApp.Controllers._Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApp.Controllers
{
    [Authorize(Roles ="Admin")]
    public class CategoryController : CustomControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(string name)
        {
            var response = await _categoryService.AddCategory(name);

            return response.Message switch
            {
                "Category created successfully" => Ok(response),
                _ => BadRequest(response),
            };
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var response = await _categoryService.DeleteCategory(id);

            return response.Message switch
            {
                "Category deleted successfully" => Ok(response),
                _ => BadRequest(response),
            };
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCategories()
        {
            var response = await _categoryService.GetAllCategories();

            return Ok(response);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategory(int id)
        {
            var response = await _categoryService.GetCategory(id);

            return response.Message switch
            {
                "Category returned successfully" => Ok(response),
                _ => BadRequest(response),
            };
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory(int id, string name)
        {
            var response = await _categoryService.UpdateCategory(id, name);

            return response.Message switch
            {
                "Category updated successfully" => Ok(response),
                _ => BadRequest(response),
            };
        }
    }
}
