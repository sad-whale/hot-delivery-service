using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hot_delivery_service.Persistence
{
    public interface IDeliveryWorkUnitProvider
    {
        IDeliveryWorkUnit GetWorkUnit();
    }
}
