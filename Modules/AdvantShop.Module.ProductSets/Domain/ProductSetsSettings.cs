using AdvantShop.Core.Modules;

namespace AdvantShop.Module.ProductSets.Domain
{
    public class ProductSetsSettings
    {
        public static string Title
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("Title", ProductSets.ModuleId); }
            set { ModuleSettingsProvider.SetSettingValue("Title", value, ProductSets.ModuleId); }
        }
    }
}
