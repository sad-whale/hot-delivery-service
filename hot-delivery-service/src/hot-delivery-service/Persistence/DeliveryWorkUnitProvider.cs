using hot_delivery_service.Helpers;
using hot_delivery_service.Persistence.File;
using hot_delivery_service.Persistence.SQLite;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hot_delivery_service.Persistence
{
    public class DeliveryWorkUnitProvider : IDeliveryWorkUnitProvider
    {
        private readonly IOptions<StorageOptions> _optionsAccessor;
        private DeliveryContext _context;

        //внедрение EF контекста для sqlite и опций из конфига
        public DeliveryWorkUnitProvider(IOptions<StorageOptions> optionsAccessor, DeliveryContext context)
        {
            _optionsAccessor = optionsAccessor;
            _context = context;
        }
        public IDeliveryWorkUnit GetWorkUnit()
        {
            //на основании значения опции StorageType создаем и возвращаем конкретный экземпляр
            string storageType = _optionsAccessor.Value.StorageType;

            switch (storageType)
            {
                case "file":
                    return new FileDeliveryWorkUnit(_optionsAccessor.Value.StorageFileName);
                case "sqlite":
                    return new SqliteDeliveryWorkUnit(_context);
                default:
                    throw new Exception($"Unknown storage type: {storageType}");
            }
        }
    }
}
