using ProductService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Data
{
    public class SqlProductRepo : IProductRepo
    {
        private ProductContext _context;

        public SqlProductRepo(ProductContext context)
        {
            _context = context;
        }

        public void CreateProduct(Product prod)
        {
            if (prod == null)
            {
                throw new ArgumentException(nameof(prod));
            }

            _context.Products.Add(prod);
        }

        public void CreateProduct(int eggTypeId, Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            product.EggTypeId = eggTypeId;
            _context.Products.Add(product);
        }

        public void CreateEggType(EggType eggType)
        {
            if (eggType == null)
            {
                throw new ArgumentNullException(nameof(eggType));
            }
            _context.EggTypes.Add(eggType);
        }

        public void DeleteProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentException(nameof(product));
            }

            _context.Products.Remove(product);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public IEnumerable<EggType> GetAllEggTypes()
        {
            return _context.EggTypes.ToList();
        }

        public Product GetProduct(int eggTypeId, int productId)
        {
            return _context.Products
                .Where(c => c.EggTypeId == eggTypeId && c.Id == productId).FirstOrDefault();
        }

        public Product GetProductById(int id)
        {
            return _context.Products.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Product> GetProductsForEggType(int eggTypeId)
        {
            return _context.Products
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

        public void UpdateProduct(Product product)
        {
            //doe niks
        }

        public bool ExternalEggTypeIdExists(int externalEggTypeId)
        {
            return _context.EggTypes.Any(e => e.Id == externalEggTypeId);
        }
    }
}
