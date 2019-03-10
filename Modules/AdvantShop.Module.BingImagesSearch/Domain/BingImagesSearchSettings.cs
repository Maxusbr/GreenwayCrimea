using AdvantShop.Core.Modules;

namespace AdvantShop.Module.BingImagesSearch.Domain
{
    class BingImagesSearchSettings
    {
        public static string ApiKey
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("ApiKey", BingImagesSearchModule.ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("ApiKey", value, BingImagesSearchModule.ModuleID); }
        }
    }
}
