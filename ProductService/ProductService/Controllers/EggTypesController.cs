using AutoMapper;
using ProductService.Data;
using ProductService.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Controllers
{
   [Route("api/p/eggtypes")]    //  inclusief /p om hem te onderscheiden van de EggTypesService controller in de andere services
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

        [HttpGet]
        public ActionResult<IEnumerable<EggTypeReadDto>> GetEggTypes()
        {
            Console.WriteLine("--> Getting EggTypes from Product Service");
            var eggTypeItems = _repository.GetAllEggTypes();

            return Ok(_mapper.Map<IEnumerable<EggTypeReadDto>>(eggTypeItems));

        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine(" --> binnenkomende POST (Product Service)");
            return Ok("Inbound test from EggTypes Controller (from Product Service)");
        }
    }
}
