using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hot_delivery_service.Helpers
{
    public class StorageOptions
    {
        //класс для хранения опций, считанных из конфига
        public string StorageType { get; set; }
        public string StorageFileName { get; set; }
    }
}
