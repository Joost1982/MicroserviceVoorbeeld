using Dapr.Client;
using FlockService.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

// Dapr building block: Service invocation test -> vanuit voorliggende Flock Service naar de Product Service
// voordelen van invocation via Dapr is o.a. dat je de URI van de andere microservice niet hoeft te weten: je benaderd je sidecar en die doet de rest.
// (je moet alleen wel de naam van die andere service weten, dus heel veel voordeel t.o.v. de routing in nginx is er niet?)
// ook kan je telemetry gegevens krijgen, dat is wel handig.

namespace FlockService.Controllers
{
    [Route("api/f/products")]       
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly HttpClient _httpClient1;
        private readonly HttpClient _httpClient2;

        public ProductsController(IHttpClientFactory httpClientfact)
        {
            //voor optie 1
            _httpClient1 = httpClientfact.CreateClient();

            //voor optie 2
            _httpClient2 = DaprClient.CreateInvokeHttpClient(
                    "productservice-api", "http://localhost:3600");   // naam van service waarvan we iets willen invoken + eigen sidecar
        }



        [Route("{id}")]
        public async Task<Product> GetProductByIdDapr(int id)
        {
            //met Dapr (call naar eigen dapr sidecar (uri van andere service is dus niet nodig om te weten, alleen de naam), de sidecar zoekt het endpoint voor je op)
            
            //mogelijkheid 1
            //return await _httpClient1.GetFromJsonAsync<Product>($"http://localhost:3600/v1.0/invoke/productservice-api/method/api/products/{id}");

            //mogelijkheid 2  // vanwege goede Dapr-HttpClient() integratie (zie constructor) kan het korter en Dapr maakt er zelf die lange URI van van hierboven
            return await _httpClient2.GetFromJsonAsync<Product>($"/api/products/{id}");
        }


        //in Dapr workshop: https://www.youtube.com/watch?v=0y7ne6teHT4&t=12803s doen ze bij mogelijkheid 2 wat ik in de constructor heb in Startup.cs, maar dat gaf errors bij mij

    }
}
