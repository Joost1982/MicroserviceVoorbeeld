using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Dtos
{
    public class EggTypePublishedDto
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public string Event { get; set; }
    }
}
