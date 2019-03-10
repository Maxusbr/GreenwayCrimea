using AdvantShop.Core.Services.Catalog;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Payment
{
    public class BasePaymentOption : AbstractPaymentOption
    {
        public BasePaymentOption()
        {
        }
        public BasePaymentOption(PaymentMethod method, float preCoast)
        {
            Id = method.PaymentMethodId;
            Name = method.Name;
            IconName = PaymentIcons.GetPaymentIcon(method.PaymentKey, method.IconFileName != null ? method.IconFileName.PhotoName : null, method.Name);
            Desc = method.Description;
            Rate = method.ExtrachargeType == ExtrachargeType.Percent
                            ? (method.Extracharge * preCoast / 100).RoundPrice(CurrencyService.CurrentCurrency.Rate)
                            : method.Extracharge.RoundPrice();       
        }

        public virtual PaymentDetails GetDetails()
        {
            return null;
        }

        public virtual bool Update(BasePaymentOption temp)
        {            
            return true;
        }
    }
}