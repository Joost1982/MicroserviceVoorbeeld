using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using EggTypeService.Data;
using EggTypeService.SyncDataServices.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json.Serialization;
using EggTypeService.SyncDataServices.Grpc;

namespace EggTypeService
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
			///////// tell mongodb driver how to serialize types

            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String)); //guid wordt string
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String)); // idem
			
            Console.WriteLine("--> using Mongodb");
			services.AddSingleton<IMongoClient>(serviceProvider =>
			{
				return new MongoClient(Configuration.GetConnectionString("MongoDbConnection"));
			}); // overal waar "IMongoClient" gevraagd wordt, injecteert container concrete "MongoClient" implementatie (met additionele info)


			services.AddScoped<IEggTypeRepo, MongoEggTypeRepo>();
            
            services.AddGrpc();

            services.AddHttpClient<IFlockDataClient, HttpFlockDataClient>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //dapr (endpoints voor de sidecar)
            services.AddDaprClient(builder => builder
                .UseHttpEndpoint($"http://eggtypeservice-api-dapr:3600")
                .UseGrpcEndpoint($"http://eggtypeservice-api-dapr:60000"));

            services.AddControllers();
			services.AddControllers().AddNewtonsoftJson(s =>
            {
                s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EggTypeService", Version = "v1" });
            });

            Console.WriteLine($"--> Flock Service endpoint: {Configuration["FlockService"]}");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EggTypeService v1"));
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGrpcService<GrpcEggTypeService>();

            });

            PrepDb.PrepPopulation(app, _env.IsProduction());
        }
    }
}
