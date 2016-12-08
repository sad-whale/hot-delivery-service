using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hot_delivery_service.Models;

namespace hot_delivery_service.Persistence.SQLite
{
    //реализация юнита для работы с бд sqlite, на основе ef core
    public class SqliteDeliveryWorkUnit : IDeliveryWorkUnit
    {
        private DeliveryContext _context;
        private EfRepository<Delivery> _deliveries;

        public SqliteDeliveryWorkUnit(DeliveryContext context)
        {
            _context = context;
            if (_context != null)
                _deliveries = new EfRepository<Delivery>(_context.Deliveries);
            else
                _deliveries = null;
        }

        public IRepository<Delivery> Deliveries
        {
            get
            {
                return _deliveries;
            }
        }

        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }

        public void SaveChanges()
        {
            if (_context != null)
                _context.SaveChanges();
        }
    }
}
