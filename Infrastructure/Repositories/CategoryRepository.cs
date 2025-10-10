using Application.Intefraces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence._Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category> , ICategoryRepository
    {
        public CategoryRepository(AppDbContext appDbContext) : base(appDbContext) {}
    }
}
