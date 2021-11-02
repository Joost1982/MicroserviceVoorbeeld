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
using Dapr.Client;
using Dapr;

namespace EggTypeService.Controllers
{
    [Route("api/eggtypes")] // /api/eggtypes
    [ApiController] // hierdoor komt er een hele hoop out of the box (bijvoorbeeld json in body interpreteren)
    public class EggTypesController : ControllerBase
    {
        private readonly IEggTypeRepo _repository;
        private readonly IMapper _mapper;
        private readonly IFlockDataClient _commandDataClient;
        private readonly DaprClient _dapprClient;
        private const string STATESTORE_NAME = "eggstatestore"; // voor Dapr statemanagement voorbeeld

        public EggTypesController(
            IEggTypeRepo repository, 
            IMapper mapper,
            IFlockDataClient flockDataClient,
            DaprClient daprClient)
        {
            _repository = repository;
            _mapper = mapper;
            _commandDataClient = flockDataClient;
            _dapprClient = daprClient;  // voor state management test
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
        public async Task<ActionResult<EggTypeReadDto>> CreateEggType(EggTypeCreateDto eggTypeCreateDto, 
            [FromServices] DaprClient daprClient)
        {

            var eggTypeModel = _mapper.Map<EggType>(eggTypeCreateDto);
            _repository.CreateEggType(eggTypeModel);
            _repository.SaveChanges();

            //om een 201 te returnen en een locatie van de nieuwe entry

            var eggTypeReadDto = _mapper.Map<EggTypeReadDto>(eggTypeModel);

            
            // send Async message (van microservice naar messagebus)

            try
            {
                var eggTypePublishedDto = _mapper.Map<EggTypePublishedDto>(eggTypeReadDto);
                eggTypePublishedDto.Event = "EggType_Published";

                //nu met dapr:
                Console.WriteLine("--> Debuuug: publish message with Dapr");
                await daprClient.PublishEventAsync("pubsub", "trigger", eggTypePublishedDto);
                Console.WriteLine("--> **** dapr klaar?");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
                Console.WriteLine($"--> InnerException: {ex.InnerException}");
            }


            // voor Dapr statemanagement (bijhouden hoe vaak er gePOST is) nb: zonder enige redis code! Dapr doet alles. En state store is eenvoudig te switchen
            int aantal = await _dapprClient.GetStateAsync<int>(STATESTORE_NAME, "bij");
            aantal += 1;
            await _dapprClient.SaveStateAsync(STATESTORE_NAME, "bij", aantal);


            return CreatedAtRoute(nameof(GetEggTypeById), new { Id = eggTypeReadDto.Id }, eggTypeReadDto);
                //CreatedAtRoute returned een 201 en een locatie (een route)
        }



        // endpoint mbt Dapr State Management test (via /api/state/bij kun je zien hoevaak er een eggtype gePOST is)
        [HttpGet]
        [Route("state/bij")]
        public async Task<int> GetState()
        {
            int aantal = await _dapprClient.GetStateAsync<int>(STATESTORE_NAME, "bij");
            return aantal;
        }


    }
}
