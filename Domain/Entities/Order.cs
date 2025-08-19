using Domain._Common;
using System;
using System.Collections.Generic;
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

        public decimal totalAmount { get; set; }
        public DateTime orderDate { get; set; }

        public ICollection<OrderItem> orderItems { get; set; } = new HashSet<OrderItem>();

    }
}
