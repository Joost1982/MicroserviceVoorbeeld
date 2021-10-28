using AutoMapper;
using EggTypeService.Dtos;
using EggTypeService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EggTypeService.Profiles
{
    public class EggTypesProfile : Profile
    {

        public EggTypesProfile()
        {
            // Source -> Target
            CreateMap<EggType, EggTypeReadDto>();
            CreateMap<EggTypeCreateDto, EggType>();
            CreateMap<EggTypeReadDto, EggTypePublishedDto>();
        }
    }
}
