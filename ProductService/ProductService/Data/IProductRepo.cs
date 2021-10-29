using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductService.Models;

namespace ProductService.Data
{
    public interface IProductRepo
    {

        bool SaveChanges();

        // EggTypes
        IEnumerable<EggType> GetAllEggTypes();
        void CreateEggType(EggType eggType);
        bool EggTypeExists(int eggTypeId);
        bool ExternalEggTypeIdExists(int externalEggTypeId);


        // Products
        IEnumerable<Product> GetAllProducts();
        Product GetProductById(int id);
        void CreateProduct(Product cmd);
        void UpdateProduct(Product cmd);
        void DeleteProduct(Product cmd);
       
        // Combinatie
        IEnumerable<Product> GetProductsForEggType(int eggTypeId);
        Product GetProduct(int eggTypeId, int productId);
        void CreateProduct(int eggTypeId, Product product);

    }
}
