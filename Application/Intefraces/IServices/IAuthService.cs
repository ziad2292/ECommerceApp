using Application.DTOs._Common;
using Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Intefraces.IServices
{
    public interface IAuthService
    {
        Task<ApiResponse> RegisterAsync(RegisterRequestDto requestDto);

        Task<ApiResponse> LoginAsync(LoginRequestDto loginRequestDto);

        Task LogoutAsync();
    }
}
