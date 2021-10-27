using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlockService.Models
{
    public class Flock
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FlockCode { get; set; }

        [Required]
        [MaxLength(250)]
        public string Description { get; set; }
       
        [Required]
        public int EggTypeId { get; set; }

        public EggType EggType;
    }
}
