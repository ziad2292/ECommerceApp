using Application.DTOs._Common;
using Application.DTOs.Auth;
using Application.IServices;
using Domain.Enums;
using Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;

        public AuthService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }   

        public Task<ApiResponse<bool>> LoginAsync()
        {
            throw new NotImplementedException();
        }

        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse> RegisterAsync(RegisterRequestDto registerDto)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = "The Email is already registered"
                };

            User user = new User()
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                BirthDate = registerDto.BirthDate,
                Gender = (Gender)registerDto.gender

            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            //Registration Failed
            if (!result.Succeeded)
            {
                return new ApiResponse<IEnumerable<IdentityError>>
                {
                    IsSuccess = false,
                    Message = "Registration failed",
                    Data = result.Errors
                };
            }

            //TODO: Add role to user

            return new ApiResponse
            {
                IsSuccess = true,
                Message = "Registration successful"
            };
        } 
    }
}
