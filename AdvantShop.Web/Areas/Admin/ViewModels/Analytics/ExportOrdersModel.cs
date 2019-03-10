using System;
using System.Collections.Generic;

namespace AdvantShop.Web.Admin.ViewModels.Analytics
{
    public class ExportOrdersModel
    {
        public bool UseStatus { get; set; }

        public bool UseDate { get; set; }

        public bool UsePaid { get; set; }

        public bool UseSum { get; set; }

        public bool UseShipping { get; set; }

        public bool UseCity { get; set; }


        public int Status { get; set; }

        public string Encoding { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }


        public bool Paid { get; set; }

        public int Shipping { get; set; }

        public float OrderSumFrom { get; set; }

        public float OrderSumTo { get; set; }

        public string City { get; set; }


        public Dictionary<string, string> Encodings { get; set; }

        public Dictionary<int, string> OrderStatuses { get; set; }

        public Dictionary<int, string> Shippings { get; set; }

        public Dictionary<string, string> Cities { get; set; }

        public Dictionary<bool, string> PaidStatuses { get; set; }
    }
}
