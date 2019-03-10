using AdvantShop.Core.Services.Catalog;
using AdvantShop.Payment;
using System;
using AdvantShop.Taxes;

namespace AdvantShop.Shipping
{
    public abstract class AbstractShippingOption : IShippingOption
    {
        public virtual string Id
        {
            get { return MethodId + "_" + (Name + MethodId.ToString() + DeliveryId.ToString()).GetHashCode(); }
        }

        public int DeliveryId { get; set; }
        public int MethodId { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public bool DisplayCustomFields { get; set; }
        public bool DisplayIndex { get; set; }
        public string IconName { get; set; }
        public bool ShowInDetails { get; set; }
        public string ZeroPriceMessage { get; set; }
        public TaxType TaxType { get; set; }
        public string ShippingType { get; set; }

        public string NameRate { get; set; }
        public bool HideAddressBlock { get; set; }

        public float Rate { get; set; }

        public string FormatRate
        {
            get { return Rate == 0 ? ZeroPriceMessage : Rate.FormatPrice(false, false); } // .RoundPrice(CurrencyService.CurrentCurrency)
        }

        public string DeliveryTime { get; set; }

        public Type ModelType
        {
            get { return this.GetType(); }
        }


        public virtual string Template
        {
            get { return ""; }
        }

        public virtual OptionValidationResult Validate()
        {
            return new OptionValidationResult()
            {
                IsValid = true
            };
        }


        public virtual string ForMailTemplate()
        {
            return NameRate ?? Name;
        }

        public virtual string GetDescriptionForPayment()
        {
            return string.Empty;
        }

        public virtual bool ApplyPay(BasePaymentOption payOption)
        {
            return false;
        }
    }
}