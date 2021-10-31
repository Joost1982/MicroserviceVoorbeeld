using FlockService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// nb: niet alle methods van Interface geimplementeerd!
namespace FlockService.Data
{
    public class MockFlockRepo : IFlockRepo
    {
        public void CreateFlock(Flock flock)
        {
            throw new NotImplementedException();
        }

        public void CreateFlock(int eggTypeId, Flock flock)
        {
            throw new NotImplementedException();
        }

        public void CreateEggType(EggType eggType)
        {
            throw new NotImplementedException();
        }

        public void DeleteFlock(Flock flock)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Flock> GetAllFlocks()
        {
            var flocks = new List<Flock>()
            {
                new Flock{ Id=1, FlockCode="1111-22", Description="Bennekom stal", EggTypeId=1},
                new Flock{ Id=2, FlockCode="1111-23", Description="Bennekom stal2", EggTypeId=2},
                new Flock{ Id=3, FlockCode="1111-25", Description="Bennekom stal3", EggTypeId=11}
            };

            return flocks;
        }

        public IEnumerable<EggType> GetAllEggTypes()
        {
            throw new NotImplementedException();
        }

        public Flock GetFlock(int eggTypeId, int flockId)
        {
            throw new NotImplementedException();
        }

        public Flock GetFlockById(int id)
        {
            return new Flock { Id = 2, FlockCode = "1111-23", Description = "Bennekom stal2", EggTypeId = 2 };
        }

        public IEnumerable<Flock> GetFlocksForEggType(int eggTypeId)
        {
            throw new NotImplementedException();
        }

        public bool EggTypeExists(int eggTypeId)
        {
            throw new NotImplementedException();
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void UpdateFlock(Flock flock)
        {
            throw new NotImplementedException();
        }

        public bool ExternalEggTypeIdExists(int externalEggTypeId)
        {
            throw new NotImplementedException();
        }

    }
}
