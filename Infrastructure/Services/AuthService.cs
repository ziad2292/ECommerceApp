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
        private readonly SignInManager<User> _signInManager;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }   

        public async Task<ApiResponse> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDto.Email);

            if(user == null)
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Email not found"
                };

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequestDto.Password, lockoutOnFailure: false);

            if(result.IsNotAllowed)
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Account is not allowed"
                };

            if(result.IsLockedOut)
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Account is locked out"
                };

            if(!result.Succeeded)
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Invalid credentials"
                };

            //TODO: Check for user role

            //TODO: Generate JWT Token

            //TODO: Initialize AuthResponseDto
            var response = new AuthResponseDto();

            return new ApiResponse<AuthResponseDto>
            {
                IsSuccess = true,
                Message = "Login successful",
                Data = response
            };

        }

        public async Task LogoutAsync()
        {
            //TODO: Revoke refresh token

            await _signInManager.SignOutAsync();
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


            //Sign in
            await _signInManager.SignInAsync(user, isPersistent: false); //isPersistent: persist the authentication cookie in the browser even after closing the browser

            return new ApiResponse
            {
                IsSuccess = true,
                Message = "Registration successful"
            };
        } 
    }
}
