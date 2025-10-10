using Domain._Common;
using Domain.IdentityEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Payment : BaseAuditableEntity<Guid>
    {
        public Guid UserID { get; set; }
        public User? User { get; set; }

        public int PaymentMethodId { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }

        [Range(0, int.MaxValue)]
        [Precision(18, 2)]
        public Decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public int PaymentStatusId { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }

       public Order? Order { get; set; }
    }
}
