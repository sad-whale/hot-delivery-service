using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hot_delivery_service.Models
{
    public class Delivery
    {
        public int Id { get; set; }
        public DeliveySatus Status { get; set; }
        public string Title { get; set; }
        public int UserId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public int ExpirationTime { get; set; }
    }

    public enum DeliveySatus
    {
        Available = 0,
        Taken = 1,
        Expired = 2
    }
}
