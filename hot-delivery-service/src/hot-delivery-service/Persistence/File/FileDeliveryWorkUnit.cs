﻿using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hot_delivery_service.Models;
using System.IO;

namespace hot_delivery_service.Persistence.File
{
    //реализация юнита для работы с бд, основанной на списочном репозитории и файловым хранилищем
    //для хранения данных был выбран формат json
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

        //чтение данных из файла в List и создание репозитория на его основе
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
                    catch(Exception ex)
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

        //сохранение изменений
        public void SaveChanges()
        {
            lock (_lock)
            {
                //проставляем добавленным записям id, на основании максимального из уже имеющихся
                int maxId = _deliveries.Max(d => d.Id);
                var deliveries = _deliveries.OrderBy(d => d.Id).ToList();

                foreach (var delivery in deliveries)
                {
                    if (delivery.Id == 0)
                        delivery.Id = ++maxId;
                }

                //сериализуем в json и сохраняем в файл
                string data = JsonConvert.SerializeObject(deliveries);
                using (StreamWriter writer = new StreamWriter(new FileStream(_fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite)))
                {
                    writer.Write(data);
                }
            }
        }
    }
}