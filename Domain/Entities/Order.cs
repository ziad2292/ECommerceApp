using Domain._Common;
using Domain.IdentityEntities;
using Domain.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order : BaseAuditableEntity<Guid>
    {

        public Guid UserId { get; set; }
        public User? User { get; set; }

        public int OrderStatusId { get; set; }
        public OrderStatus? OrderStatus { get; set; }

        public Guid PaymentId { get; set; }
        public Payment? Payment { get; set; }

        [Range(0, int.MaxValue)]
        [Precision(18,2)]
        public decimal TotalAmount { get; set; }

        [PastDateValidator]
        public DateTime OrderDate { get; set; }



        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();

    }
}
