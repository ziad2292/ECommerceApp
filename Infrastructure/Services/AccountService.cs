using Application.DTOs._Common;
using Application.DTOs.Account;
using Application.Intefraces.IServices;
using Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager; 
        public AccountService(UserManager<User> userManager) {
            _userManager = userManager;
        }
        public async Task<ApiResponse> DeleteUser(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = "This user already doesn't exists"
                };

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return new ApiResponse<IEnumerable<IdentityError>>
                {
                    IsSuccess = false,
                    Data = result.Errors,
                    Message = "User deletion failed"
                };
            }
            return new ApiResponse
            {
                IsSuccess = true,
                Message = "User deleted successfully"
            };
        }

        public ApiResponse GetAllUsers()
        {
            var users = _userManager.Users.Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.UserName!,
                Email = u.Email!,
                PhoneNumber = u.PhoneNumber!,
                BirthDate = u.BirthDate,
                Gender = u.Gender
            })
            .ToList();

            return new ApiResponse<List<UserDto>>
            {
                IsSuccess = true,
                Message = "Users returned successfully",
                Data = users
            };
        }

        public async Task<ApiResponse> GetById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = "User doesn't exist"
                };

            var userDto = new UserDto
            {  
                Id = user.Id,
                Name = user.UserName!,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber!,
                BirthDate = user.BirthDate,
                Gender = user.Gender
            };

            return new ApiResponse<UserDto>
            {
                IsSuccess = true,
                Message = "User returned successfully",
                Data = userDto
            };
        }

        public async Task<ApiResponse> UpdatePassword(Guid id, string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = "User doesn't exist"
                };
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

            if (!result.Succeeded)
            {
                return new ApiResponse<IEnumerable<IdentityError>>
                {
                    IsSuccess = false,
                    Message = "Password change failed",
                    Data = result.Errors
                };
            }

            return new ApiResponse { IsSuccess = true, Message = "Password changed successfully" };
        }

        public async Task<ApiResponse> UpdateUser(UserDto userDto)
        {
            var user = await _userManager.FindByIdAsync(userDto.Id.ToString());
            if (user == null)
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = "User doesn't exist"
                };

            var existingUserWithEmail = await _userManager.FindByEmailAsync(userDto.Email);
            if (existingUserWithEmail != null && existingUserWithEmail.Id != user.Id)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Email is already in use by another account"
                };
            }

            user.UserName = userDto.Name;
            user.Email = userDto.Email;
            user.PhoneNumber = userDto.PhoneNumber;
            user.BirthDate = userDto.BirthDate;
            user.Gender = userDto.Gender;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return new ApiResponse<IEnumerable<IdentityError>>
                {
                    IsSuccess = false,
                    Message = "User didn't update",
                    Data = result.Errors
                };

            return new ApiResponse
            {
                IsSuccess = true,
                Message = "User updated successfully",
            };
        }
    }
}
