using Microsoft.EntityFrameworkCore;
using QLNhaTro.Moddel;
using QLNhaTro.Moddel.SeedData;

namespace QLNhaTro.API.Extensions
{
    public static class DbMigrationExtension
    {
        public static void UseDbMigration(this IApplicationBuilder app, bool isDevelopment)
        {
            using(var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
                context.Database.Migrate();
                SeedDataV1.SeedData(context);
                /*if (isDevelopment)
                {
                    DataSeeding.DevelopmentSeeding(context);
                }
                else
                {
                    DataSeeding.ProductionSeeding(context);
                } */
            }    
        }
    }
}
