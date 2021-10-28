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

                //Les Jackson gebruikt een InMem voor de Command Service waar deze Flock Service op gebaseerd is
                //Die InMem variant checkt niet op foreign key contraints en daardoor gaat het toevoegen van Flocks
                //zonder records in de EggTypes tabel goed.
                //Ik gebruik echter een MS-SQL db en dan gaat het mis want de fk's ontbreken. Die voeg ik hieronder
                //dus eerst toe.
                
                context.EggTypes.AddRange(
                    new EggType() { ExternalId = 10, Description = "Bruin Bio" },
                    new EggType() { ExternalId = 20, Description = "Wit" },
                    new EggType() { ExternalId = 30, Description = "Vrije uitloop" }
                    );

                context.Flocks.AddRange(
                    new Flock() { FlockCode = "1111-22", Description = "Bennekom stal", EggTypeId = 1 },
                    new Flock() { FlockCode = "1111-26", Description = "Bennekom stal2", EggTypeId = 3 },
                    new Flock() { FlockCode = "1122-22", Description = "Amerongen stal", EggTypeId = 3 }
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
