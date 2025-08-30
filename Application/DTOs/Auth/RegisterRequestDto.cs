using Domain.Enums;
using Domain.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Auth
{
    public class RegisterRequestDto
    {
        [Required]
        public required string UserName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        [Required]
        [Compare("Password")]
        public required string ConfirmPassword { get; set; }

        [Required]
        [Phone]
        public required string PhoneNumber { get; set; }

        [Required]
        public required int gender { get; set; }

        [Required]
        [MinimumYearValidator]
        public required DateOnly BirthDate { get; set; }


    }
}
