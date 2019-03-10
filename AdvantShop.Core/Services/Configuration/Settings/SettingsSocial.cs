//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

namespace AdvantShop.Configuration
{
    public class SettingsSocial
    {
        public static bool SocialShareCustomEnabled
        {

            get { return bool.Parse(SettingProvider.Items["SocialShareCustomEnabled"]); }
            set { SettingProvider.Items["SocialShareCustomEnabled"] = value.ToString(); }
        }

        public static string SocialShareCustomCode
        {

            get { return SettingProvider.Items["SocialShareCustomCode"]; }
            set { SettingProvider.Items["SocialShareCustomCode"] = value; }
        }

        public static bool SocialShareEnabled
        {

            get { return bool.Parse(SettingProvider.Items["SocialShareEnabled"]); }
            set { SettingProvider.Items["SocialShareEnabled"] = value.ToString(); }
        }
    }
}