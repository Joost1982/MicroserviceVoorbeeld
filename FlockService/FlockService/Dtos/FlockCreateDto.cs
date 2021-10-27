using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlockService.Dtos
{
    public class FlockCreateDto   // annotaties zodat de client een 400 bad request error krijgt als die een veld vergeet (anders krijgt die een 500 internal server error)
    {
        [Required]
        [MaxLength(250)]
        public string FlockCode { get; set; }
        [Required]
        public string Description { get; set; }
        
        //[Required]
        //public int EggTypeId { get; set; }   // niet nodig, want komt uit de url!
        
    }
}
