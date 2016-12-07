using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hot_delivery_service.Commands
{
    public class CreateDeliveryCommand
    {
        public string Title { get; set; }
        public int ExpirationTime { get; set; }
    }
}
