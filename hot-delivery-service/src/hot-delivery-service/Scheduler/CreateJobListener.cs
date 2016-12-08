using System;
using System.Threading.Tasks;
using Quartz;

namespace hot_delivery_service.Scheduler
{
    //джоб-лиснер, перепланирующий таску создания доставки
    internal class CreateJobListener : IJobListener
    {
        //функция, создающая новый триггер (задается извне)
        private Func<ITrigger> _createTriggerFunction;

        public CreateJobListener(Func<ITrigger> createTriggerFunction)
        {
            _createTriggerFunction = createTriggerFunction;
        }

        public string Name
        {
            get
            {
                return "Create job listener";
            }
        }

        public Task JobExecutionVetoed(IJobExecutionContext context)
        {
            return Task.Run(() => { });
        }

        public Task JobToBeExecuted(IJobExecutionContext context)
        {
            return Task.Run(() => { });
        }

        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException)
        {
            return Task.Run(() =>
            {
                var scheduler = context.Scheduler;
                var triggerKey = context.Trigger.Key;
                var newTrigger = _createTriggerFunction();
                scheduler.RescheduleJob(triggerKey, newTrigger);
            });
        }
    }
}