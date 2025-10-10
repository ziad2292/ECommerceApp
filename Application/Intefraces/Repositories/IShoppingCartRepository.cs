using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Intefraces.Repositories
{
    public interface IShoppingCartRepository : IGenericRepository<ShoppingCart>
    {
        Task<ShoppingCart?> GetByUserIdAsync(Guid userId);
    }
}
