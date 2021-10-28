using Microsoft.AspNetCore.Builder;
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
                SeedData(serviceScope.ServiceProvider.GetService <IEggTypeRepo>() , isProductionEnv);
            }

        }

        private static void SeedData(IEggTypeRepo repository, bool isProd)
        {

            if (!repository.GetAllEggTypes().Any())
            {
                Console.WriteLine("--> Seeding Data...");

                List<EggType> lijstje = new List<EggType> {
                    new EggType() { Description = "Omega-3 braun", EggTypeGroupParameterCode = 0, EggColorParameterCode = 1 },
                    new EggType() { Description = "Omega-3 weiss", EggTypeGroupParameterCode = 3, EggColorParameterCode = 0 },
                    new EggType() { Description = "Bio ei", EggTypeGroupParameterCode = 1, EggColorParameterCode = 21 }
                    };
                foreach (var eggType in lijstje)
                {
                    repository.CreateEggType(eggType);
                }
                
            }
            else 
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}
