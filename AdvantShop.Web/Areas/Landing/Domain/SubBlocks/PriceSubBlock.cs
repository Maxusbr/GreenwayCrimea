using System.Collections.Generic;
using System.Linq;
using AdvantShop.Catalog;

namespace AdvantShop.App.Landing.Domain.SubBlocks
{
    public class PriceSubBlock : BaseLpSubBlock
    {
        public override dynamic GetSettings(LpBlock currentBlock, Product product, dynamic settings)
        {
            if (product == null)
                return settings;

            var offer = product.Offers.FirstOrDefault(x => x.Main);
            var discount = product.Discount.Percent;

            if (offer == null)
                return settings;

            if (settings.ContainsKey("old_price"))
            {
                settings["old_price"] = offer.BasePrice.ToString("F2");
            }
            if (settings.ContainsKey("new_price"))
            {
                settings["new_price"] =
                    discount != 0
                        ? (offer.BasePrice - offer.BasePrice/100*discount).ToString("F2")
                        : offer.BasePrice.ToString("F2");
            }
            if (settings.ContainsKey("currency"))
            {
                settings["currency"] = product.Currency.Symbol;
            }

            return settings;
        }
    }
}
