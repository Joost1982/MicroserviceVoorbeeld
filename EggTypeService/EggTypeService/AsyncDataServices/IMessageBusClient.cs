using EggTypeService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EggTypeService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewEggType(EggTypePublishedDto eggTypePublishedDto);
    }
}
