using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hot_delivery_service.Scheduler
{
    //интерфейс планировщика, позволяющий запустить задачи
    public interface IDeliveryScheduler
    {
        void StartTasks();
    }
}
