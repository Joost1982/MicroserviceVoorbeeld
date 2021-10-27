using Commander.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Commander.Data
{
    public class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProductionEnv)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<CommandContext>(), isProductionEnv);
            }

        }

        private static void SeedData(CommandContext context, bool isProd)
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


            if (!context.Commands.Any())
            {
                Console.WriteLine("--> Seeding Data...");

                context.Commands.AddRange(
                    new Command() { HowTo = "Computer opstarten", Line = "Op aan knop duwen", PlatformId = 1 },
                    new Command() { HowTo = "git updaten", Line = "git push", PlatformId = 2 },
                    new Command() { HowTo = "migrations doorvoeren", Line = "dotnet ef database update", PlatformId = 1 }
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
