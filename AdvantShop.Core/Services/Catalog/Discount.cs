using AdvantShop.Repository.Currencies;

namespace AdvantShop.Catalog
{
    public enum DiscountType
    {
        Percent = 0,
        Amount = 1,
    }

    public class Discount
    {
        public Discount() { }

        public Discount(float percent, float amount)
        {
            if (percent != 0)
            {
                Percent = percent;
                Type = DiscountType.Percent;
            }
            else if (amount != 0)
            {
                Amount = amount;
                Type = DiscountType.Amount;
            }
        }

        public Discount(float percent, float amount, DiscountType type)
        {
            if (type == DiscountType.Percent)
                Percent = percent;
            else
                Amount = amount;

            Type = type;
        }

        public float Percent { get; private set; }
        public float Amount { get; private set; }

        public DiscountType Type { get; private set; }

        public bool HasValue
        {
            get
            {
                return (Type == DiscountType.Percent && Percent != 0) ||
                       (Type == DiscountType.Amount && Amount != 0);
            }
        }
    }

    public static class DiscountExtensions
    {
        public static string GetText(this Discount discount, Currency renderCurrency = null)
        {
            if (renderCurrency == null)
                renderCurrency = CurrencyService.CurrentCurrency;

            return discount.Type == DiscountType.Percent
                   ? discount.Percent + "%"
                   : (renderCurrency != null
                       ? (renderCurrency.IsCodeBefore
                           ? renderCurrency.Symbol + discount.Amount
                           : discount.Amount + " " + renderCurrency.Symbol)
                       : discount.Amount.ToString());
        }
    }
}
