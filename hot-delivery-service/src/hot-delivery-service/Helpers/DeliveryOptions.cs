using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hot_delivery_service.Helpers
{
    public class DeliveryOptions
    {
        public int ExpirationTime { get; set; }
        public int CreateIntervalMin { get; set; }
        public int CreateIntervalMax { get; set; }
        public string StorageType { get; set; }
    }
}
