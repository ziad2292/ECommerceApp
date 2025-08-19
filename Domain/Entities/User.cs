using Domain._Common;
using Domain.Enums;
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
        public string? name { get; set; }

        public string? email { get; set; }
        public string? passwordHash { get; set; }

        public string? role { get; set; }

        public Gender? gender { get; set; }

        public DateTime birthDate { get; set; }

        //Not mapped to the database, calculated property
        public int age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - birthDate.Year;

                //Adjust if birthday hasn't occurred yet this year
                if (birthDate.Date > today.AddYears(-age))
                    age--;

                return age;
            }
        }

        public string? phone { get; set; }


    }
}
