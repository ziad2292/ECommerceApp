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
        [Required(ErrorMessage = "Username can't be blank.")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "Email can't be blank.")]
        [EmailAddress(ErrorMessage = "Email should be in a proper format.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password can't be blank.")]
        [RegularExpression(@"(?=^.{6,20}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#%^&*()_+}{"":;'?/>\.<,])(?!.*\s).*$",
            ErrorMessage = "Password must be 6-20 characters and include 1 uppercase, 1 lowercase, 1 number, and 1 special character.")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Password can't be blank.")]
        [Compare("Password", ErrorMessage = "Passwords don't match.")]
        public required string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Phone Number can't be blank.")]
        [Phone]
        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Choose a gender.")]
        public required int gender { get; set; }

        [Required(ErrorMessage = "Select a Birthdate.")]
        [MinimumYearValidator]
        public required DateOnly BirthDate { get; set; }


    }
}
