using Domain.Entities;
using Infrastructure.Persistence._Data.Configurations._Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence._Data.Configurations
{
    public class ShoppingCartItemConfiguration : BaseEntityConfiguration<ShoppingCartItem, Guid>
    {  
        public override void Configure(EntityTypeBuilder<ShoppingCartItem> builder)
        {

            builder.HasOne(sci => sci.Product)
                .WithMany(p => p.ShoppingCartItems)
                .HasForeignKey(sci => sci.ProductId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(sci => sci.ShoppingCart)
                .WithMany(sc => sc.Items)
                .HasForeignKey(sci => sci.ShoppingCartId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
