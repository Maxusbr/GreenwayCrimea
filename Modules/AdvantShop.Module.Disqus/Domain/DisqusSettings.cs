using AdvantShop.Core.Modules;

namespace AdvantShop.Module.Disqus.Domain
{
    class DisqusSettings
    {
        private const string ModuleStringId = "Disqus";

        public static string ShortName
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("ShortName", ModuleStringId); }
            set { ModuleSettingsProvider.SetSettingValue("ShortName", value, ModuleStringId); }
        }
    }
}
