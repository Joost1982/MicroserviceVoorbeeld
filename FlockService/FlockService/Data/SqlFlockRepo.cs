using FlockService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlockService.Data
{
    public class SqlFlockRepo : IFlockRepo
    {
        private FlockContext _context;

        public SqlFlockRepo(FlockContext context)
        {
            _context = context;
        }

        public void CreateFlock(Flock flock)
        {
            if (flock == null)
            {
                throw new ArgumentException(nameof(flock));
            }

            _context.Flocks.Add(flock);
        }

        public void CreateFlock(int eggTypeId, Flock flock)
        {
            if (flock == null)
            {
                throw new ArgumentNullException(nameof(flock));
            }
            flock.EggTypeId = eggTypeId;
            _context.Flocks.Add(flock);
        }

        public void CreateEggType(EggType eggType)
        {
            if (eggType == null)
            {
                throw new ArgumentNullException(nameof(eggType));
            }
            _context.EggTypes.Add(eggType);
        }

        public void DeleteFlock(Flock flock)
        {
            if (flock == null)
            {
                throw new ArgumentException(nameof(flock));
            }

            _context.Flocks.Remove(flock);
        }

        public IEnumerable<Flock> GetAllFlocks()
        {
            return _context.Flocks.ToList();
        }

        public IEnumerable<EggType> GetAllEggTypes()
        {
            return _context.EggTypes.ToList();
        }

        public Flock GetFlock(int eggTypeId, int flockId)
        {
            return _context.Flocks
                .Where(c => c.EggTypeId == eggTypeId && c.Id == flockId).FirstOrDefault();
        }

        public Flock GetFlockById(int id)
        {
            return _context.Flocks.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Flock> GetFlocksForEggType(int eggTypeId)
        {
            return _context.Flocks
                .Where(c => c.EggTypeId == eggTypeId)
                .OrderBy(c => c.EggType.Description);
        }

        public bool EggTypeExists(int eggTypeId)
        {
            return _context.EggTypes.Any(p => p.Id == eggTypeId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateFlock(Flock flock)
        {
            //doe niks
        }

        public bool ExternalEggTypeIdExists(int externalEggTypeId)
        {
            return _context.EggTypes.Any(e => e.Id == externalEggTypeId);
        }
    }
}
