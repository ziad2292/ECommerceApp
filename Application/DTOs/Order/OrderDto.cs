using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Order
{
    public class OrderDto
    {
        public required Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public required Guid PaymentId { get; set; }
        public required OrderStatusEnum OrderStatus { get; set; }
        public required decimal TotoalAmount { get; set; }
        public required DateTime Date { get; set; }
    }
}
