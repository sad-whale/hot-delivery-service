using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;

namespace hot_delivery_service.Scheduler
{
    //реализация кастомной джоб фабрики, позволяющей использовать dependency injection в классах задач
    public class CustomJobFactory : IJobFactory
    {
        private IServiceProvider _serviceProvider;

        public CustomJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                IJobDetail jobDetail = bundle.JobDetail;
                Type jobType = jobDetail.JobType;
                
                return (IJob)_serviceProvider.GetService(jobType);
            }
            catch (Exception e)
            {
                SchedulerException se = new SchedulerException("Problem instantiating class", e);
                throw se;
            }
        }

        public void ReturnJob(IJob job)
        {

        }
    }
}