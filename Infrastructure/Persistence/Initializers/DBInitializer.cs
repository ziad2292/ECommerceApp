using Application.Intefraces.Initializers;
using Domain.Entities;
using Domain.Enums;
using Domain.IdentityEntities;
using Infrastructure.Persistence._Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Initializers
{
    public class DBInitializer(AppDbContext _context, RoleManager<Role> roleManager) : IDbInitializer
    {
        public async Task InitializeDbAsync()
        {
            var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();

            if (pendingMigrations.Any())
            {
                await _context.Database.MigrateAsync();
            }
        }

        public async Task SeedAsync()
        {
            foreach (UserTypeEnum userTypeEnum in Enum.GetValues(typeof(UserTypeEnum)))
            {
                if (!await roleManager.RoleExistsAsync(userTypeEnum.ToString()))
                    await roleManager.CreateAsync(new Role { Name = userTypeEnum.ToString() });
            }

            await SeedDemoCatalogAsync();
            await _context.SaveChangesAsync();
        }

        private async Task SeedDemoCatalogAsync()
        {
            string[] categoryNames = ["Electronics", "Clothing", "Home"];
            foreach (var categoryName in categoryNames)
            {
                if (!await _context.Categories.AnyAsync(c => c.Name == categoryName))
                    await _context.Categories.AddAsync(new Category { Name = categoryName });
            }

            await _context.SaveChangesAsync();

            var electronicsId = await GetCategoryIdAsync("Electronics");
            var clothingId = await GetCategoryIdAsync("Clothing");
            var homeId = await GetCategoryIdAsync("Home");

            await AddDemoProductIfMissingAsync(
                "Wireless Headphones",
                "Comfortable Bluetooth headphones for everyday listening.",
                79.99m,
                "https://images.unsplash.com/photo-1505740420928-5e560c06d30e",
                electronicsId,
                25);

            await AddDemoProductIfMissingAsync(
                "Classic Cotton T-Shirt",
                "Soft cotton t-shirt available for a simple demo purchase.",
                19.99m,
                "https://images.unsplash.com/photo-1521572163474-6864f9cf17ab",
                clothingId,
                50);

            await AddDemoProductIfMissingAsync(
                "Ceramic Coffee Mug",
                "Reusable ceramic mug for home and office.",
                12.50m,
                "https://images.unsplash.com/photo-1514228742587-6b1558fcca3d",
                homeId,
                40);
        }

        private async Task<int> GetCategoryIdAsync(string name)
        {
            return await _context.Categories
                .Where(c => c.Name == name)
                .Select(c => c.Id)
                .FirstAsync();
        }

        private async Task AddDemoProductIfMissingAsync(string name, string description, decimal price, string imageUrl, int categoryId, int stock)
        {
            if (await _context.Products.AnyAsync(p => p.Name == name))
                return;

            await _context.Products.AddAsync(new Product
            {
                Name = name,
                Description = description,
                Price = price,
                ImageUrl = imageUrl,
                CategoryId = categoryId,
                Stock = stock,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "SEED"
            });
        }
    }
}
