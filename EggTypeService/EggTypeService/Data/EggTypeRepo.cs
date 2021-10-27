using EggTypeService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EggTypeService.Data
{
    public class EggTypeRepo : IEggTypeRepo
    {
        private readonly AppDbContext _context;

        public EggTypeRepo(AppDbContext context)
        {
            _context = context;
        }
        public void CreateEggType(EggType eggtype)
        {
            if (eggtype == null)
            {
                throw new ArgumentNullException(nameof(eggtype));
            }

            _context.EggTypes.Add(eggtype);
        }


        public IEnumerable<EggType> GetAllEggTypes()
        {
            return _context.EggTypes.ToList();
        }

        public EggType GetEggTypeById(int id)
        {
            return _context.EggTypes.FirstOrDefault(p => p.Id == id);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
