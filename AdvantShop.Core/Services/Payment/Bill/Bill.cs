//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Shipping;

namespace AdvantShop.Payment
{
    public class BillPaymentOption : BasePaymentOption
    {
        public BillPaymentOption()
        {
        }

        public BillPaymentOption(PaymentMethod menthod, float preCoast)
            : base(menthod, preCoast)
        {
        }

        public string CompanyName { get; set; }
        public string INN { get; set; }

        public override PaymentDetails GetDetails()
        {
            return new PaymentDetails {CompanyName = CompanyName, INN = INN};
        }

        public override string Template
        {
            get { return UrlService.GetUrl() + "scripts/_partials/payment/extendTemplate/BillPaymentOption.html"; }
        }

        public override bool Update(BasePaymentOption temp)
        {
            var curent = temp as BillPaymentOption;
            if (curent == null) return false;
            INN = curent.INN;
            CompanyName = curent.CompanyName;
            return true;
        }
    }

    /// <summary>
    /// Summary description for Bill
    /// </summary>
    [PaymentKey("Bill")]
    public class Bill : PaymentMethod
    {
        public string CompanyName { get; set; }
        public string TransAccount { get; set; }
        public string CorAccount { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string INN { get; set; }
        public string KPP { get; set; }
        public string BIK { get; set; }
        public string BankName { get; set; }
        public string Director { get; set; }
        public string PosDirector { get; set; }
        public string Accountant { get; set; }
        public string PosAccountant { get; set; }
        public string Manager { get; set; }
        public string PosManager { get; set; }
        public string StampImageName { get; set; }

        public override ProcessType ProcessType
        {
            get { return ProcessType.Javascript; }
        }

        public override Dictionary<string, string> Parameters
        {
            get
            {
                return new Dictionary<string, string>
                {
                    {BillTemplate.CompanyName, CompanyName},
                    {BillTemplate.TransAccount, TransAccount},
                    {BillTemplate.CorAccount, CorAccount},
                    {BillTemplate.Address, Address},
                    {BillTemplate.Telephone, Telephone},
                    {BillTemplate.INN, INN},
                    {BillTemplate.KPP, KPP},
                    {BillTemplate.BIK, BIK},
                    {BillTemplate.BankName, BankName},
                    {BillTemplate.Director, Director},
                    {BillTemplate.Accountant, Accountant},
                    {BillTemplate.Manager, Manager},
                    {BillTemplate.StampImageName, StampImageName},
                    {BillTemplate.PosAccountant, PosAccountant},
                    {BillTemplate.PosDirector, PosDirector},
                    {BillTemplate.PosManager, PosManager}
                };
            }
            set
            {
                CompanyName = value.ElementOrDefault(BillTemplate.CompanyName);
                Accountant = value.ElementOrDefault(BillTemplate.Accountant);
                TransAccount = value.ElementOrDefault(BillTemplate.TransAccount);
                CorAccount = value.ElementOrDefault(BillTemplate.CorAccount);
                Address = value.ElementOrDefault(BillTemplate.Address);
                Telephone = value.ElementOrDefault(BillTemplate.Telephone);
                INN = value.ElementOrDefault(BillTemplate.INN);
                KPP = value.ElementOrDefault(BillTemplate.KPP);
                BIK = value.ElementOrDefault(BillTemplate.BIK);
                BankName = value.ElementOrDefault(BillTemplate.BankName);
                Director = value.ElementOrDefault(BillTemplate.Director);
                Manager = value.ElementOrDefault(BillTemplate.Manager);
                StampImageName = value.ElementOrDefault(BillTemplate.StampImageName);
                PosDirector = value.ElementOrDefault(BillTemplate.PosDirector);
                PosAccountant = value.ElementOrDefault(BillTemplate.PosAccountant);
                PosManager = value.ElementOrDefault(BillTemplate.PosManager);
            }
        }

        public override string ProcessJavascriptButton(Orders.Order order)
        {
            return String.Format("javascript:window.open('paymentreceipt/bill?ordernumber={0}');", order.Number);
        }

        public override string ButtonText
        {
            get { return LocalizationService.GetResource("Core.Payment.Bill.PrintBill"); }
        }

        public override BasePaymentOption GetOption(BaseShippingOption shippingOption, float preCoast)
        {
            var option = new BillPaymentOption(this, preCoast);            
            return option;
        }

        public override PaymentDetails PaymentDetails()
        {
            return new PaymentDetails() {CompanyName = CompanyName, INN = INN};
        }
    }
}