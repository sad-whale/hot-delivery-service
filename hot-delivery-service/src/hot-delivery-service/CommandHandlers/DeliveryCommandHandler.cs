using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hot_delivery_service.Commands;
using hot_delivery_service.Persistence;
using hot_delivery_service.Models;

namespace hot_delivery_service.CommandHandlers
{
    public class DeliveryCommandHandler : IDeliveryCommandHandler
    {
        private IDeliveryWorkUnit _workUnit;

        public DeliveryCommandHandler(IDeliveryWorkUnitProvider workUnitProvider)
        {
            _workUnit = workUnitProvider.GetWorkUnit();
        }

        public void Handle(AssignToUserCommand command)
        {
            var delivery = _workUnit.Deliveries.FirstOrDefault(d => d.Id == command.DeliveryId);
            if (delivery != null)
            {
                delivery.Status = DeliveySatus.Taken;
                delivery.UserId = command.UserId;

                _workUnit.SaveChanges();
            }
        }

        public void Handle(DeliveryExpireCommand command)
        {
            var delivery = _workUnit.Deliveries.FirstOrDefault(d => d.Id == command.DeliveryId);
            if (delivery != null)
            {
                delivery.Status = DeliveySatus.Expired;

                _workUnit.SaveChanges();
            }
        }

        public int Handle(CreateDeliveryCommand command)
        {
            var delivery = _workUnit.Deliveries.Create();
            delivery.Title = command.Title;
            delivery.CreationDate = DateTime.Now;
            delivery.ExpirationTime = command.ExpirationTime;
            delivery.Status = DeliveySatus.Available;

            _workUnit.SaveChanges();

            return delivery.Id;
        }
    }
}
