using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Data;
using ProductService.Dtos;
using ProductService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("--> Determining Event");
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
            switch (eventType.Event)
            {
                case "EggType_Published":
                        Console.WriteLine("--> EggType Published Event detected");
                    return EventType.EggTypePublished;
                default:
                    Console.WriteLine("--> Could not determine the event type");
                    return EventType.Undetermined;
            }
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.EggTypePublished:
                    //to do
                    break;
                default:
                    break;
            }
        }

        private void addEggType(string eggTypePublishedMessage)
        {
            //get reference to repo (kan niet via constructor injection ivm met verschil in levensduur van de objecten
            //hieronder wordt die reference op een andere manier opgehaald
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IProductRepo>();

                var eggTypePublishedDto = JsonSerializer.Deserialize<EggTypePublishedDto>(eggTypePublishedMessage);

                try
                {
                    var eggType = _mapper.Map<EggType>(eggTypePublishedDto);
                    if (!repo.EggTypeExists(eggType.ExternalId))
                    {
                        repo.CreateEggType(eggType);
                        repo.SaveChanges();
                        Console.WriteLine($"--> added eggtype to dabase: {eggType.Description}");
                    }
                    else
                    {
                        Console.WriteLine($"--> eggtype already exists, not added to db: {eggType.Description}");
                    }


                }
                catch (Exception e)
                {
                    Console.WriteLine($"--> Could not add eggType to db: {e}");
                }
            
            }
        }
    }

    enum EventType
    {
        EggTypePublished,
        Undetermined
    }

}
