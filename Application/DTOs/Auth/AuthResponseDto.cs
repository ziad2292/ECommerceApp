using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Auth
{
    public class AuthResponseDto
    {
        public required string AccessToken {  get; set; } 
        public required string RefreshToken { get; set; }   
        public required DateTime RefreshTokenExpirationDateTime { get; set; }
    }
}
