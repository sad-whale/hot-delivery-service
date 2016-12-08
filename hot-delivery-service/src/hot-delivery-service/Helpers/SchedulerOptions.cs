﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hot_delivery_service.Helpers
{
    //класс для хранения опций, считанных из конфига
    public class SchedulerOptions
    {
        public int ExpirationTime { get; set; }
        public int CreateIntervalMin { get; set; }
        public int CreateIntervalMax { get; set; }
    }
}
