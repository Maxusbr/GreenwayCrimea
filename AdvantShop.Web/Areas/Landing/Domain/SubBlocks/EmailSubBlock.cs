using AdvantShop.Catalog;
using AdvantShop.Configuration;

namespace AdvantShop.App.Landing.Domain.SubBlocks
{
    public class EmailSubBlock : BaseLpSubBlock
    {
        public override string GetContent(Product product)
        {
            return SettingsCheckout.BuyInOneClickEmail;
        }
    }
}
