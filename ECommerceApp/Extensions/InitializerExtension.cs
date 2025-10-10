using Application.Intefraces.Initializers;

namespace ECommerceApp.Extensions
{
    public static class InitializerExtension
    {
        public static async Task<WebApplication> InitializeDbAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                try
                {
                    await dbInitializer.InitializeDbAsync();
                    await dbInitializer.SeedAsync();
                }
                catch (Exception){

                }
            }
            return app;
        }
    }
}
