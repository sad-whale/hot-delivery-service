using hot_delivery_service.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hot_delivery_service.CommandHandlers
{
    public interface IDeliveryCommandHandler
    {
        int Handle(CreateDeliveryCommand command);
        void Handle(DeliveryExpireCommand command);
        void Handle(AssignToUserCommand command);
    }
}
