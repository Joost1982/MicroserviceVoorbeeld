using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EggTypeService.Models
{
    public class EggType
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int EggTypeGroupParameterCode { get; set; }
        
        [Required]
        public int EggColorParameterCode { get; set; }
    }
}
