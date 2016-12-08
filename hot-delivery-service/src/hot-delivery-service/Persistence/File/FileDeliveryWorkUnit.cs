using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hot_delivery_service.Models;
using System.IO;

namespace hot_delivery_service.Persistence.File
{
    public class FileDeliveryWorkUnit : IDeliveryWorkUnit
    {
        private readonly object _lock = new object();

        private string _fileName;
        private ListRepository<Delivery> _deliveries;

        public FileDeliveryWorkUnit(string storageFileName)
        {
            _fileName = storageFileName;
            _deliveries = LoadData(_fileName);
        }

        private ListRepository<Delivery> LoadData(string fileName)
        {

            List<Delivery> list;
            lock (_lock)
            {
                using (StreamReader reader = new StreamReader(new FileStream(_fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite)))
                {
                    string data = reader.ReadToEnd();
                    try
                    {
                        list = JsonConvert.DeserializeObject<List<Delivery>>(data);
                        if (list == null)
                            list = new List<Delivery>();
                    }
                    catch
                    {
                        list = new List<Delivery>();
                    }
                }
            }

            return new ListRepository<Delivery>(list);
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
        }

        public void SaveChanges()
        {
            lock (_lock)
            {
                int maxId = _deliveries.Max(d => d.Id);
                var deliveries = _deliveries.OrderBy(d => d.Id).ToList();

                foreach (var delivery in deliveries)
                {
                    if (delivery.Id == 0)
                        delivery.Id = ++maxId;
                }

                string data = JsonConvert.SerializeObject(deliveries);
                using (StreamWriter writer = new StreamWriter(new FileStream(_fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite)))
                {
                    writer.Write(data);
                }
            }
        }
    }
}