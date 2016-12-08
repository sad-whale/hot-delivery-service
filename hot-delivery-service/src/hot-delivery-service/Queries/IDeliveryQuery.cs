using hot_delivery_service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hot_delivery_service.Queries
{
    //интерфейс для запроса к доставкам - моя попытка придерживаться принципов CQRS
    public interface IDeliveryQuery
    {
        IQueryable<Delivery> Deliveries { get; }
    }
}
