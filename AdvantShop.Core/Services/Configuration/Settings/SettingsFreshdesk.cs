namespace AdvantShop.Configuration
{
    public class SettingsFreshdesk
    {
        public static string FreshdeskDomain
        {
            get { return SettingProvider.Items["FreshdeskDomain"]; }
            set { SettingProvider.Items["FreshdeskDomain"] = value; }
        }


        public static string FreshdeskWidgetCode
        {
            get {
                return string.Format(
                  "<iframe src=\"{0}/api/freshdesk?email={{{{requester.email}}}}\" width=\"100%\" height=\"350\" frameborder=\"0\" scrolling=\"yes\"></iframe>",
                  SettingsMain.SiteUrl.Replace("http://", "https://"));
            }
        }

    }
}