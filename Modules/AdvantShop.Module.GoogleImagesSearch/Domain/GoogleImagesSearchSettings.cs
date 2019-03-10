using AdvantShop.Core.Modules;

namespace AdvantShop.Module.GoogleImagesSearch.Domain
{
    class GoogleImagesSearchSettings
    {
        public static string ApiKey
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("ApiKey", GoogleImagesSearchModule.ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("ApiKey", value, GoogleImagesSearchModule.ModuleID); }
        }

        public static string CSEngineId
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("CSEngineId", GoogleImagesSearchModule.ModuleID); }
            set { ModuleSettingsProvider.SetSettingValue("CSEngineId", value, GoogleImagesSearchModule.ModuleID); }
        }
    }
}
