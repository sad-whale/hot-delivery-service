using hot_delivery_service.Helpers;
using hot_delivery_service.Persistence.File;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hot_delivery_service.Persistence
{
    public class DeliveryWorkUnitProvider : IDeliveryWorkUnitProvider
    {
        private readonly IOptions<DeliveryOptions> _optionsAccessor;

        public DeliveryWorkUnitProvider(IOptions<DeliveryOptions> optionsAccessor)
        {
            _optionsAccessor = optionsAccessor;
        }
        public IDeliveryWorkUnit GetWorkUnit()
        {
            string storageType = _optionsAccessor.Value.StorageType;

            switch (storageType)
            {
                case "file":
                    return new FileDeliveryWorkUnit();
                //case "sqlite":
                //    return new SqliteDelivryWorkUnit();
                default:
                    throw new Exception($"Unknown storage type: {storageType}");
            }
        }
    }
}
