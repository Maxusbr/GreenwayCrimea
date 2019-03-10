using System;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Catalog;

namespace AdvantShop.Web.Admin.Models.Orders
{
    public partial class OrderItemsModel
    {
        public int OrderId { get; set; }
        public string Number { get; set; }

        public string StatusName { get; set; }
        public int OrderStatusId { get; set; }

        public string PaymentDate { get; set; }
        public bool IsPaid { get; set; } //{ get { return PaymentDate != null; } }

        public string PaymentMethodName { get; set; }
        public string ShippingMethodName { get; set; }
        public string BuyerName { get; set; }
        public string PaymentMethod { get; set; }
        public string ShippingMethod { get; set; }
        
        public float Sum { get; set; }

        public string SumFormatted
        {
            get
            {
                return PriceFormatService.FormatPrice(Sum, CurrencyValue, CurrencySymbol,CurrencyCode, IsCodeBefore);
            }
        }

        public string CurrencyCode { get; set; }
        public float CurrencyValue { get; set; }
        public string CurrencySymbol { get; set; }
        public bool IsCodeBefore { get; set; }

        public DateTime OrderDate { get; set; }

        public string OrderDateFormatted
        {
            get { return OrderDate.ToString(SettingsMain.AdminDateFormat.Replace(":ss", "")); }
        }

        public Guid CustomerId { get; set; }

        public string ManagerId { get; set; }

        public string Color { get; set; }
        public float Rating { get; set; }

        public bool ManagerConfirmed { get; set; }
        public Guid ManagerCustomerId { get; set; }
        public string ManagerName { get; set; }
    }
}
