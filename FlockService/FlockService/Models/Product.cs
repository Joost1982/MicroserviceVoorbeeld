using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlockService.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductCode { get; set; }
        public bool isActive { get; set; }
        public int EggTypeId { get; set; }
    }
}
