using hot_delivery_service.Queries;
using hot_delivery_service.CommandHandlers;
using hot_delivery_service.Commands;
using hot_delivery_service.Helpers;
using hot_delivery_service.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Quartz.Impl.Matchers;

namespace hot_delivery_service.Scheduler
{
    public class DeliveryScheduler : IDeliveryScheduler
    {
        private IDeliveryQuery _query;
        private IDeliveryCommandHandler _commandHandler;
        private readonly IOptions<SchedulerOptions> _optionsAccessor;
        private IJobFactory _jobFactory;

        private int _createIntervalMin;
        private int _createIntervalMax;

        private Random _random;

        public DeliveryScheduler(IDeliveryQuery query, IDeliveryCommandHandler commandHandler, IOptions<SchedulerOptions> optionsAccessor, IJobFactory jobFactory)
        {
            _query = query;
            _commandHandler = commandHandler;
            _optionsAccessor = optionsAccessor;
            _jobFactory = jobFactory;

            _createIntervalMax = _optionsAccessor.Value.CreateIntervalMax;
            _createIntervalMin = _optionsAccessor.Value.CreateIntervalMin;

            _random = new Random();
        }

        public void StartTasks()
        {
            ISchedulerFactory schedFact = new StdSchedulerFactory();
            IScheduler sched = schedFact.GetScheduler().Result;
            sched.JobFactory = _jobFactory;
            
            sched.Start();

            IJobDetail createJob = JobBuilder.Create<CreateDeliveryJob>()
                .WithIdentity("createJob", "deliveryGroup")
                .Build();
            IJobDetail expireJob = JobBuilder.Create<ExpireDeliveriesJob>()
                .WithIdentity("expireJob", "deliveryGroup")
                .Build();

            ITrigger createTrigger = BuildCreateTrigger();
            ITrigger expireTrigger = BuildExpireTrigger();
            
            sched.ScheduleJob(createJob, createTrigger);
            sched.ScheduleJob(expireJob, expireTrigger);

            sched.ListenerManager.AddJobListener(new CreateJobListener(() => BuildCreateTrigger()), KeyMatcher<JobKey>.KeyEquals(new JobKey("createJob", "deliveryGroup")));
        }

        private ITrigger BuildCreateTrigger()
        {
            var delayTime = _random.Next(_createIntervalMin, _createIntervalMax);

            ITrigger trigger = TriggerBuilder.Create()
              .WithIdentity("createTrigger", "deliveryGroup")
              .StartAt(DateBuilder.FutureDate(delayTime, IntervalUnit.Second))
              .Build();

            return trigger;
        }

        private ITrigger BuildExpireTrigger()
        {
            var delayTime = 10;

            ITrigger trigger = TriggerBuilder.Create()
              .WithIdentity("expireTrigger", "deliveryGroup")
              .StartAt(DateBuilder.FutureDate(delayTime, IntervalUnit.Second))
              .WithSimpleSchedule(x => x
                  .WithIntervalInSeconds(delayTime)
                  .RepeatForever())
              .Build();

            return trigger;
        }
    }
}
