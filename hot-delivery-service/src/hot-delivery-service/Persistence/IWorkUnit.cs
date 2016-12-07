using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hot_delivery_service.Persistence
{
    public interface IWorkUnit: IDisposable
    {
        void SaveChanges();
    }
}
