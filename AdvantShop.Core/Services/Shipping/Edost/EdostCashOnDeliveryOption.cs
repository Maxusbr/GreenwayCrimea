using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Payment;

namespace AdvantShop.Shipping.Edost
{
    public class EdostCashOnDeliveryOption : EdostOption
    {
        public float BasePrice { get; set; }
        public float PriceCash { get; set; }
        public float Transfer { get; set; }

        public EdostCashOnDeliveryOption()
        {
        }

        public EdostCashOnDeliveryOption(ShippingMethod method, EdostTarif tarif)
            : base(method, tarif)
        {
            BasePrice = tarif.Price;
            PriceCash = tarif.PriceCash;
            Transfer = tarif.PriceTransfer;

            Rate = BasePrice;
        }

        public override bool ApplyPay(BasePaymentOption payOption)
        {
            if (payOption != null && payOption.GetType() == typeof(CashOnDeliverytOption))
                Rate = PriceCash;
            else
            {
                Rate = BasePrice;
            }
            return true;
        }

        //todo research
        public override string GetDescriptionForPayment()
        {
            var diff = PriceCash - BasePrice;
            if (diff <= 0)
                return string.Empty;

            string transferMessage = Transfer > 0 ? 
                string.Format(LocalizationService.GetResource("Edost.CachOnDelivery.Transfer"), 
                        Transfer.FormatPrice(), (diff + Transfer).FormatPrice()) 
                : string.Empty;

            return string.Format(LocalizationService.GetResource("Edost.CachOnDelivery.Sum"), diff.FormatPrice(), transferMessage);
        }
    }
}