using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlockService.Models;

namespace FlockService.Data
{
    public interface IFlockRepo
    {

        bool SaveChanges();

        // EggTypes
        IEnumerable<EggType> GetAllEggTypes();
        void CreateEggType(EggType eggType);
        bool EggTypeExists(int eggTypeId);
        
        
        // Flocks (van andere tutorial)
        IEnumerable<Flock> GetAllFlocks();
        Flock GetFlockById(int id);
        void CreateFlock(Flock cmd);
        void UpdateFlock(Flock cmd);
        void DeleteFlock(Flock cmd);
       
        // Combinatie
        IEnumerable<Flock> GetFlocksForEggType(int eggTypeId);
        Flock GetFlock(int eggTypeId, int flockId);
        void CreateFlock(int eggTypeId, Flock flock);

    }
}
