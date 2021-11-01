using AutoMapper;
using EggTypeService.Data;
using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EggTypeService.SyncDataServices.Grpc
{
    public class GrpcEggTypeService : GrpcEggType.GrpcEggTypeBase
    {
        private readonly IEggTypeRepo _repository;
        private readonly IMapper _mapper;

        public GrpcEggTypeService(IEggTypeRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public override Task<EggTypeResponse> GetAllEggTypes(GetAllRequest request, ServerCallContext context)
        {
            var response = new EggTypeResponse();
            var eggTypes = _repository.GetAllEggTypes();

            foreach (var eggType in eggTypes)
            {
                response.Eggtype.Add(_mapper.Map<GrpcEggTypeModel>(eggType));
            }

            return Task.FromResult(response);
        }
    }
}
