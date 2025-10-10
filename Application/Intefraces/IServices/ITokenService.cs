using Application.DTOs._Common;
using Application.DTOs.Auth;
using Domain.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Intefraces.IServices
{
    public interface ITokenService
    {
        AuthResponseDto GenerateToken(User user, string role, string? ValidRefreshToken);

        public string GenerateRefreshToken();

        ClaimsPrincipal? GetUserInfoFromExpiredToken(string? token);

        Task<ApiResponse> RefreshExpiredToken(TokenDto token);
    }
}
