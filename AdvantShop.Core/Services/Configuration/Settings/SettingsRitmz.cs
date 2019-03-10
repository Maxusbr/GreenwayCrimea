//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Configuration
{
    public class SettingsRitmz
    {
        public static string RitmzLogin
        {
            get { return SettingProvider.Items["RitmzLogin"]; }
            set { SettingProvider.Items["RitmzLogin"] = value; }
        }
        public static string RitmzPassword
        {
            get { return SettingProvider.Items["RitmzPassword"]; }
            set { SettingProvider.Items["RitmzPassword"] = value; }
        }
    }
}
