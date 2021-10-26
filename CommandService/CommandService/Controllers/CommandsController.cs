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

    [ApiController]
    [Route("/api/commands")]
    public class CommandsController : ControllerBase
    {
        private readonly ICommanderRepo _repository;
        private readonly IMapper _mapper;

        public CommandsController(ICommanderRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        
        [HttpGet] // /api/commands
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands()
        {
            var commandItems = _repository.GetAllCommands(); 
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
        }


        [HttpGet]
        [Route("{id}", Name = "GetCommandById")] // /api/commands/{id} // Name is nodig voor CreatedAtRoute()
        public ActionResult<CommandReadDto> GetCommandById(int id)
        {
            var commandItem = _repository.GetCommandById(id);
            if (commandItem != null)
            {
                return Ok(_mapper.Map<CommandReadDto>(commandItem));
            }

            return NotFound();
        }

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
