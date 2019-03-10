using AdvantShop.Repository.Currencies;

namespace AdvantShop.Core.Services.Catalog
{
    public static class PriceExtentions
    {
        public static float RoundPrice(this float price, float baseCurrencyValue = 1, Currency renderCurrency = null)
        {
            return PriceService.RoundPrice(price, renderCurrency, baseCurrencyValue);
        }

        public static float RoundPrice(this float price, Currency renderCurrency)
        {
            return PriceService.RoundPrice(price, renderCurrency, 1);
        }

        public static string RoundAndFormatPrice(this float price, Currency renderCurrency, float baseCurrencyValue = 1)
        {
            return PriceService.RoundPrice(price, renderCurrency, baseCurrencyValue).FormatPrice(renderCurrency);
        }

        public static string FormatPrice(this float price)
        {
            return PriceFormatService.FormatPrice(price);
        }

        public static string FormatPrice(this float price, bool isWrap, bool isMainPrice)
        {
            return PriceFormatService.FormatPrice(price, isWrap, isMainPrice);
        }

        public static string FormatPrice(this float price, Currency currency)
        {
            return PriceFormatService.FormatPrice(price, currency);
        }

        public static string FormatPriceInvariant(this float price)
        {
            return PriceFormatService.FormatPriceInvariant(price);
        }
    }
}
