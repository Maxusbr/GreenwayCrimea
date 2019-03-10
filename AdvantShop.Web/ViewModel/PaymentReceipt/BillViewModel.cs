using System.Collections.Generic;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;
using AdvantShop.Taxes;

namespace AdvantShop.ViewModel.PaymentReceipt
{
    public class BillViewModel
    {
        public string Kpp { get; set; }
        public string Inn { get; set; }
        public string PayeesBank { get; set; }
        public string Bik { get; set; }
        public string CorrespondentAccount { get; set; }
        public string TransactAccount { get; set; }
        public string OrderNumber { get; set; }
        public string OrderDate { get; set; }
        public string Vendor { get; set; }
        public string Buyer { get; set; }
        public string CompanyName { get; set; }
        public string Director { get; set; }
        public string PosDirector { get; set; }
        public string Manager { get; set; }
        public string PosManager { get; set; }
        public string Accountant { get; set; }
        public string PosAccountant { get; set; }
        public string TotalKop { get; set; }

        public string StampImageName { get; set; }

        public string ShippingCost { get; set; }
        public string PaymentCost { get; set; }
        public string ProductsCost { get; set; }
        public string DiscountCost { get; set; }
        public string TotalCost { get; set; }
        public string IntPartPrice { get; set; }

        public List<OrderItem> OrderItems { get; set; }
        public List<AdvantShop.Orders.GiftCertificate> OrderCertificates { get; set; }
        public List<OrderTax> Taxes { get; set; } 

        public OrderCurrency OrderCurrency { get; set; }
        public Currency RenderCurrency { get; set; }
    }
}