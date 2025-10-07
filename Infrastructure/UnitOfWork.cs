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
        private ICategoryRepository? _categoryService;
        private IShoppingCartRepository? _cartRepository;
        private IShoppingCartItemRepository? _itemRepository;

        public IProductRepository Products => _productService ??= new ProductRepository(context);
        public ICategoryRepository Categories => _categoryService ??= new CategoryRepository(context);
        public IShoppingCartRepository ShoppingCarts => _cartRepository ??= new ShoppingCartRepository(context);
        public IShoppingCartItemRepository ShoppingCartItems => _itemRepository ??= new ShoppingCartItemRepository(context);

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
