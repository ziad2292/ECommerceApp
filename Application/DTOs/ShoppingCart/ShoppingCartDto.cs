using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ShoppingCart
{
    public class ShoppingCartDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public ICollection<GetShoppingCartItemDto>? Items { get; set; }
    }
}
