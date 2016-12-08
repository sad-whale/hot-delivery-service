using System;
using System.Threading.Tasks;
using Quartz;
using Microsoft.Extensions.Logging;
using hot_delivery_service.CommandHandlers;
using hot_delivery_service.Commands;
using Microsoft.Extensions.Options;
using hot_delivery_service.Helpers;

namespace hot_delivery_service.Scheduler
{
    //таска, создающие новые доставки
    internal class CreateDeliveryJob : IJob
    {
        private ILogger<CreateDeliveryJob> _logger;
        private IDeliveryCommandHandler _commandHandler;
        private readonly IOptions<SchedulerOptions> _optionsAccessor;

        //внедрение зависимостей
        public CreateDeliveryJob(ILogger<CreateDeliveryJob> logger, IDeliveryCommandHandler commandHandler, IOptions<SchedulerOptions> optionsAccessor)
        {
            _logger = logger;
            _commandHandler = commandHandler;
            _optionsAccessor = optionsAccessor;
        }

        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                //в качестве рандомного тайтла используем гуид
                string title = Guid.NewGuid().ToString();
                int expTime = _optionsAccessor.Value.ExpirationTime;
                int newId = _commandHandler.Handle(new CreateDeliveryCommand() { Title = title, ExpirationTime = expTime });

                _logger.LogInformation($"Job {newId} {title} created!");
            });
        }
    }
}