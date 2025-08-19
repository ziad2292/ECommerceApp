using Domain._Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ShoppingCartItem : BaseEntity<Guid>
    {
        public Guid ShoppingCartId { get; set; }
        public ShoppingCart? ShoppingCart { get; set; }

        public Guid ProductId { get; set; }
        public Product? Product { get; set; }

        public int Quantity { get; set; }
    }
}
