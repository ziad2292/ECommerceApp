using Application.Intefraces._Common;
using Application.Intefraces.Initializers;
using Application.Intefraces.IServices;
using Application.Intefraces.Repositories;
using Application.Settings;
using Infrastructure;
using Infrastructure.Persistence._Data;
using Infrastructure.Persistence.Initializers;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace ECommerceApp
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure Redis Connection
            var redisConnection = configuration.GetConnectionString("Redis") ?? throw new InvalidOperationException("Redis connection string is required");
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configuration = ConfigurationOptions.Parse(redisConnection);
                configuration.AbortOnConnectFail = false;
                configuration.ConnectRetry = 3;
                configuration.ConnectTimeout = 5000;
                return ConnectionMultiplexer.Connect(configuration);
            });

            //Cache Service
            services.AddSingleton<ICacheService, RedisCacheService>();

            //Rate Limit Service
            services.AddScoped<IRateLimitService, FixedWindowRateLimitService>();

            services.AddDbContext<AppDbContext>(optionsBuilder =>
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddScoped<IDbInitializer, DBInitializer>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IShoppingCartService, ShoppingCartService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();

            return services;
        }
    }
}
