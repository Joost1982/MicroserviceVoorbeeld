using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EggTypeService.Dtos
{
    public class EggTypeReadDto
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public int EggTypeGroupParameterCode { get; set; }

        public int EggColorParameterCode { get; set; }
    }
}
