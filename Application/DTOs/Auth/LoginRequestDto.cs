using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Auth
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Email can't be blank.")]
        [EmailAddress(ErrorMessage = "Email should be in a proper format.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password can't be blank.")]
        public required string Password { get; set; }
    }
}
