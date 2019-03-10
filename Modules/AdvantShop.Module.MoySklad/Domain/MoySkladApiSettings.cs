using AdvantShop.Core.Modules;

namespace AdvantShop.Module.MoySklad.Domain
{
    public class MoySkladApiSettings
    {
        public static string Login
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("MoySkladApiLogin", MoySklad.GetModuleStringId()); }
        }
        public static string Password
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("MoySkladApiPassword", MoySklad.GetModuleStringId()); }
        }
    }
}
