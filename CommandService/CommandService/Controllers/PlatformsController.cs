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
        public PlatformsController()
        {

        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine(" --> binnenkomende POST # CommandService");
            return Ok("Inbound test from Platforms Controller");
        }
    }
}
