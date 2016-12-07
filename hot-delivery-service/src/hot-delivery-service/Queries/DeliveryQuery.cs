using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hot_delivery_service.Models;
using hot_delivery_service.Persistence;

namespace hot_delivery_service.Queries
{
    public class DeliveryQuery : IDeliveryQuery
    {
        private IDeliveryWorkUnit _workUnit;

        public DeliveryQuery(IDeliveryWorkUnitProvider workUnitProvider)
        {
            _workUnit = workUnitProvider.GetWorkUnit();
        }

        public IQueryable<Delivery> Deliveries
        {
            get
            {
                return (IQueryable<Delivery>)_workUnit.Deliveries;
            }
        }
    }
}
