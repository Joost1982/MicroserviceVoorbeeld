using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Dtos
{
    public class EggTypeReadDto
    {
        public int Id { get; set; }
        public int ExternalId { get; set; }
        public string Description { get; set; }
    }
}
