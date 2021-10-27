using AutoMapper;
using Commander.Data;
using CommandService.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandService.Controllers
{
   [Route("api/c/[controller]")]    //  inclusief /c om hem te onderscheiden van de PlatformsService controller in de andere service
   [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;

        public PlatformsController(ICommandRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;

        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms from Command Service");
            var platformItems = _repository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));

        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine(" --> binnenkomende POST # CommandService");
            return Ok("Inbound test from Platforms Controller");
        }
    }
}
