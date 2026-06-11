using Application.Intefraces.Initializers;

namespace ECommerceApp.Extensions
{
    public static class InitializerExtension
    {
        public static async Task<WebApplication> InitializeDbAsync(this WebApplication app)
        {
            const int maxAttempts = 10;

            for (var attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    using var scope = app.Services.CreateScope();
                    var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                    await dbInitializer.InitializeDbAsync();
                    await dbInitializer.SeedAsync();
                    return app;
                }
                catch (Exception ex) when (attempt < maxAttempts)
                {
                    app.Logger.LogWarning(ex, "Database initialization failed. Retrying attempt {Attempt}/{MaxAttempts}", attempt, maxAttempts);
                    await Task.Delay(TimeSpan.FromSeconds(5));
                }
            }

            using (var scope = app.Services.CreateScope())
            {
                var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                await dbInitializer.InitializeDbAsync();
                await dbInitializer.SeedAsync();
            }

            return app;
        }
    }
}
