using Microsoft.Extensions.Configuration;
using EggTypeService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EggTypeService.SyncDataServices.Http
{
    public class HttpFlockDataClient : IFlockDataClient
    {
        private readonly HttpClient _httpclient;
        private readonly IConfiguration _configuration;

        public HttpFlockDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpclient = httpClient;
            _configuration = configuration;
        }
        
        public async Task SendEggTypeToFlock(EggTypeReadDto eggType)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(eggType),
                Encoding.UTF8,
                "application/json"
                );

            Console.WriteLine("debuuuuuuug: " + _configuration["FlockService"]);
            var response = await _httpclient.PostAsync($"{_configuration["FlockService"]}", httpContent);

            if (response.IsSuccessStatusCode) 
            { 
                Console.WriteLine("--> Sync POST to Flock Service Ok"); 
            } else
            {
                Console.WriteLine("--> Sync POST to Flock Service not Ok"); 
            }
        }
    }
}
