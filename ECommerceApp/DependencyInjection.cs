using Application;
using Application.Intefraces.IServices;
using Infrastructure;
using Infrastructure.Services;

namespace ECommerceApp
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplication();
            services.AddInfrastructure(configuration);

            services.AddScoped<IRateLimitService, FixedWindowRateLimitService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }
    }
}
