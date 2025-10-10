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
    public class OrderStatusConfiguration : BaseEntityConfiguration<OrderStatus, int>
    {

        public override void Configure(EntityTypeBuilder<OrderStatus> builder)
        {
            builder.Property(os => os.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasData(
                new OrderStatus { Id = 1, Name = "Pending" },
                new OrderStatus { Id = 2, Name = "Confirmed" },
                new OrderStatus { Id = 3, Name = "Cancelled" }
            );
        }
    }
}
