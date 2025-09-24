using Application.Intefraces._Common;
using Application.Intefraces.IServices;
using Application.Intefraces.Repositories;
using Infrastructure.Persistence._Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    internal class UnitOfWork(AppDbContext context) : IUnitOfWork
    {
        private IProductRepository? _productService;

        public IProductRepository Products => _productService ??= new ProductRepository(context);

        public async Task BeginTransactionAsync()
        {
            await context.Database.BeginTransactionAsync();
        }

        public async Task<int> CommitAsync()
        {
            return await context.SaveChangesAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await context.Database.CommitTransactionAsync();
        }

        public async Task RollBackTransactionAsync()
        {
            await context.Database.RollbackTransactionAsync();
        }
    }
}
