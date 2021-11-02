using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using FlockService.Data;
using FlockService.SyncDataServices.Grpc;
using System.Net.Http;
using FlockService.Controllers;
using Dapr.Client;

namespace FlockService
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        private readonly IWebHostEnvironment _env;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddDbContext<FlockContext>(opt => opt.UseInMemoryDatabase("InMem"));
           services.AddDbContext<FlockContext>(opt =>opt.UseSqlServer
                //(Configuration.GetConnectionString("FlockConnectionLokaal")));
                    (Configuration.GetConnectionString("FlockConnectionDocker"))); 

            //services.AddScoped<IFlockRepo, MockFlockRepo>();
            services.AddScoped<IFlockRepo, SqlFlockRepo>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IEggTypeDataClient, EggTypeDataClient>();

            services.AddHttpClient();
            services.AddControllers().AddNewtonsoftJson(s =>
            {
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCloudEvents();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapSubscribeHandler();    // hierdoor weet Dapr welke endpoints geannoteerd zijn (gesubscribed)
                endpoints.MapControllers();
            });

            PrepDb.PrepPopulation(app, _env.IsProduction());
        }
    }
}
