using Application.DTOs._Common;
using Application.DTOs.Account;
using Application.Intefraces.IServices;
using Domain.IdentityEntities;
using ECommerceApp.Controllers._Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommerceApp.Controllers
{

    [Authorize]
    public class AccountController : CustomControllerBase
    {
        private readonly IAccountService _accountService;
        
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("get-all")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllUsers()
        {
            var response = _accountService.GetAllUsers();

            return Ok(response);
        }

        [HttpGet("get-current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (Guid.TryParse(userIdString, out var userId))
            {
                var response = await _accountService.GetById(userId);
                return response.Message switch
                {
                    "User doesn't exist" => NotFound(response),
                    "User returned successfully" => Ok(response),
                    _ => BadRequest(response)
                };

            }
            else
            {
                return BadRequest(new ApiResponse
                {
                    IsSuccess = true,
                    Message = "Error while getting user id claim"
                });
            }
        }

        [HttpGet("get-user")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var response = await _accountService.GetById(userId);
            return response.Message switch
            {
                "User doesn't exist" => NotFound(response),
                "User returned successfully" => Ok(response),
                _ => BadRequest(response)
            };
        }

        [HttpPut("update-user")]
        public async Task<IActionResult> UpdateUser(UserDto userdto)
        {
            //Model binding Vaidation
            if (ModelState.IsValid == false)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiResponse<IEnumerable<string>>
                {
                    IsSuccess = false,
                    Message = "Validation errors occurred.",
                    Data = errors
                });
            }

            if (userdto.Id.ToString() != User.FindFirstValue(ClaimTypes.NameIdentifier) && !User.IsInRole("Admin"))
                return StatusCode(StatusCodes.Status403Forbidden, (new ApiResponse
                {
                    IsSuccess = false,
                    Message = "User can't update another user's information"
                }));

            var response = await _accountService.UpdateUser(userdto);

            return response.Message switch
            {
                "User doesn't exist" => NotFound(response),
                "Email is already in use by another account" => BadRequest(response),
                "User didn't update" => BadRequest(response),
                "User updated successfully" => Ok(response),
                _ => BadRequest(response)
            };
        }

        [HttpPut("update-password")]
        public async Task<IActionResult> UpdateUserPassword(Guid id, string oldPassword, string newPassword)
        {
            if (id.ToString() != User.FindFirstValue(ClaimTypes.NameIdentifier) && !User.IsInRole("Admin"))
                return StatusCode(StatusCodes.Status403Forbidden, (new ApiResponse
                {
                    IsSuccess = false,
                    Message = "User can't update another user's password"
                }));

            var response = await _accountService.UpdatePassword(id, oldPassword, newPassword);

            return response.Message switch
            {
                "User doesn't exist" => NotFound(response),
                "Password change failed" => BadRequest(response),
                "Password changed successfully" => Ok(response),
                _ => BadRequest(response)
            };
        }

        [HttpDelete("delete-user")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            if (id.ToString() != User.FindFirstValue(ClaimTypes.NameIdentifier) && !User.IsInRole("Admin"))
                return StatusCode(StatusCodes.Status403Forbidden, (new ApiResponse
                {
                    IsSuccess = false,
                    Message = "User can't delete other user's account"
                }));

            var response = await _accountService.DeleteUser(id);

            return response.Message switch
            {
                "This user already doesn't exists" => NotFound(response),
                "User deletion failed" => BadRequest(response),
                "User deleted successfully" => Ok(response),
                _ => BadRequest(response)
            };
        }
    }
}
