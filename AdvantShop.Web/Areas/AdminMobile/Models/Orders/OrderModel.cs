using System;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Catalog;

namespace AdvantShop.Areas.AdminMobile.Models.Orders
{
    public class OrderModel
    {
        public int OrderId { get; set; }
        
        public string Number { get; set; }

        public float Sum { get; set; }

        public string SumPrice
        {
            get { return Sum.FormatPriceInvariant(); }
        }

        public DateTime OrderDate { get; set; }

        public string OrderDateFormated
        {
            get { return OrderDate.ToString(SettingsMain.AdminDateFormat); }
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string StatusName { get; set; }

        public string Color { get; set; }
    }
}