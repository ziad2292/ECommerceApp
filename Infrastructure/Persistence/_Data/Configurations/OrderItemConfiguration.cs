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
    public class OrderItemConfiguration : BaseEntityConfiguration<OrderItem, Guid>
    {
        public override void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasOne(OrderItem => OrderItem.Order)
                   .WithMany(Order => Order.OrderItems)
                   .HasForeignKey(OrderItem => OrderItem.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(OrderItem => OrderItem.Product)
                   .WithMany(Product => Product.OrderItems)
                   .HasForeignKey(OrderItem => OrderItem.ProductId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(oi => oi.OrderId);
        }
    }
}
