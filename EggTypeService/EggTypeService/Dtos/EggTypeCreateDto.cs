using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EggTypeService.Dtos
{
    public class EggTypeCreateDto
    {

        [Required]
        public string Description { get; set; }

        [Required]
        public int EggTypeGroupParameterCode { get; set; }

        [Required]
        public int EggColorParameterCode { get; set; }
    }
}
