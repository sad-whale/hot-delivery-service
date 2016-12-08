using System;
using System.Threading.Tasks;
using Quartz;
using Microsoft.Extensions.Logging;
using hot_delivery_service.Helpers;
using hot_delivery_service.CommandHandlers;
using Microsoft.Extensions.Options;
using hot_delivery_service.Queries;
using System.Linq;
using hot_delivery_service.Commands;

namespace hot_delivery_service.Scheduler
{
    internal class ExpireDeliveriesJob : IJob
    {
        private IDeliveryCommandHandler _commandHandler;
        private IDeliveryQuery _query;
        private readonly IOptions<SchedulerOptions> _optionsAccessor;
        private ILogger<ExpireDeliveriesJob> _logger;

        public ExpireDeliveriesJob(ILogger<ExpireDeliveriesJob> logger, IDeliveryCommandHandler commandHandler, IOptions<SchedulerOptions> optionsAccessor, IDeliveryQuery query)
        {
            _logger = logger;
            _commandHandler = commandHandler;
            _optionsAccessor = optionsAccessor;
            _query = query;
        }

        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                var expiredDeliveries = _query.Deliveries.Where(
                    d => d.Status == Models.DeliveySatus.Available && 
                    d.CreationDate.Add(TimeSpan.FromSeconds(d.ExpirationTime)) < DateTime.Now)
                    .ToList();

                int expiredCount = expiredDeliveries.Count;

                foreach (var expire in expiredDeliveries)
                {
                    _commandHandler.Handle(new DeliveryExpireCommand() { DeliveryId = expire.Id });
                }

                if (expiredCount > 0)
                    _logger.LogInformation($"{expiredCount} deliveries expired!");
            });
        }
    }
}