using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string ProductCode { get; set; }

        [Required]
        public bool isActive { get; set; }
       
        [Required]
        public int EggTypeId { get; set; }

        public EggType EggType;
    }
}
