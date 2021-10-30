using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlatformService.Models;

namespace PlatformService.Data
{
    public static class PrepareDatabase
    {
        public static void Prepare(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                SeedData(context,isProd);
            }
        }
        private static void SeedData(AppDbContext context, bool isProd)
        {
            if(isProd){
                Console.WriteLine("--> Attempting to apply migrations");
                try
                {
                    context.Database.Migrate();
                }
                catch(Exception ex)
                {
                    Console.WriteLine("--> Error applying migrations: " + ex.Message);
                }
            }
            if(!context.Platforms.Any())
            {
                Console.WriteLine("--> Seeding Data...");
                context.Platforms.AddRange(
                    new Platform { Name = "Dot Net",Publisher = "Microsoft", Cost="Free" },
                    new Platform { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
                    new Platform { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
                );
                context.SaveChanges();
            }
            else{
                Console.WriteLine("--> Platforms already exist");
            }
        }
    }
}