﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hot_delivery_service.Commands
{
    //команда закрепления задачи за пользователем
    public class AssignToUserCommand
    {
        public int DeliveryId { get; set; }
        public int UserId { get; set; }
    }
}
