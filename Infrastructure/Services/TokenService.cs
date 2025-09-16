using Application.DTOs._Common;
using Application.DTOs.Auth;
using Application.Intefraces.IServices;
using Application.Settings;
using Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly JWTSettings _jwtSettings;
        private readonly UserManager<User> _userManager;

        public TokenService(IOptions<JWTSettings> jwtSettings, UserManager<User> userManager)
        {
            _jwtSettings = jwtSettings.Value;
            _userManager = userManager;
        }
        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

        public AuthResponseDto GenerateToken(User user, string role, string? ValidRefreshToken)
        {
            DateTime expiration = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryInMin); //Token Expiration time

            //User Claims
            Claim[] claims = new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64), //Issued At - TODO: Check if working (else use: ~UtcNow.ToUnixTimeSeconds()~)
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) //JWT Unique ID
            };

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret!)); //Secret key wrapped for cryptographic use
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256); //pairing of key + algorithm for signing JWTs

            //Token to be Generated
            JwtSecurityToken tokenGenerator= new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: expiration,
                signingCredentials: signingCredentials
                );

            //Generate the token
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            string token = tokenHandler.WriteToken(tokenGenerator);

            AuthResponseDto newToken = new AuthResponseDto
            {
                AccessToken = token,
                RefreshToken = ValidRefreshToken ?? GenerateRefreshToken(),
                RefreshTokenExpirationDateTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryInDay)
            };

            return newToken;
        }

        //To refresh the token without forcing the user to login again
        public ClaimsPrincipal? GetUserInfoFromExpiredToken(string? token)
        {
            if(string.IsNullOrWhiteSpace(token))
                return null;

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret!)),
                ValidateLifetime = false //We are checking for expired tokens here, so we don't validate lifetime
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            ClaimsPrincipal principal = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken); //Validate the token and retrieve the security token(Raw representation of the token)

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase)){
                return null;
            }

            return principal;
        }

        public async Task<ApiResponse> RefreshExpiredToken(TokenDto token)
        {
            //Extract Claims from expired access token
            var principal = GetUserInfoFromExpiredToken(token.AccessToken);
            if (principal == null)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Invalid Access Token"
                };
            }

            //Get userId from extracted claims
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = "User Id not found in the token claims"
                };
            }

            //Get User from database
            User? user = await _userManager.FindByIdAsync(userId); 
            if (user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Invalid Refresh Token"
                };
            }

            var role = principal?.FindFirst(ClaimTypes.Role)?.Value;
            if (role == null)
            {
                return new ApiResponse<AuthResponseDto>
                {
                    Message = "User role not found in the token claims",
                    IsSuccess = false,
                };
            }

            AuthResponseDto response = GenerateToken(user, role.ToString(), token.RefreshToken);
            user.RefreshToken = response.RefreshToken;
            user.RefreshTokenExpiryTime = response.RefreshTokenExpirationDateTime;
            await _userManager.UpdateAsync(user);

            //Generate new token
            return new ApiResponse<AuthResponseDto>
            {
                IsSuccess = true,
                Message = "Token refreshed successfully",
                Data = response
            };
        }

    }
}
