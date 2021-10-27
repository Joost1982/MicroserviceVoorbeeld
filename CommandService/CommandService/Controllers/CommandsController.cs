using AutoMapper;
using Commander.Data;
using Commander.Dtos;
using Commander.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Commander.Controllers
{

    // Les Jackson gebruikt "api/c/platforms/{platform{id}/[controller]" als top route.
    // Omdat voorliggend project van een andere tutorial komt, heb ik er "api" van gemaakt
    // en de methoden voorzien van de extra paths. Hierdoor kunnen zowel de oude als de 
    // nieuwe endpoints gebruikt worden.

    [ApiController] // zorgt er o.a. ook voor dat applicatie de ingevoerde json checkt op benodigde velden
    [Route("api")]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepo _repository;
        private readonly IMapper _mapper;

        public CommandsController(ICommandRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // Nieuwe endpoints

        [HttpGet]
        [Route("c/platforms/{platformId}/[controller]")]   // /api/c/...etc
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"--> Hit GetCommandsForPlatform: {platformId}");
            if (!_repository.PlatformExists(platformId))
            {
                return NotFound();
            }

            var commandItems = _repository.GetCommandsForPlatform(platformId);
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }

        // route -> /api/c/platforms/{plaformId}/commands/{commandId}
        [HttpGet("c/platforms/{platformId}/[controller]/{commandId}", Name = "GetCommandForPlatform")]
        public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine($"--> Hit GetCommandsForPlatform: {platformId} / {commandId}");
            if (!_repository.PlatformExists(platformId))
            {
                return NotFound();
            }

            var commandItem = _repository.GetCommand(platformId, commandId);

            if (commandItem == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CommandReadDto>(commandItem));
        }

        [HttpPost]
        [Route("c/platforms/{platformId}/[controller]")]
        public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandDto)
        {
            Console.WriteLine($"--> Hit CreateCommandForPlatform: {platformId}");
            if (!_repository.PlatformExists(platformId))
            {
                return NotFound();
            }

            var command = _mapper.Map<Command>(commandDto); //geen foutchecks nodig, doet [ApiController] al

            _repository.CreateCommand(command);
            _repository.SaveChanges();

            //we willen een URI returnen naar het nieuw aangemaakte command, dus:
            var commandReadDto = _mapper.Map<CommandReadDto>(command); // door SaveChanges() heeft het command object hier een Id

            return CreatedAtRoute(nameof(GetCommandForPlatform),
                new { platformId = platformId, commandId = commandReadDto.Id }, commandReadDto);
        }


        //  Endpoints van oude tutorial

        [HttpGet]
        [Route("commands")]   // /api/commands
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands()
        {
            var commandItems = _repository.GetAllCommands();
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }

        [HttpGet]
        [Route("commands/{id}", Name = "GetCommandById")] // /api/commands/{id} // Name is nodig voor CreatedAtRoute()
        public ActionResult<CommandReadDto> GetCommandById(int id)
        {
            var commandItem = _repository.GetCommandById(id);
            if (commandItem != null)
            {
                return Ok(_mapper.Map<CommandReadDto>(commandItem));
            }

            return NotFound();
        }

        [Route("commands")]   // /api/commands
        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto commandCreateDto) 
        {
            var commandModel = _mapper.Map<Command>(commandCreateDto);
            _repository.CreateCommand(commandModel);
            _repository.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);

            return CreatedAtRoute(nameof(GetCommandById), new {Id = commandReadDto.Id }, commandReadDto);
            // nu wordt er een Location (https://localhost:44351/api/Commands/5) in de header meegestuurd bij de POST en een 201 status


            //return Ok(commandReadDto);

        }

        [Route("commands")]   // /api/commands
        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, CommandUpdateDto commandUpdateDto)
        {
            var commandModelFromRepo = _repository.GetCommandById(id);
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(commandUpdateDto, commandModelFromRepo);    // changes are tracked by dbContext

            _repository.UpdateCommand(commandModelFromRepo);        // dus dit hoeft niet per se, maar is good practice (voor als je een andere implementatie hebt)
            //deze methode is ook helemaal niet implemented in sqlrepo.

            _repository.SaveChanges();

            return NoContent();
        }

        //PATCH api/commands/{id}


        //patches hebben dit formaat als Json body:

        /*        [
                    {
                     "op" : "replace",
                     "path" : "/howto",
                     "value" : "nieuwe waarde"
                     },
                     {
                     "op" : "replace",
                     "path" : "/line",
                     "value" : "nieuwe waardeLine"
                     }
                ]

                of een enkele:

                 [
                    {
                     "op" : "replace",
                     "path" : "/howto",
                     "value" : "nieuwe waarde"
                     }
                ]
         */

        [Route("commands")]   // /api/commands
        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchDoc)
        {
            var commandModelFromRepo = _repository.GetCommandById(id);
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }

            var commandToPatch = _mapper.Map<CommandUpdateDto>(commandModelFromRepo);
            patchDoc.ApplyTo(commandToPatch, ModelState);
            
            if (!TryValidateModel(commandToPatch)) 
            {
                return ValidationProblem(ModelState);
            }


            //hieronder = hetzelfde als in PUT method/endpoint

            _mapper.Map(commandToPatch, commandModelFromRepo);    // changes are tracked by dbContext

            _repository.UpdateCommand(commandModelFromRepo);        // dus dit hoeft niet per se, maar is good practice (voor als je een andere implementatie hebt)
            //deze methode is ook helemaal niet implemented in sqlrepo.

            _repository.SaveChanges();

            return NoContent();

        }


        //DELETE api/command/{id}

        [Route("commands")]   // /api/commands
        [HttpDelete("{id}")]
        public ActionResult DeleteCommand(int id)
        {
            var commandModelFromRepo = _repository.GetCommandById(id);
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }


            _repository.DeleteCommand(commandModelFromRepo);
            _repository.SaveChanges();

            return NoContent();
        }

    }
}
