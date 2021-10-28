using ProductService.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Data
{
    public class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProductionEnv)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<ProductContext>(), isProductionEnv);
            }

        }

        private static void SeedData(ProductContext context, bool isProd)
        {

            if (!context.Products.Any())
            {
                Console.WriteLine("--> Seeding Data...");

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
