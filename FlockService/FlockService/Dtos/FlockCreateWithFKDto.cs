using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlockService.Dtos
{
    public class FlockCreateWithFKDto
    {
        [Required]
        [MaxLength(250)]
        public string FlockCode { get; set; }
        [Required]
        public string Description { get; set; }

        //[Required]
        public int EggTypeId { get; set; }   
        //bij endpoint POST /api/flocks is er geen Id, dus die moet bij gebruik van dit endpoint wel worden meegegeven!
        //(anders faalt de insert vanwege de Fk constraint)
    }
}
