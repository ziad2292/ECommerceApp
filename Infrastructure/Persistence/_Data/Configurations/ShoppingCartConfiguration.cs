using Domain.Entities;
using Infrastructure.Persistence._Data.Configurations._Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;    
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence._Data.Configurations
{
    public class ShoppingCartConfiguration : BaseAuditableEntityConfiguration<ShoppingCart, Guid>
    {
        public override void Configure(EntityTypeBuilder<ShoppingCart> builder)
        {
            builder.HasOne(sc => sc.User)
                   .WithOne(u => u.ShoppingCart)
                   .HasForeignKey<ShoppingCart>(sc => sc.UserId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();
        }
    }
}
