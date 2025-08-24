using Domain._Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product : BaseAuditableEntity<Guid>
    {
        public string? Name { get; set; }

        public string? Description { get; set; }

        [Range(0, int.MaxValue)]
        public decimal Price { get; set; }

        [Url]
        public string? ImageUrl { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();

        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; } = new HashSet<ShoppingCartItem>();
    }
}
