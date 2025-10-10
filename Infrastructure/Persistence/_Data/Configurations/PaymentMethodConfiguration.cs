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
    public class PaymentMethodConfiguration : BaseEntityConfiguration<PaymentMethod, int>
    {
        public override void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder.Property(pm => pm.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasData(
                new PaymentMethod { Id = 1, Name = "Cash" }
            );
        }
    }
}
