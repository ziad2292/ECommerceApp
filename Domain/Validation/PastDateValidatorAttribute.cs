using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Validation
{
    public class PastDateValidatorAttribute : ValidationAttribute
    {
        public string defaultErrorMessage { get; set; } = "The date must be in the past";
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime date)
            {
                if (date >= DateTime.Now)
                {
                    return new ValidationResult(defaultErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}
