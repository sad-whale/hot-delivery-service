using hot_delivery_service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hot_delivery_service.Commands
{
    public class DeliveryExpireCommand
    {
        public int DeliveryId { get; set; }
    }
}
