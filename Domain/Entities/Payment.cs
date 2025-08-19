using Domain._Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Payment : BaseAuditableEntity<Guid>
    {
        public Guid userID { get; set; }
        public User? user { get; set; }

        public int paymentMethodId { get; set; }
        public PaymentMethod? paymentMethod { get; set; }

        public Decimal amount { get; set; }

        public DateTime paymentDate { get; set; }

        public int paymentStatusId { get; set; }
        public PaymentStatus? paymentStatus { get; set; }
    }
}
