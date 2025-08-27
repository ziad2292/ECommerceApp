using Domain.IdentityEntities;
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
    public class UserConfiguration :  IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.UserName)
                   .HasMaxLength(100);

            builder.Property(u => u.Email)
                .HasMaxLength(100);

            builder.HasAlternateKey(u => u.Email);

            builder.Property(u => u.PhoneNumber)
                .HasMaxLength(15);
        }
    }
}
