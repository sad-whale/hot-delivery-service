using hot_delivery_service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hot_delivery_service.Persistence
{
    //интерфейс юнита для работы с бд, предоставляющий доступ к данным
    public interface IDeliveryWorkUnit: IWorkUnit
    {
        IRepository<Delivery> Deliveries { get; }
    }
}
