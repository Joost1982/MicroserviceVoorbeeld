using AutoMapper;
using FlockService.Data;
using FlockService.Dtos;
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
        public ActionResult<IEnumerable<EggTypeReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting EggTypes from Flock Service");
            var eggTypeItems = _repository.GetAllEggTypes();

            return Ok(_mapper.Map<IEnumerable<EggTypeReadDto>>(eggTypeItems));

        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine(" --> binnenkomende POST (Flock Service)");
            return Ok("Inbound test from EggTypes Controller (from Flock Service)");
        }
    }
}
