using Application.DTOs._Common;
using Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices
{
    public interface IAuthService
    {
        //TODO: Modify return types and parameters
        Task<ApiResponse> RegisterAsync(RegisterRequestDto requestDto);

        Task<ApiResponse> LoginAsync(LoginRequestDto loginRequestDto);

        Task LogoutAsync();
    }
}
