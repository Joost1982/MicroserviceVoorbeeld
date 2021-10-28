using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Dtos
{
    public class ProductUpdateDto
    {
        [Required]
        public string ProductCode { get; set; }

        [Required]
        public bool isActive { get; set; }

        [Required]
        public string EggType { get; set; }
    }
}
