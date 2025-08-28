using Domain._Common;
using Domain.Enums;
using Domain.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.IdentityEntities
{
    public class User : IdentityUser<Guid>
    {
        public Gender? Gender { get; set; }

        public DateOnly BirthDate { get; set; }

        [NotMapped]
        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - BirthDate.Year;

                //Adjust if birthday hasn't occurred yet this year
                if (BirthDate.DayOfYear < today.DayOfYear)
                    age--;

                return age;
            }
        }

        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();

        public ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();

        public ShoppingCart? ShoppingCart { get; set; }

    }
}
