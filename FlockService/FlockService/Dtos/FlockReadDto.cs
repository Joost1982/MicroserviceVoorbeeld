using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlockService.Dtos
{
    public class FlockReadDto
    {
        public int Id { get; set; }
        public string FlockCode { get; set; }
        public string Description { get; set; }
        public int EggTypeId { get; set; }
    }
}
