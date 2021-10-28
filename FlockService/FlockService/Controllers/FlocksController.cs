using AutoMapper;
using FlockService.Data;
using FlockService.Dtos;
using FlockService.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlockService.Controllers
{

    // Les Jackson gebruikt "api/c/platforms/{platform{id}/[controller]" als top route.
    // Omdat voorliggend project van een andere tutorial komt (en bovendien aangepast is), heb ik er "api" van gemaakt
    // en de methoden voorzien van de extra paths. Hierdoor kunnen zowel de oude als de 
    // nieuwe endpoints gebruikt worden.

    [ApiController] // zorgt er o.a. ook voor dat applicatie de ingevoerde json checkt op benodigde velden
    [Route("api")]
    public class FlocksController : ControllerBase
    {
        private readonly IFlockRepo _repository;
        private readonly IMapper _mapper;

        public FlocksController(IFlockRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // Nieuwe endpoints

        [HttpGet]
        [Route("f/eggtypes/{eggTypeId}/flocks")]   // /api/c/...etc
        public ActionResult<IEnumerable<FlockReadDto>> GetFlocksForEggType(int eggTypeId)
        {
            Console.WriteLine($"--> Hit GetFlocksForEggType: {eggTypeId}");
            if (!_repository.EggTypeExists(eggTypeId))
            {
                return NotFound();
            }

            var flockItems = _repository.GetFlocksForEggType(eggTypeId);
            return Ok(_mapper.Map<IEnumerable<FlockReadDto>>(flockItems));
        }

        // route -> /api/f/eggtypes/{eggTypeId}/flocks/{flockId}
        [HttpGet("f/eggtypes/{eggTypeId}/flocks/{flockId}", Name = "GetFlockForEggType")]
        public ActionResult<FlockReadDto> GetFlockForEggType(int eggTypeId, int flockId)
        {
            Console.WriteLine($"--> Hit GetFlockssForEggType: {eggTypeId} / {flockId}");
            if (!_repository.EggTypeExists(eggTypeId))
            {
                return NotFound();
            }

            var flockItem = _repository.GetFlock(eggTypeId, flockId);

            if (flockItem == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<FlockReadDto>(flockItem));
        }

        [HttpPost]
        [Route("f/eggtypes/{eggTypeId}/flocks")]
        public ActionResult<FlockReadDto> CreateFlockForEggType(int eggTypeId, FlockCreateDto flockDto)
        {
            Console.WriteLine($"--> Hit CreateFlockForEggType: {eggTypeId}");
            if (!_repository.EggTypeExists(eggTypeId))
            {
                return NotFound();
            }

            //dirty Fix om fk key error te voorkomen
            FlockCreateWithFKDto flockCreateWithFKDto = new FlockCreateWithFKDto();
            flockCreateWithFKDto.Description = flockDto.Description;
            flockCreateWithFKDto.FlockCode = flockDto.FlockCode;
            flockCreateWithFKDto.EggTypeId = eggTypeId;

            var flock = _mapper.Map<Flock>(flockCreateWithFKDto); //geen foutchecks nodig, doet [ApiController] al

            _repository.CreateFlock(flock);
            _repository.SaveChanges();

            //we willen een URI returnen naar het nieuw aangemaakte command, dus:
            var flockReadDto = _mapper.Map<FlockReadDto>(flock); // door SaveChanges() heeft het command object hier een Id

            return CreatedAtRoute(nameof(GetFlockForEggType),
                new { eggTypeId = eggTypeId, flockId = flockReadDto.Id }, flockReadDto);
        }


        //  Endpoints van oude tutorial

        [HttpGet]
        [Route("flocks")]   // /api/flocks
        public ActionResult<IEnumerable<FlockReadDto>> GetAllFlocks()
        {
            var flockItems = _repository.GetAllFlocks();
            return Ok(_mapper.Map<IEnumerable<FlockReadDto>>(flockItems));
        }

        [HttpGet]
        [Route("flocks/{id}", Name = "GetFlockById")] // /api/flocks/{id} // Name is nodig voor CreatedAtRoute()
        public ActionResult<FlockReadDto> GetFlockById(int id)
        {
            var flockItem = _repository.GetFlockById(id);
            if (flockItem != null)
            {
                return Ok(_mapper.Map<FlockReadDto>(flockItem));
            }

            return NotFound();
        }

        [Route("flocks")]   // /api/flocks
        [HttpPost]
        public ActionResult<FlockReadDto> CreateFlock(FlockCreateWithFKDto flockCreateDto) 
        {
            var flockModel = _mapper.Map<Flock>(flockCreateDto);
            _repository.CreateFlock(flockModel);
            _repository.SaveChanges();

            var flockReadDto = _mapper.Map<FlockReadDto>(flockModel);

            return CreatedAtRoute(nameof(GetFlockById), new {Id = flockReadDto.Id }, flockReadDto);
            // nu wordt er een Location (https://localhost:44351/api/flocks/5) in de header meegestuurd bij de POST en een 201 status


            //return Ok(flockReadDto);

        }

        [Route("flocks/{id}")]   // /api/flocks/id
        [HttpPut]
        public ActionResult UpdateFlock(int id, FlockUpdateDto flockUpdateDto)
        {
            var flockModelFromRepo = _repository.GetFlockById(id);
            if (flockModelFromRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(flockUpdateDto, flockModelFromRepo);    // changes are tracked by dbContext

            _repository.UpdateFlock(flockModelFromRepo);        // dus dit hoeft niet per se, maar is good practice (voor als je een andere implementatie hebt)
            //deze methode is ook helemaal niet implemented in sqlrepo.

            _repository.SaveChanges();

            return NoContent();
        }

        //PATCH api/flocks/{id}


        //patches hebben dit formaat als Json body:

        /*        [
                    {
                     "op" : "replace",
                     "path" : "/flockcode",
                     "value" : "nieuwe waarde"
                     },
                     {
                     "op" : "replace",
                     "path" : "/description",
                     "value" : "nieuwe waardeLine"
                     }
                ]

                of een enkele:

                 [
                    {
                     "op" : "replace",
                     "path" : "/description",
                     "value" : "nieuwe waarde"
                     }
                ]
         */

        [Route("flocks/{id}")]   // /api/flocks
        [HttpPatch]
        public ActionResult PartialFlockUpdate(int id, JsonPatchDocument<FlockUpdateDto> patchDoc)
        {
            var flockModelFromRepo = _repository.GetFlockById(id);
            if (flockModelFromRepo == null)
            {
                return NotFound();
            }

            var flockToPatch = _mapper.Map<FlockUpdateDto>(flockModelFromRepo);
            patchDoc.ApplyTo(flockToPatch, ModelState);
            
            if (!TryValidateModel(flockToPatch)) 
            {
                return ValidationProblem(ModelState);
            }


            //hieronder = hetzelfde als in PUT method/endpoint

            _mapper.Map(flockToPatch, flockModelFromRepo);    // changes are tracked by dbContext

            _repository.UpdateFlock(flockModelFromRepo);        // dus dit hoeft niet per se, maar is good practice (voor als je een andere implementatie hebt)
            //deze methode is ook helemaal niet implemented in sqlrepo.

            _repository.SaveChanges();

            return NoContent();

        }


        //DELETE api/flocks/{id}

        [Route("flocks/{id}")]   // /api/flocks
        [HttpDelete]
        public ActionResult DeleteFlock(int id)
        {
            var flockModelFromRepo = _repository.GetFlockById(id);
            if (flockModelFromRepo == null)
            {
                return NotFound();
            }


            _repository.DeleteFlock(flockModelFromRepo);
            _repository.SaveChanges();

            return NoContent();
        }

    }
}
