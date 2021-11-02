using AutoMapper;
using Dapr;
using FlockService.Data;
using FlockService.Dtos;
using FlockService.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlockService.Controllers
{
   [Route("api/f/eggtypes")]    //  inclusief /f om hem te onderscheiden van de EggTypesService controller in de andere service
   [ApiController]
    public class EggTypesController : ControllerBase
    {
        private readonly IFlockRepo _repository;
        private readonly IMapper _mapper;

        public EggTypesController(IFlockRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;

        }

        [HttpGet]
        public ActionResult<IEnumerable<EggTypeReadDto>> GetEggTypes()
        {
            Console.WriteLine("--> Getting EggTypes from Flock Service");
            var eggTypeItems = _repository.GetAllEggTypes();

            return Ok(_mapper.Map<IEnumerable<EggTypeReadDto>>(eggTypeItems));

        }

        //[HttpPost]
        //public ActionResult TestInboundConnection()
        //{
        //    Console.WriteLine(" --> binnenkomende POST (Flock Service)");
        //    return Ok("Inbound test from EggTypes Controller (from Flock Service)");
        //}

        [HttpPost]
        [Topic("pubsub", "trigger")]        // door deze annotatie is dit endpoint geregistreerd als subscriber (elke keer als er een message is wordt de method uitgevoerd)
        public ActionResult AddEggType(EggTypePublishedDto eggTypePublishedDto)
        // via "app.UseCloudEvents()" in Startup wordt er (via middleware) de payload vanuit de messagebus gehaald -> eggTypePublishedDto
        {
            Console.WriteLine(" --> binnenkomende POST vanuit EggType Service via Message Bus (Flock Service)");

            try
            {
                var eggType = _mapper.Map<EggType>(eggTypePublishedDto);
                if (!_repository.EggTypeExists(eggType.ExternalId))
                {
                    _repository.CreateEggType(eggType);
                    _repository.SaveChanges();
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

            return Ok();
        }
    }
}
