using Application.Intefraces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence._Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ShoppingCartItemRepository : GenericRepository<ShoppingCartItem>, IShoppingCartItemRepository
    {
        private readonly AppDbContext _appDbContext;
        public ShoppingCartItemRepository(AppDbContext appDbContext) : base(appDbContext) {
            _appDbContext = appDbContext;
        }

        public async Task<bool> ShoppingItemExistsAsync(Guid cartId, Guid productId)
        {
           return await _appDbContext.ShoppingCartItems.AnyAsync(i => i.ShoppingCartId == cartId && i.ProductId == productId);   
        }

        public async Task EmptyShoppingCartAsync(Guid cartId)
        {
            var items = await _appDbContext.ShoppingCartItems
                .Where(i => i.ShoppingCartId == cartId)
                .ToListAsync();

            if (items.Any())
            {
                _appDbContext.ShoppingCartItems.RemoveRange(items);
            }
        }

    }
}
