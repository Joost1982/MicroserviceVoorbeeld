using AutoMapper;
using ProductService.Dtos;
using ProductService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Profiles
{
    public class ProductsProfile : Profile
    {
        public ProductsProfile()
        {
            //Source -> Target
            CreateMap<Product, ProductReadDto>();
            CreateMap<ProductCreateDto, Product>();
            CreateMap<ProductUpdateDto, Product>();
            CreateMap<Product, ProductUpdateDto>();

            CreateMap<EggType, EggTypeReadDto>();
        }
    }
}
