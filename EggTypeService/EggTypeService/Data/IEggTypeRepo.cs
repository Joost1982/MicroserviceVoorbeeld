using EggTypeService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EggTypeService.Data
{
    public interface IEggTypeRepo
    {
        bool SaveChanges();

        IEnumerable<EggType> GetAllEggTypes();

        EggType GetEggTypeById(int id);

        void CreateEggType(EggType eggtype);

    }
}
