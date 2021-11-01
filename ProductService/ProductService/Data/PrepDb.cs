using ProductService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductService.SyncDataServices.Grpc;

namespace ProductService.Data
{
    public class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProductionEnv)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<IEggTypeDataClient>();

                var eggTypes = grpcClient.ReturnAllEggTypes();

                SeedData(serviceScope.ServiceProvider.GetService<ProductContext>(),
                    serviceScope.ServiceProvider.GetService<IProductRepo>(),
                    isProductionEnv, eggTypes);
            }

        }

        private static void SeedData(ProductContext context, IProductRepo repo, bool isProd, IEnumerable<EggType> eggTypes)
        {

            if (!context.Products.Any())
            {
                Console.WriteLine("--> Seeding Data...");

                //EggTypes
                foreach (var eggType in eggTypes)
                {
                    if (!repo.ExternalEggTypeIdExists(eggType.ExternalId))
                    {
                        repo.CreateEggType(eggType);
                    }
                    repo.SaveChanges();
                }

                //Products
                context.Products.AddRange(
                    new Product() { ProductCode = "1111-22", isActive = true, EggTypeId = 1 },
                    new Product() { ProductCode = "1111-26", isActive = true , EggTypeId = 3 },
                    new Product() { ProductCode = "1122-22", isActive = false, EggTypeId = 3 }
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
