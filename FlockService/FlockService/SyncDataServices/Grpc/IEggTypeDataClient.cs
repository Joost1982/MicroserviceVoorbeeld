using FlockService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlockService.SyncDataServices.Grpc
{
    public interface IEggTypeDataClient
    {
        IEnumerable<EggType> ReturnAllEggTypes();

    }
}
