using Application.Intefraces.Initializers;
using Infrastructure.Persistence._Data;
using Infrastructure.Persistence.Initializers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(optionsBuilder =>
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddScoped<IDbInitializer, DBInitializer>();

            return services;
        }
    }
}
