using Application.DTOs._Common;
using Application.DTOs.Auth;
using Application.Intefraces.IServices;
using Domain.IdentityEntities;
using ECommerceApp.Controllers._Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceApp.Controllers
{
    [AllowAnonymous]
    public class AuthController : CustomControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        public AuthController(IAuthService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ApiResponse> Register([FromBody]RegisterRequestDto registerRequestDto)
        {
            //Model binding Vaidation
            if(ModelState.IsValid == false)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return new ApiResponse<IEnumerable<string>>
                {
                    IsSuccess = false,
                    Message = "Validation errors occurred.",
                    Data = errors
                };
            }

            var response = await _authService.RegisterAsync(registerRequestDto);
            return response;


        }

        [HttpPost("login")]
        public async Task<ApiResponse> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            //Model binding Vaidation
            if (ModelState.IsValid == false)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return new ApiResponse<IEnumerable<string>>
                {
                    IsSuccess = false,
                    Message = "Validation errors occurred.",
                    Data = errors
                };
            }
            var response = await _authService.LoginAsync(loginRequestDto);
            return response;
        }

        [HttpPost("generate")]
        public async Task<ApiResponse> GenerateNewAccessToken(TokenDto tokenDto)
        {
            if (tokenDto == null)
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Invalid Client Request"
                };

            return await _tokenService.RefreshExpiredToken(tokenDto);
        }


        [HttpPost("logout")]
        public async Task<ApiResponse> Logout()
        {
            await _authService.LogoutAsync();
            return new ApiResponse
            {
                IsSuccess = true,
                Message = "Logout successful"
            };
        }
    }
}
