using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EggTypeService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EggTypeService.Data
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


            if (!context.EggTypes.Any())
            {
                Console.WriteLine("--> Seeding Data...");

                context.EggTypes.AddRange(
                    new EggType() { Description= "Omega-3 braun", EggTypeGroupParameterCode = 0, EggColorParameterCode  = 1},
                    new EggType() { Description= "Omega-3 weiss", EggTypeGroupParameterCode = 3, EggColorParameterCode  = 0},
                    new EggType() { Description= "Bio ei", EggTypeGroupParameterCode = 1, EggColorParameterCode  = 21}
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
