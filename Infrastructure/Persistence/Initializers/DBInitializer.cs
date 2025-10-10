using Application.Intefraces.Initializers;
using Domain.Enums;
using Domain.IdentityEntities;
using Infrastructure.Persistence._Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Initializers
{
    public class DBInitializer(AppDbContext _context, RoleManager<Role> roleManager) : IDbInitializer
    {
        public async Task InitializeDbAsync()
        {
            var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
            {
                await _context.Database.MigrateAsync(); // Update the database to the latest migration
            }
        }

        public async Task SeedAsync()
        {
            //Seed Roles
            foreach (UserTypeEnum userTypeEnum in Enum.GetValues(typeof(UserTypeEnum)))
            {
                if (!await roleManager.RoleExistsAsync(userTypeEnum.ToString()))
                    await roleManager.CreateAsync(new Role { Name = userTypeEnum.ToString() });

            }
        }

    }
}
