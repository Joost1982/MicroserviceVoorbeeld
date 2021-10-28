using AutoMapper;
using FlockService.Dtos;
using FlockService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlockService.Profiles
{
    public class EggTypesProfile : Profile
    {
        public EggTypesProfile()
        {
            //Source -> Target
            CreateMap<Flock, FlockReadDto>();
            CreateMap<FlockCreateDto, Flock>();
            CreateMap<FlockCreateWithFKDto, Flock>();
            CreateMap<FlockUpdateDto, Flock>();
            CreateMap<Flock, FlockUpdateDto>();

            CreateMap<EggType, EggTypeReadDto>();
        }
    }
}
