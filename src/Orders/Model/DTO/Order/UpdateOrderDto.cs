using Orders.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Model.DTO.Order
{
    public class UpdateOrderDto
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public Fruit Fruit { get; set; }
        public int Amount { get; set; }

        public string Address { get; set; }
        public bool IsCallbackRequired { get; set; }
        public DateTimeOffset ETA { get; set; }
    }
}
