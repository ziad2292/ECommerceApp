using Domain._Common;
using Domain.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ShoppingCart : BaseAuditableEntity<Guid>
    {
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public ICollection<ShoppingCartItem> Items { get; set; } = new HashSet<ShoppingCartItem>();
    }
}
