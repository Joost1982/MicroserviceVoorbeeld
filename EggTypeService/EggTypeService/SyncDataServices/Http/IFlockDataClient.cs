using EggTypeService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EggTypeService.SyncDataServices.Http
{
    public interface IFlockDataClient
    {
        Task SendEggTypeToFlock(EggTypeReadDto eggType);
    }
}
