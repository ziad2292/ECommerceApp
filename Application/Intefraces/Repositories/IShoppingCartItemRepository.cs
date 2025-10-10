using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Intefraces.Repositories
{
    public interface IShoppingCartItemRepository : IGenericRepository<ShoppingCartItem>
    {
        Task<bool> ShoppingItemExistsAsync(Guid cartId, Guid productId);
        Task EmptyShoppingCartAsync(Guid cartId);
    }
}
