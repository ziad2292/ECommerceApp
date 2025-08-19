using Domain._Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product : BaseAuditableEntity<Guid>
    {
        public string? name { get; set; }

        public string? description { get; set; }

        public decimal price { get; set; }

        public string? imageUrl { get; set; }

        public int categoryId { get; set; }
        public Category? category { get; set; }

        public int stock { get; set; }
    }
}
