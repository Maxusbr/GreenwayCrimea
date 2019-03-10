using System.Collections.Generic;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.ViewModel.PaymentReceipt
{
    public class CheckViewModel
    {
        public string OrderDate { get; set; }
        public string OrderId { get; set; }
        public string ShippingMethod { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string CompanyPhone { get; set; }
        public string InterPhone { get; set; }
        public string CompanyFax { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string BillingAddress { get; set; }
        public string BillingCity { get; set; }
        public string BillingState { get; set; }
        public string BillingCountry { get; set; }
        public string BillingZip { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingState { get; set; }
        public string ShippingCountry { get; set; }
        public string ShippingZip { get; set; }
        public string SubTotal { get; set; }
        public string ShippingCost { get; set; }
        public string Discount { get; set; }
        public string Total { get; set; }

        public List<OrderItem> OrderItems { get; set; }
        public Currency OrderCurrency { get; set; }

        public Currency RenderCurrency { get; set; }
    }
}