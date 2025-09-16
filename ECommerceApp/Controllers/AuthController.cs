using Application.DTOs._Common;
using Application.DTOs.Auth;
using Application.Intefraces.IServices;
using Domain.IdentityEntities;
using ECommerceApp.Controllers._Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public async Task<IActionResult> Register([FromBody]RegisterRequestDto registerRequestDto)
        {
            //Model binding Vaidation
            if(ModelState.IsValid == false)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiResponse<IEnumerable<string>>
                {
                    IsSuccess = false,
                    Message = "Validation errors occurred.",
                    Data = errors
                });
            }

            var response = await _authService.RegisterAsync(registerRequestDto);

            if (response.IsSuccess)
            {
                return Ok(response);
            }
            else
            {
                return response.Message switch
                {
                    "The Email is already registered" => Conflict(response),
                    "Registration failed" => Unauthorized(response),
                    _ => BadRequest(response)
                };
            }


        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
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
            var response = await _authService.LoginAsync(loginRequestDto);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            else
            {
                return Unauthorized(response);
            }
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateNewAccessToken(TokenDto tokenDto)
        {
            if (tokenDto == null)
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Invalid Client Request"
                });

            var response = await _tokenService.RefreshExpiredToken(tokenDto);
            
            if(response.IsSuccess)
            {
                return Ok(response);
            }
            else
            {
                return Unauthorized(response);
            }
        }


        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Logout successful"
            });
        }
    }
}
