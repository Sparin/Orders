using Orders.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Model.DTO.Order
{
    public class SearchQuery
    {
        public Fruit? Fruit { get; set; }
        public int? MinimumAmount { get; set; }
        public int? MaximumAmount { get; set; }

        public string Address { get; set; }
        public bool? IsCallbackRequired { get; set; }
        public DateTimeOffset? FromETA { get; set; }
        public DateTimeOffset? UntilETA { get; set; }

        public string OrderBy { get; set; }
        public bool Descending { get; set; }
    }
}
