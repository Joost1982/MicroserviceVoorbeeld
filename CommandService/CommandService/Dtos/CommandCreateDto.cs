using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Commander.Dtos
{
    public class CommandCreateDto   // annotaties zodat de client een 400 bad request error krijgt als die een veld vergeet (anders krijgt die een 500 internal server error)
    {
        [Required]
        [MaxLength(250)]
        public string HowTo { get; set; }
        [Required]
        public string Line { get; set; }
        
        //[Required]
        //public int PlatformId { get; set; }   // niet nodig, want komt uit de url!
        
    }
}
