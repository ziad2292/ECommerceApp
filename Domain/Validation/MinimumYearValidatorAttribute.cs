using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Validation
{
    public class MinimumYearValidatorAttribute : ValidationAttribute
    {
        public int minimumAge = 18;

        public string defaultErrorMessage { get; set; } = "User must be 18 years old or above";

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is DateTime birthDate)
            {
                var user = (User)validationContext.ObjectInstance;

                if (user.Age < minimumAge)
                    return new ValidationResult(defaultErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
