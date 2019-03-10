using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace AdvantShop.Module.RussianPostPrintBlank.Models
{
    public class OrderModelView
    {
        public List<Order> Orders { get; set; }
        public List<string> StatusList { get; set; }
        public string SelectStatus { get; set; }
        public List<PaySelect> PayedList { get; set; }
        public List<string> ShippingList { get; set; }
        public string SelectShipping { get; set; }
        public string SelectPayed { get; set; }
        public string OrderNumber { get; set; }
        public List<SelectListItem> FormTypes { get; set; }

        public int BackPage { get; set; }
        public int NextPage { get; set; }

        public bool Next { get; set; }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public string BuyerName { get; set; }
        public DateTime OrderDate { get; set; }
        public string StatusName { get; set; }
        public SelectListItem FormType { get; set; }
        public DateTime? PaymentDate { get; set; }
        public float Prepayment { get; set; }
        public string ShippingName { get; set; }
    }

    public class PaySelect
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    
    public class OrderInfoTemplatesList
    {
        public int OrderId { get; set; }

        public List<SelectListItem> FormTypes { get; set; }
    }
}
