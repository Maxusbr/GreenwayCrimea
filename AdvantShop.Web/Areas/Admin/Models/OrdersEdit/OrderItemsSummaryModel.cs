using System;
using System.Collections.Generic;
using AdvantShop.Core.Services.Bonuses.Model;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Orders;
using AdvantShop.Payment;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Web.Admin.Models.OrdersEdit
{
    public class OrderItemsSummaryModel
    {
        public OrderCurrency OrderCurrency { get; set; }

        public Card BonusCard { get; set; }
        public Purchase BonusCardPurchase { get; set; }
        public float BonusCost { get; set; }
        public float BonusesAvailable { get; set; }
        public bool CanChangeBonusAmount { get; set; }


        public float ProductsCost { get; set; }
        public string ProductsCostStr { get { return ProductsCost.FormatPrice(OrderCurrency); } }

        public float ProductsWeight { get; set; }
        public string ProductsWeightStr { get { return ProductsWeight.FormatPriceInvariant(); } }

        public float ProductsDiscountPrice { get; set; }
        public string ProductsDiscountPriceStr { get { return ProductsDiscountPrice.FormatPrice(OrderCurrency); } }
        public float OrderDiscount { get; set; }
        
        public float CouponPrice { get; set; }

        public string CouponPriceStr
        {
            get { return CouponPrice.FormatPrice(OrderCurrency); }
        }

        public string Coupon { get; set; }

        public float CertificatePrice { get; set; }
        public string CertificatePriceStr { get { return CertificatePrice.FormatPrice(OrderCurrency); } }
        public OrderCertificate Certificate { get; set; }
        

        public string ShippingName { get; set; }
        public string ShippingType { get; set; }
        public float ShippingCost { get; set; }
        public OrderPickPoint OrderPickPoint { get; set; }
        public string ShippingCostStr { get { return ShippingCost.FormatPrice(OrderCurrency); } }

        public string DeliveryDate { get; set; }
        public string DeliveryTime { get; set; }

        public string PaymentName { get; set; }
        public float PaymentCost { get; set; }
        public string PaymentCostStr { get { return PaymentCost.FormatPrice(OrderCurrency); } }
        public PaymentDetails PaymentDetails { get; set; }
        public string PaymentKey { get; set; }

        public bool ShowSendBillingLink { get; set; }
        public bool ShowPrintPaymentDetails { get; set; }
        public string PrintPaymentDetailsText { get; set; }
        public string PrintPaymentDetailsLink { get; set; }

        public List<KeyValuePair<string, string>> Taxes { get; set; }

        public float Sum { get; set; }
        public string SumStr { get { return Sum.FormatPrice(OrderCurrency); } }
        public float OrderDiscountValue { get; set; }
        public bool Paid { get; set; }
        
    }


    public class LeadItemsSummaryModel
    {
        public Currency LeadCurrency { get; set; }
        
        public float ProductsCost { get; set; }
        public string ProductsCostStr { get { return ProductsCost.FormatPrice(LeadCurrency); } }

        public float ProductsDiscountPrice { get; set; }
        public string ProductsDiscountPriceStr { get { return ProductsDiscountPrice.FormatPrice(LeadCurrency); } }
        public float Discount { get; set; }
        public float DiscountValue { get; set; }

        public float Sum { get; set; }
        public string SumStr { get { return Sum.FormatPrice(LeadCurrency); } }
        public string SumValueFormat { get { return Sum.ToString(); } }

        public string ShippingName { get; set; }
        public float ShippingCost { get; set; }
        public string ShippingCostStr { get { return ShippingCost.FormatPrice(LeadCurrency); } }
        public OrderPickPoint ShippingPickPoint { get; set; }
        public string DeliveryDate { get; set; }
        public string DeliveryTime { get; set; }
        public int LeadItemsCount { get; set; }

        public string City { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
    }
}
