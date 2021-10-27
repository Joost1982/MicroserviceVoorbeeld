using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlockService.Dtos
{
    public class FlockUpdateDto
    {
        [Required]
        public string FlockCode { get; set; }

        [Required]
        [MaxLength(250)]
        public string Description { get; set; }

        [Required]
        public string EggType { get; set; }
    }
}
