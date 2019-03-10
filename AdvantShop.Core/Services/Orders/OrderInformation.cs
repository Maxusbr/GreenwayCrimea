//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Payment;

namespace AdvantShop.Orders
{
    public class OrderInformation
    {
        public bool Payed { get; set; }
        public int OrderID { get; set; }

        public string OrderNumber { get; set; }

        public string ShippingMethod { get; set; }

        public string ShippingMethodName { get; set; }

        public int PaymentMethodID { get; set; }

        private PaymentMethod _paymentMethod;
        public PaymentMethod PaymentMethod
        {
            get { return _paymentMethod ?? (_paymentMethod = PaymentService.GetPaymentMethod(PaymentMethodID)); }
        }

        public float Sum { get; set; }

        public DateTime OrderDate { get; set; }

        public string Status { get; set; }
        public string PreviousStatus { get; set; }

        public string ProductsHtml { get; set; }

        public string CurrencyCode { get; set; }

        public float CurrencyValue { get; set; }

        public string CurrencySymbol { get; set; }

        public bool IsCodeBefore { get; set; }

        public string PriceFormat { get; set; }

        public Guid CustomerId { get; set; }

        public string CustomerFirstName { get; set; }

        public string CustomerLastName { get; set; }

        public int StatusID { get; set; }

        public string ArchivedPaymentName { get; set; }


        public int? ManagerId { get; set; }
        public string ManagerName { get; set; }
    }
}