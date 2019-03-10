using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdvantShop.Areas.Api.Models
{
    public class FreshDeskModel
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Link { get; set; }
        public int OrdersCount { get; set; }
        public string OrdersSum { get; set; }
        public List<FreshDeskOrder> LastOrders { get; set; }
    }

    public class FreshDeskOrder
    {
        public int ID { get; set; }
        public string Date { get; set; }
        public bool Payed { get; set; }
        public string Status { get; set; }
        public string StatusColor { get; set; }
        public string Sum { get; set; }
        public string Link { get; set; }
    }
}