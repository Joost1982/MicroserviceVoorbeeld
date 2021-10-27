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
                SeedData(serviceScope.ServiceProvider.GetService<CommanderContext>(), isProductionEnv);
            }

        }

        private static void SeedData(CommanderContext context, bool isProd)
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
                    new Command() { HowTo = "Computer opstarten", Line = "Op aan knop duwen", Platform = "PC" },
                    new Command() { HowTo = "git updaten", Line = "git push", Platform = "Github" },
                    new Command() { HowTo = "migrations doorvoeren", Line = "dotnet ef database update", Platform = "dotnet core" }
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
