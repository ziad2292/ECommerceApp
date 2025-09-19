using Application.DTOs.Auth;
using Domain.IdentityEntities;
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
        public int minimumYear = 18;

        public string defaultErrorMessage { get; set; } = "User must be 18 years old or above";

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

                var BirthDateProp = validationContext.ObjectType.GetProperty("BirthDate");
            if (BirthDateProp != null)
            {
                var BirthDate = BirthDateProp.GetValue(validationContext.ObjectInstance);

                if (BirthDate is DateOnly birthDate)
                {
                    var today = DateTime.Today;
                    var age = today.Year - birthDate.Year;

                    //Adjust if birthday hasn't occurred yet this year
                    if (birthDate.DayOfYear < today.DayOfYear)
                        age--;

                    if (age < minimumYear)
                        return new ValidationResult(defaultErrorMessage);
                }



                return ValidationResult.Success;
            }
            else
            {
                throw new Exception();
            }


        }
    }
}
