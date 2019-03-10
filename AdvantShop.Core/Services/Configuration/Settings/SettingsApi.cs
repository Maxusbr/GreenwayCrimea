//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Configuration;

namespace AdvantShop.Core.Services.Configuration.Settings
{
    public class SettingsApi
    {
        public static string ApiKey
        {
            get { return SettingProvider.Items["Api_ApiKey"]; }
            set { SettingProvider.Items["Api_ApiKey"] = value; }
        }
    }
}
