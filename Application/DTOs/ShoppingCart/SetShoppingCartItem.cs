using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ShoppingCart
{
    public class SetShoppingCartItem
    {
        public required Guid ShoppingCartId { get; set; }
        public required Guid ProductId { get; set; }
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
