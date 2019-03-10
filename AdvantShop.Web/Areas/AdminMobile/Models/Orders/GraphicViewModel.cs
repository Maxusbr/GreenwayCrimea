using System;
namespace AdvantShop.Areas.AdminMobile.Models.Orders
{
    public class GraphicViewModel
    {
        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public bool Paid { get; set; }

        public string DataChart { get; set; }
        
        public string Min { get; set; }

        public string Max { get; set; }
    }
}