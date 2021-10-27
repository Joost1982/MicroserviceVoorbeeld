using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using EggTypeService.Data;
using EggTypeService.Dtos;
using EggTypeService.Models;
using EggTypeService.SyncDataServices.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EggTypeService.Controllers
{
    [Route("api/eggtypes")] // /api/eggtypes
    [ApiController] // hierdoor komt er een hele hoop out of the box (bijvoorbeeld json in body interpreteren)
    public class EggTypesController : ControllerBase
    {
        private readonly IEggTypeRepo _repository;
        private readonly IMapper _mapper;
        private readonly IFlockDataClient _commandDataClient;

        public EggTypesController(
            IEggTypeRepo repository, 
            IMapper mapper,
            IFlockDataClient commandDataClient)
        {
            _repository = repository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<EggTypeReadDto>> GetEggTypes()
        {
            var eggtypes = _repository.GetAllEggTypes();
            return Ok(_mapper.Map<IEnumerable<EggTypeReadDto>>(eggtypes));
        }

        [HttpGet("{id}", Name = "GetEggTypeById")]
        public ActionResult<EggTypeReadDto> GetEggTypeById(int id)
        {
            var eggTypeItem = _repository.GetEggTypeById(id);
            if (eggTypeItem == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<EggTypeReadDto>(eggTypeItem));
        }

        [HttpPost]
        public async Task<ActionResult<EggTypeReadDto>> CreatePlatform(EggTypeCreateDto eggTypeCreateDto)
        {

            var eggTypeModel = _mapper.Map<EggType>(eggTypeCreateDto);
            _repository.CreateEggType(eggTypeModel);
            _repository.SaveChanges();

            //om een 201 te returnen en een locatie van de nieuwe entry

            var eggTypeReadDto = _mapper.Map<EggTypeReadDto>(eggTypeModel);

            try
            {
                await _commandDataClient.SendEggTypeToFlock(eggTypeReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetEggTypeById), new { Id = eggTypeReadDto.Id }, eggTypeReadDto);
                //CreatedAtRoute returned een 201 en een locatie (een route)
        }

    }
}
