using Domain.Entities;
using Infrastructure.Persistence._Data.Configurations._Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence._Data.Configurations
{
    public class CategoryConfiguration : BaseEntityConfiguration<Category, int>
    {

        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(c => c.Name).IsUnique();

            builder.HasData(
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Books" },
                new Category { Id = 3, Name = "Clothing" },
                new Category { Id = 4, Name = "Home & Kitchen" },
                new Category { Id = 5, Name = "Sports & Outdoors" },
                new Category { Id = 6, Name = "Health & Beauty" },
                new Category { Id = 7, Name = "Toys & Games" },
                new Category { Id = 8, Name = "Automotive" },
                new Category { Id = 9, Name = "Jewelry & Accessories" },
                new Category { Id = 10, Name = "Groceries" },
                new Category { Id = 11, Name = "Pet Supplies" },
                new Category { Id = 12, Name = "Office Supplies" }
            );

        }
    }
}
