using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Dtos
{
    public class ProductCreateWithFKDto
    {
        [Required]
        [MaxLength(20)]
        public string ProductCode { get; set; }
        [Required]
        public bool isActive { get; set; }

        [Required]
        public int EggTypeId { get; set; }          
        //bij endpoint POST /api/products is er geen Id, dus die moet bij gebruik van dit endpoint wel worden meegegeven!
        //(anders faalt de insert vanwege de Fk constraint) <-- klopt niet want InMem db, maar bij MS-SQL wel, dus laat het staan
    }
}
