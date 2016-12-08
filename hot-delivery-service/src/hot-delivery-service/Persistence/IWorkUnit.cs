using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hot_delivery_service.Persistence
{
    //интерфейс юнита для работы с бд, позволяющий сохранить изменения
    public interface IWorkUnit: IDisposable
    {
        void SaveChanges();
    }
}
