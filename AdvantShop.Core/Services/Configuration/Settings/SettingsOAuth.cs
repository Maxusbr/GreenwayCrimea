//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using AdvantShop.Helpers;

namespace AdvantShop.Configuration
{
    public class SettingsOAuth
    {

        public static bool YandexActive
        {
            get { return SQLDataHelper.GetBoolean(SettingProvider.Items["OpenIdProviderYandexActive"]); }
            set { SettingProvider.Items["OpenIdProviderYandexActive"] = value.ToString(); }
        }
        public static bool GoogleActive
        {
            get { return SQLDataHelper.GetBoolean(SettingProvider.Items["OpenIdProviderGoogleActive"]); }
            set { SettingProvider.Items["OpenIdProviderGoogleActive"] = value.ToString(); }
        }

        public static bool MailActive
        {
            get { return SQLDataHelper.GetBoolean(SettingProvider.Items["OpenIdProviderMailActive"]); }
            set { SettingProvider.Items["OpenIdProviderMailActive"] = value.ToString(); }
        }

        public static bool VkontakteActive
        {
            get { return SQLDataHelper.GetBoolean(SettingProvider.Items["OpenIdProviderVkontakteActive"]); }
            set { SettingProvider.Items["OpenIdProviderVkontakteActive"] = value.ToString(); }
        }

        public static bool FacebookActive
        {
            get { return SQLDataHelper.GetBoolean(SettingProvider.Items["OpenIdProviderFacebookActive"]); }
            set { SettingProvider.Items["OpenIdProviderFacebookActive"] = value.ToString(); }
        }

        public static bool OdnoklassnikiActive
        {
            get { return SQLDataHelper.GetBoolean(SettingProvider.Items["OpenIdProviderOdnoklassnikiActive"]); }
            set { SettingProvider.Items["OpenIdProviderOdnoklassnikiActive"] = value.ToString(); }
        }

        public static string FacebookClientId
        {
            get { return SettingProvider.Items["oidFacebookClientId"]; }
            set { SettingProvider.Items["oidFacebookClientId"] = value; }
        }

        public static string FacebookApplicationSecret
        {
            get { return SettingProvider.Items["oidFacebookApplicationSecret"]; }
            set { SettingProvider.Items["oidFacebookApplicationSecret"] = value; }
        }

        public static string VkontakeClientId
        {
            get { return SettingProvider.Items["oidVkontakeClientId"]; }
            set { SettingProvider.Items["oidVkontakeClientId"] = value; }
        }

        public static string VkontakeSecret
        {
            get { return SettingProvider.Items["oidVkontakeSecret"]; }
            set { SettingProvider.Items["oidVkontakeSecret"] = value; }
        }

        public static string OdnoklassnikiClientId
        {
            get { return SettingProvider.Items["oidOdnoklassnikiClientId"]; }
            set { SettingProvider.Items["oidOdnoklassnikiClientId"] = value; }
        }

        public static string OdnoklassnikiPublicApiKey
        {
            get { return SettingProvider.Items["oidOdnoklassnikiPublicApiKey"]; }
            set { SettingProvider.Items["oidOdnoklassnikiPublicApiKey"] = value; }
        }

        public static string OdnoklassnikiSecret
        {
            get { return SettingProvider.Items["oidOdnoklassnikiSecret"]; }
            set { SettingProvider.Items["oidOdnoklassnikiSecret"] = value; }
        }

        public static string GoogleClientId
        {
            get { return SettingProvider.Items["oidGoogleClientId"]; }
            set { SettingProvider.Items["oidGoogleClientId"] = value; }
        }

        public static string GoogleClientSecret
        {
            get { return SettingProvider.Items["oidGoogleClientSecret"]; }
            set { SettingProvider.Items["oidGoogleClientSecret"] = value; }
        }

        public static string GoogleAnalyticsAccessToken
        {
            get { return SettingProvider.Items["GoogleAnalyticsAccessToken"]; }
            set { SettingProvider.Items["GoogleAnalyticsAccessToken"] = value; }
        }


        public static string YandexClientId
        {
            get { return SettingProvider.Items["oauthYandexClientId"]; }
            set { SettingProvider.Items["oauthYandexClientId"] = value; }
        }
        public static string YandexClientSecret
        {
            get { return SettingProvider.Items["oauthYandexClientSecret"]; }
            set { SettingProvider.Items["oauthYandexClientSecret"] = value; }
        }


        public static string MailAppId
        {
            get { return SettingProvider.Items["oauthMailAppId"]; }
            set { SettingProvider.Items["oauthMailAppId"] = value; }
        }
        public static string MailClientId
        {
            get { return SettingProvider.Items["oauthMailClientId"]; }
            set { SettingProvider.Items["oauthMailClientId"] = value; }
        }
        public static string MailClientSecret
        {
            get { return SettingProvider.Items["oauthMailClientSecret"]; }
            set { SettingProvider.Items["oauthMailClientSecret"] = value; }
        }

        public static bool IsOauthEnabled()
        {
            return FacebookActive || GoogleActive || MailActive || OdnoklassnikiActive || VkontakteActive || YandexActive;
        }

    }
}