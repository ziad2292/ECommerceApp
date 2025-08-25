using Application.Intefraces.Initializers;
using Infrastructure.Persistence._Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Initializers
{
    public class DBInitializer(AppDbContext _context) : IDbInitializer
    {
        public async Task InitializeDbAsync()
        {
            var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
            {
                await _context.Database.MigrateAsync(); // Update the database to the latest migration
            }
        }
        
    }
}
