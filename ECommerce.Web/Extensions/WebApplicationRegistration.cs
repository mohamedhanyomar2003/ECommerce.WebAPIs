using E_Commerce.Domain.Contracts;
using E_Commerce.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace ECommerce.Web.Extensions
{
    public static class WebApplicationRegistration
    {
        public static WebApplication MigrateDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<StoreDbContext>();
            if (dbContext.Database.GetPendingMigrations().Any())
                dbContext.Database.Migrate();
            return app;
        }

        public static WebApplication SeedDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dataInitializerService = scope.ServiceProvider.GetRequiredService<IDataInitializer>();
            dataInitializerService.Initialize();
            return app;
        }
    }
}
