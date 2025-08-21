using Domain._Common;
using Domain.Validation;
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

        public Guid userId { get; set; }
        public User? user { get; set; }

        public int orderStatusId { get; set; }
        public OrderStatus? orderStatus { get; set; }

        public Guid paymentId { get; set; }
        public Payment? payment { get; set; }

        [Range(0, int.MaxValue)]
        public decimal totalAmount { get; set; }

        [PastDateValidator]
        public DateTime orderDate { get; set; }



        public ICollection<OrderItem> orderItems { get; set; } = new HashSet<OrderItem>();

    }
}
