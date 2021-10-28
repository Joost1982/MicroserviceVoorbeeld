using AutoMapper;
using ProductService.Data;
using ProductService.Dtos;
using ProductService.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Controllers
{

    // Les Jackson gebruikt "api/c/platforms/{platform{id}/[controller]" als top route.
    // Omdat voorliggend project van een andere tutorial komt (en bovendien aangepast is), heb ik er "api" van gemaakt
    // en de methoden voorzien van de extra paths. Hierdoor kunnen zowel de oude als de 
    // nieuwe endpoints gebruikt worden.

    [ApiController] // zorgt er o.a. ook voor dat applicatie de ingevoerde json checkt op benodigde velden
    [Route("api")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepo _repository;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // Nieuwe endpoints

        [HttpGet]
        [Route("f/eggtypes/{eggTypeId}/products")]   // /api/c/...etc
        public ActionResult<IEnumerable<ProductReadDto>> GetProductsForEggType(int eggTypeId)
        {
            Console.WriteLine($"--> Hit GetProductsForEggType: {eggTypeId}");
            if (!_repository.EggTypeExists(eggTypeId))
            {
                return NotFound();
            }

            var productItems = _repository.GetProductsForEggType(eggTypeId);
            return Ok(_mapper.Map<IEnumerable<ProductReadDto>>(productItems));
        }

        // route -> /api/f/eggtypes/{eggTypeId}/products/{productId}
        [HttpGet("f/eggtypes/{eggTypeId}/products/{productId}", Name = "GetProductForEggType")]
        public ActionResult<ProductReadDto> GetProductForEggType(int eggTypeId, int productId)
        {
            Console.WriteLine($"--> Hit GetProductsForEggType: {eggTypeId} / {productId}");
            if (!_repository.EggTypeExists(eggTypeId))
            {
                return NotFound();
            }

            var productItem = _repository.GetProduct(eggTypeId, productId);

            if (productItem == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<ProductReadDto>(productItem));
        }

        [HttpPost]
        [Route("f/eggtypes/{eggTypeId}/products")]
        public ActionResult<ProductReadDto> CreateProductForEggType(int eggTypeId, ProductCreateDto productDto)
        {
            Console.WriteLine($"--> Hit CreateProductForEggType: {eggTypeId}");
            if (!_repository.EggTypeExists(eggTypeId))
            {
                return NotFound();
            }

            var product = _mapper.Map<Product>(productDto); //geen foutchecks nodig, doet [ApiController] al

            _repository.CreateProduct(product);
            _repository.SaveChanges();

            //we willen een URI returnen naar het nieuw aangemaakte command, dus:
            var productReadDto = _mapper.Map<ProductReadDto>(product); // door SaveChanges() heeft het command object hier een Id

            return CreatedAtRoute(nameof(GetProductForEggType),
                new { eggTypeId = eggTypeId, productId = productReadDto.Id }, productReadDto);
        }


        //  Endpoints van oude tutorial

        [HttpGet]
        [Route("products")]   // /api/products
        public ActionResult<IEnumerable<ProductReadDto>> GetAllProducts()
        {
            var productItems = _repository.GetAllProducts();
            return Ok(_mapper.Map<IEnumerable<ProductReadDto>>(productItems));
        }

        [HttpGet]
        [Route("products/{id}", Name = "GetProductById")] // /api/products/{id} // Name is nodig voor CreatedAtRoute()
        public ActionResult<ProductReadDto> GetProductById(int id)
        {
            var productItem = _repository.GetProductById(id);
            if (productItem != null)
            {
                return Ok(_mapper.Map<ProductReadDto>(productItem));
            }

            return NotFound();
        }

        [Route("products")]   // /api/products
        [HttpPost]
        public ActionResult<ProductReadDto> CreateProduct(ProductCreateDto productCreateDto) 
        {
            var productModel = _mapper.Map<Product>(productCreateDto);
            _repository.CreateProduct(productModel);
            _repository.SaveChanges();

            var productReadDto = _mapper.Map<ProductReadDto>(productModel);

            return CreatedAtRoute(nameof(GetProductById), new {Id = productReadDto.Id }, productReadDto);
            // nu wordt er een Location (https://localhost:xxxxxx/api/products/5) in de header meegestuurd bij de POST en een 201 status


            //return Ok(productReadDto);

        }

        [Route("products")]   // /api/products
        [HttpPut("{id}")]
        public ActionResult UpdateProduct(int id, ProductUpdateDto productUpdateDto)
        {
            var productModelFromRepo = _repository.GetProductById(id);
            if (productModelFromRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(productUpdateDto, productModelFromRepo);    // changes are tracked by dbContext

            _repository.UpdateProduct(productModelFromRepo);        // dus dit hoeft niet per se, maar is good practice (voor als je een andere implementatie hebt)
            //deze methode is ook helemaal niet implemented in sqlrepo.

            _repository.SaveChanges();

            return NoContent();
        }

        //PATCH api/products/{id}


        //patches hebben dit formaat als Json body:

        /*        [
                    {
                     "op" : "replace",
                     "path" : "/productcode",
                     "value" : "nieuwe waarde"
                     },
                     {
                     "op" : "replace",
                     "path" : "/isActive",
                     "value" : "nieuwe waardeLine"
                     }
                ]

                of een enkele:

                 [
                    {
                     "op" : "replace",
                     "path" : "/productcode",
                     "value" : "nieuwe waarde"
                     }
                ]
         */

        [Route("products")]   // /api/products
        [HttpPatch("{id}")]
        public ActionResult PartialProductUpdate(int id, JsonPatchDocument<ProductUpdateDto> patchDoc)
        {
            var productModelFromRepo = _repository.GetProductById(id);
            if (productModelFromRepo == null)
            {
                return NotFound();
            }

            var productToPatch = _mapper.Map<ProductUpdateDto>(productModelFromRepo);
            patchDoc.ApplyTo(productToPatch, ModelState);
            
            if (!TryValidateModel(productToPatch)) 
            {
                return ValidationProblem(ModelState);
            }


            //hieronder = hetzelfde als in PUT method/endpoint

            _mapper.Map(productToPatch, productModelFromRepo);    // changes are tracked by dbContext

            _repository.UpdateProduct(productModelFromRepo);        // dus dit hoeft niet per se, maar is good practice (voor als je een andere implementatie hebt)
            //deze methode is ook helemaal niet implemented in sqlrepo.

            _repository.SaveChanges();

            return NoContent();

        }


        //DELETE api/products/{id}

        [Route("products")]   // /api/products
        [HttpDelete("{id}")]
        public ActionResult DeleteProduct(int id)
        {
            var productModelFromRepo = _repository.GetProductById(id);
            if (productModelFromRepo == null)
            {
                return NotFound();
            }


            _repository.DeleteProduct(productModelFromRepo);
            _repository.SaveChanges();

            return NoContent();
        }

    }
}
