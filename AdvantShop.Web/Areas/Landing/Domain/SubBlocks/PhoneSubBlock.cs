using AdvantShop.Catalog;
using AdvantShop.Configuration;

namespace AdvantShop.App.Landing.Domain.SubBlocks
{
    public class PhoneSubBlock : BaseLpSubBlock
    {
        public override string GetContent(Product product)
        {
            return SettingsMain.Phone;
        }
    }
}
