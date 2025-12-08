using Application.Intefraces._Common;
using Application.Intefraces.Initializers;
using Application.Intefraces.IServices;
using Application.Intefraces.Repositories;
using Infrastructure.Persistence._Data;
using Infrastructure.Persistence.Initializers;
using Infrastructure.Repositories;
using Infrastructure.Services;
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
            

            return services;
        }
    }
}
