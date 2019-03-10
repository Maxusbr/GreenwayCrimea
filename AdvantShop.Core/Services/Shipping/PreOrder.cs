using System.Collections.Generic;
using System.Linq;
using AdvantShop.Payment;
using AdvantShop.Repository.Currencies;
using System;

namespace AdvantShop.Shipping
{
    [Serializable]
    public class PreOrder
    {
        public string CountryDest { get; set; }
        public string CountryIso { get; set; }
        public string ZipDest { get; set; }
        public string CityDest { get; set; }
        public string RegionDest { get; set; }

        public BaseShippingOption ShippingOption { get; set; }
        public BasePaymentOption PaymentOption { get; set; }

        //public string CurrencyIso3 { get; set; }
        public Currency Currency { get; set; }
        public List<PreOrderItem> Items { get; set; }

        public string AddressDest { get; set; }
        public Guid? BonusCardId { get; set; }
        public bool BonusUseIt { get; set; }
        public float TotalDiscount { get; set; }


        public override int GetHashCode()
        {
            var hash = Items.Aggregate(17, (current, item) => current ^ item.GetHashCode());
            ShippingOption = ShippingOption ?? new BaseShippingOption();
            PaymentOption = PaymentOption ?? new BasePaymentOption();

            return
                (CityDest ?? "").GetHashCode()
                ^ (CountryDest ?? "").GetHashCode()
                ^ (RegionDest ?? "").GetHashCode()
                ^ (AddressDest ?? "").GetHashCode()
                ^ (ZipDest ?? "").GetHashCode()
                ^ (Currency != null ? Currency.Iso3 : "").GetHashCode()
                ^ ShippingOption.Id.GetHashCode()
                ^ PaymentOption.Id.GetHashCode()
                ^ hash;
        }
    }
}