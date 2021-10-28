using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Dtos
{
    public class ProductCreateDto   // annotaties zodat de client een 400 bad request error krijgt als die een veld vergeet (anders krijgt die een 500 internal server error)
    {
        [Required]
        [MaxLength(20)]
        public string ProductCode { get; set; }
        [Required]
        public bool isActive { get; set; }
        
        //[Required]
        //public int EggTypeId { get; set; }   // niet nodig, want komt uit de url!
        
    }
}
