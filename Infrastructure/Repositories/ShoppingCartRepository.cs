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
    public class ShoppingCartRepository : GenericRepository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly AppDbContext _appDbContext;

        public ShoppingCartRepository(AppDbContext appDbContext) : base(appDbContext) {
            _appDbContext = appDbContext;
        }

        public async Task<ShoppingCart?> GetByUserIdAsync(Guid userId)
        {
            return await _appDbContext.ShoppingCarts.FirstOrDefaultAsync(c => c.UserId == userId);
        }
    }
}
