using AutoMapper;
using EggTypeService;
using FlockService.Models;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FlockService.SyncDataServices.Grpc
{
    public class EggTypeDataClient : IEggTypeDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public EggTypeDataClient(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public IEnumerable<EggType> ReturnAllEggTypes()
        {
            Console.WriteLine($"--> Calling GRPC Service {_configuration["GrpcEggType"]}");

            //afwijking tov Les Jackson ivm gRPC die SSL nodig heeft en dat krijg ik niet voor elkaar.
            //daarom stel ik in dat gRPC ook met invalid ssl certs kan
            //zie verder: https://docs.microsoft.com/en-us/aspnet/core/grpc/troubleshoot?view=aspnetcore-5.0

            var httpHandler = new HttpClientHandler();
            // Return `true` to allow certificates that are untrusted/invalid
            httpHandler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var channel = GrpcChannel.ForAddress(_configuration["GrpcEggType"], new GrpcChannelOptions { HttpHandler = httpHandler });
            
            
            var client = new GrpcEggType.GrpcEggTypeClient(channel);
            var request = new GetAllRequest();

            try
            {
                var reply = client.GetAllEggTypes(request);
                return _mapper.Map<IEnumerable<EggType>>(reply.Eggtype);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Couldnot call GRPC Server {ex.Message}");
                return null;
            }
        }
    }
}
