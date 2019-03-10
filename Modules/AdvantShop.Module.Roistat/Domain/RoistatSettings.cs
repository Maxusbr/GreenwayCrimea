using AdvantShop.Core.Modules;

namespace AdvantShop.Module.Roistat.Domain
{
    public class RoistatSettings
    {
        public static string RoistatScript
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("RoistatScript", Roistat.ModuleId); }
            set { ModuleSettingsProvider.SetSettingValue("RoistatScript", value, Roistat.ModuleId); }
        }

        /// <summary>
        /// Имя пользователя (Указывается в настройках интеграции)
        /// </summary>
        public static string RoistatLogin
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("RoistatLogin", Roistat.ModuleId); }
            set { ModuleSettingsProvider.SetSettingValue("RoistatLogin", value, Roistat.ModuleId); }
        }

        /// <summary>
        /// Пароль пользователя (Указывается в настройках интеграции)
        /// </summary>
        public static string RoistatPassword
        {
            get { return ModuleSettingsProvider.GetSettingValue<string>("RoistatPassword", Roistat.ModuleId); }
            set { ModuleSettingsProvider.SetSettingValue("RoistatPassword", value, Roistat.ModuleId); }
        }
    }
}
