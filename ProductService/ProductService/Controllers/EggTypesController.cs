using AutoMapper;
using ProductService.Data;
using ProductService.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapr;
using ProductService.Models;

namespace ProductService.Controllers
{
   [ApiController]
    public class EggTypesController : ControllerBase
    {
        private readonly IProductRepo _repository;
        private readonly IMapper _mapper;

        public EggTypesController(IProductRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [Route("api/p/eggtypes")]    //  inclusief /p om hem te onderscheiden van de EggTypesService controller in de andere services
        [HttpGet]
        public ActionResult<IEnumerable<EggTypeReadDto>> GetEggTypes()
        {
            Console.WriteLine("--> Getting EggTypes from Product Service");
            var eggTypeItems = _repository.GetAllEggTypes();

            return Ok(_mapper.Map<IEnumerable<EggTypeReadDto>>(eggTypeItems));
        }

        //[HttpPost]
        //public ActionResult TestInboundConnection()
        //{
        //    Console.WriteLine(" --> binnenkomende POST (Product Service)");
        //    return Ok("Inbound test from EggTypes Controller (from Product Service)");
        //}

        [Topic("pubsub", "trigger")]        // door deze annotatie is dit endpoint geregistreerd als subscriber (elke keer als er een message is wordt de method uitgevoerd)
        [Route("api/p/eggtypes")]
        [HttpPost]
        public ActionResult AddEggType(EggTypePublishedDto eggTypePublishedDto)     
        // via "app.UseCloudEvents()" in Startup wordt er (via middleware) de payload vanuit de messagebus gehaald -> eggTypePublishedDto
        {
            Console.WriteLine(" --> binnenkomende POST vanuit EggType Service via Message Bus (Product Service)");

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
