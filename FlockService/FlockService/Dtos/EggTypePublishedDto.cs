using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlockService.Dtos
{
    public class EggTypePublishedDto
    {
        public int Id { get; set; }       
        public string Description { get; set; }

        public string Event { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Description: {Description}, Event: {Event}";
        }

    }
}
