using Application.DTOs._Common;
using Application.DTOs.Auth;
using Application.Intefraces.IServices;
using Application.Settings;
using Domain.Enums;
using Domain.IdentityEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<Role> roleManager, ITokenService tokenService, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
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

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequestDto.Password, lockoutOnFailure: false); //lockoutOnFailure: if true, increments the access failed count for the user if the sign-in fails.

            if (result.IsNotAllowed)
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

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();
            if(role == null)
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = "User has no role assigned"
                };

            AuthResponseDto response = _tokenService.GenerateToken(user, role, null);

            user.RefreshToken = response.RefreshToken;
            user.RefreshTokenExpiryTime = response.RefreshTokenExpirationDateTime;
            await _userManager.UpdateAsync(user);


            return new ApiResponse<AuthResponseDto>
            {
                IsSuccess = true,
                Message = "Login successful",
                Data = response
            };

        }

        public async Task LogoutAsync()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if(userId is null)
                throw new UnauthorizedAccessException("User is not logged in");

            var user = await _userManager.FindByIdAsync(userId);
            user!.IsRevoked = true;
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

            //Assign Role to the user
            await _userManager.AddToRoleAsync(user, registerDto.Role.ToString()!);
          
            return new ApiResponse
            {
                IsSuccess = true,
                Message = "Registration successful"
            };
        } 
    }
}
