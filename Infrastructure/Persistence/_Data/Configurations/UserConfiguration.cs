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
    public class UserConfiguration :  BaseAuditableEntityConfiguration<User, Guid>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Name)
                   .HasMaxLength(100);

            builder.Property(u => u.Email)
                .HasMaxLength(100);

            builder.Property(u => u.Phone)
                .HasMaxLength(15);
        }
    }
}
