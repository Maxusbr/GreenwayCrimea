using AdvantShop.Orders;
using AdvantShop.Taxes;
using System.Collections.Generic;

namespace AdvantShop.ViewModel.Checkout
{
    public class PrintOrderViewModel
    {
        public Order Order { get; set; }

        public bool ShowStatusInfo { get; set; }

        public bool ShowContacts { get; set; }

        public string MapAdress { get; set; }

        public string MapType { get; set; }

        public bool ShowMap { get; set; }

        public OrderCurrency OrderCurrency { get; set; }

        public string OrderDiscount { get; set; }

        public string OrderBonus { get; set; }

        public string Certificate { get; set; }

        public string Coupon { get; set; }

        public string ShippingPrice { get; set; }

        public string PaymentPrice { get; set; }

        public string PaymentPriceTitle { get; set; }

        public List<OrderTax> Taxes { get; set; }

        public string ProductsPrice { get; set; }

        public string TotalPrice { get; set; }

        public string ShippingMethodName { get; set; }
        public string PaymentMethodName { get; set; }
    }
}