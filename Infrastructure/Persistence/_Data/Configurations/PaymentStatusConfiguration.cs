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
    public class PaymentStatusConfiguration : BaseEntityConfiguration<PaymentStatus, int>
    {
        public override void Configure(EntityTypeBuilder<PaymentStatus> builder)
        {
            builder.Property(ps => ps.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasData(
                new PaymentStatus { Id = 1, Name = "Pending" },
                new PaymentStatus { Id = 2, Name = "Processing" },
                new PaymentStatus { Id = 3, Name = "Paid" },
                new PaymentStatus { Id = 4, Name = "Cancelled" }
            );
        }
    }
}
