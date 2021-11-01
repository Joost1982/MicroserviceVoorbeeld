using FlockService.Models;
using FlockService.SyncDataServices.Grpc;
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
                var grpcClient = serviceScope.ServiceProvider.GetService<IEggTypeDataClient>();

                var eggTypes = grpcClient.ReturnAllEggTypes();

                SeedData(serviceScope.ServiceProvider.GetService<FlockContext>(),
                    serviceScope.ServiceProvider.GetService<IFlockRepo>(),
                    isProductionEnv, eggTypes);
            }

        }

        private static void SeedData(FlockContext context, IFlockRepo repo, bool isProd, IEnumerable<EggType> eggTypes)
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

                //context.EggTypes.AddRange(
                //    new EggType() { Id = 1, ExternalId = 1, Description = "Bruin Bio" },
                //    new EggType() { Id = 2, ExternalId = 2, Description = "Wit" },
                //    new EggType() { Id = 3, ExternalId = 3, Description = "Vrije uitloop" }
                //    );

                //na toevoegen van de gRPC connectie kan bovenstaande uit en doe we dit voor de EggTypes:
                foreach (var eggType in eggTypes)
                {
                    if (!repo.ExternalEggTypeIdExists(eggType.ExternalId))
                    {
                        repo.CreateEggType(eggType);
                    }
                    repo.SaveChanges();
                }



                //en Flocks:

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
