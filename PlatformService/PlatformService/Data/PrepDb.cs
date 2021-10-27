using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlatformService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformService.Data
{
    public static class PrepDb // klasse just for testing
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProductionEnv) 
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProductionEnv);
            }

        }

        private static void SeedData(AppDbContext context, bool isProd)
        {
            if (isProd)
            {
                Console.WriteLine("--> Applying db migrations");
                try
                {
                context.Database.Migrate();
                } 
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not apply migrations: {ex.Message}");
                }
            }


            if (!context.Platforms.Any())
            {
                Console.WriteLine("--> Seeding Data...");

                context.Platforms.AddRange(
                    new Platform() { Name="Dot net", Publisher = "Microsoft", Cost="Free"},
                    new Platform() { Name="SQL Server", Publisher = "Microsoft", Cost="Free"},
                    new Platform() { Name="Spring Boot", Publisher = "Oracle", Cost="Not expensive"}
                    );

                context.SaveChanges();
            }
            else 
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}
