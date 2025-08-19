using Domain._Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PaymentStatus : BaseEntity<int>
    {
        public required string name { get; set; }
    }
}
