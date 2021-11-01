using AutoMapper;
using EggTypeService;
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
            CreateMap<ProductCreateWithFKDto, Product>();
            CreateMap<ProductUpdateDto, Product>();
            CreateMap<Product, ProductUpdateDto>();

            CreateMap<EggType, EggTypeReadDto>();
            CreateMap<EggTypePublishedDto, EggType>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id)); // expliciet aangeven dat de externalId van EggType komt vanuit EggTypePublishedDto
            CreateMap<GrpcEggTypeModel, EggType>()
               .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.EggTypeId)) // expliciet aangeven dat de externalId van EggType komt vanuit EggTypePublishedDto
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EggTypeId));
        }
    }
}
