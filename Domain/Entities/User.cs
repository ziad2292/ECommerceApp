using Domain._Common;
using Domain.Enums;
using Domain.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User : BaseAuditableEntity<Guid>
    {
        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? PasswordHash { get; set; }

        public Gender? Gender { get; set; }

        [MinimumYearValidator]
        public DateTime BirthDate { get; set; }

        [Phone]
        public string? Phone { get; set; }

        [NotMapped]
        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - BirthDate.Year;

                //Adjust if birthday hasn't occurred yet this year
                if (BirthDate.Date > today.AddYears(-age))
                    age--;

                return age;
            }
        }

        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();

        public ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();

        public ShoppingCart? ShoppingCart { get; set; }

    }
}
