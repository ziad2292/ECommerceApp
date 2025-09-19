using Domain.Enums;
using Domain.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Account
{
    public class UserDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Username can't be blank.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Email can't be blank.")]
        [EmailAddress(ErrorMessage = "Email should be in a proper format.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Phone Number can't be blank.")]
        [Phone]
        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Select a Birthdate.")]
        [MinimumYearValidator]
        public DateOnly BirthDate { get; set; }

        [Required(ErrorMessage = "Choose a gender.")]
        public Gender Gender { get; set; }
    }
}
