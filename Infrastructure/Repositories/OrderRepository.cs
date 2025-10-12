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
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly AppDbContext _appDbContext;
        public OrderRepository(AppDbContext context) : base(context)
        {
            _appDbContext = context;
        }

        public async Task<List<Order>> GetUserPastOrdersAsync(Guid userId)
        {
            return await _appDbContext.Orders.Where(o => o.UserId == userId).ToListAsync();
        }
    }
}
