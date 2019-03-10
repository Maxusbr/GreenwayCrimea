using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Configuration
{
    public class SettingsNotifications
    {
        public static bool WebNotificationInNewTab
        {
            get { return SettingProvider.Items["Notifications.WebNotification.InNewTab"].TryParseBool(); }
            set { SettingProvider.Items["Notifications.WebNotification.InNewTab"] = value.ToString(); }
        }

        public static bool ShowCookiesPolicyMessage
        {
            get { return SettingProvider.Items["CustomersNotifications.ShowCookiesPolicyMessage"].TryParseBool(); }
            set { SettingProvider.Items["CustomersNotifications.ShowCookiesPolicyMessage"] = value.ToString(); }
        }

        public static string CookiesPolicyMessage
        {
            get { return SettingProvider.Items["CustomersNotifications.CookiesPolicyMessage"]; }
            set { SettingProvider.Items["CustomersNotifications.CookiesPolicyMessage"] = value; }
        }

        public static string CookiesPolicyMessageFormatted
        {
            get { return CookiesPolicyMessage.DefaultOrEmpty().Replace("#STORE_URL#", SettingsMain.SiteUrlPlain); }
        }
    }
}