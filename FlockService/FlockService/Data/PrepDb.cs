using FlockService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlockService.Data
{
    public class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProductionEnv)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<FlockContext>(), isProductionEnv);
            }

        }

        private static void SeedData(FlockContext context, bool isProd)
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


            if (!context.Flocks.Any())
            {
                Console.WriteLine("--> Seeding Data...");

                context.Flocks.AddRange(
                    new Flock() { Id = 1, FlockCode = "1111-22", Description = "Bennekom stal", EggTypeId = 1 },
                    new Flock() { Id = 2, FlockCode = "1111-26", Description = "Bennekom stal2", EggTypeId = 0 },
                    new Flock() { Id = 3, FlockCode = "1122-22", Description = "Amerongen stal", EggTypeId = 3 }
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
