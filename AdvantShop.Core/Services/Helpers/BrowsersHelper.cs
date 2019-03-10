//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;
using System.Web;

namespace AdvantShop.Helpers
{
    public class BrowsersHelper
    {
        const string urlOldBrowsersPage = "~/content/oldBrowser/default.aspx";

        public static Dictionary<string, int> SupportedBrowsers = new Dictionary<string, int>() {
            {"IE", 10},
        };

        public static bool IsSupportedBrowser()
        {
            HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;

            return SupportedBrowsers.ContainsKey(browser.Browser) ? SupportedBrowsers[browser.Browser] <= browser.MajorVersion : true;
        }

        public static void CheckSupportedBrowser()
        {
            HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;

            if (!IsSupportedBrowser())
            {
                RedirectToOldBrowsersPage();
            }
        }

        public static void RedirectToOldBrowsersPage()
        {
            HttpContext.Current.Response.Redirect(urlOldBrowsersPage, true);
        }

        public static bool IsBotByIp(string ip)
        {
            return BotsIPs.Contains(ip);
        }

        public static List<string> BotsIPs = new List<string>() {
            "10.0.2.33",
            "130.193.50.24",
            "130.193.51.73",
            "141.8.132.6",
            "141.8.142.10",
            "141.8.142.187",
            "141.8.142.55",
            "141.8.142.84",
            "141.8.183.21",
            "141.8.184.30",
            "144.76.101.171",
            "176.121.14.111",
            "176.121.14.136",
            "178.154.189.16",
            "178.154.189.8",
            "192.168.0.10",
            "194.28.84.75",
            "213.180.206.196",
            "37.140.192.131",
            "37.140.192.132",
            "37.18.74.162",
            "37.18.74.46",
            "37.18.77.115",
            "37.18.77.148",
            "37.18.77.20",
            "37.18.77.242",
            "37.18.77.247",
            "37.18.77.72",
            "37.230.152.116",
            "37.230.152.206",
            "37.230.153.125",
            "37.230.153.173",
            "37.230.154.105",
            "37.230.154.118",
            "37.230.154.186",
            "37.230.154.70",
            "37.230.154.71",
            "37.9.118.28",
            "46.4.89.220",
            "5.255.253.52",
            "5.255.253.74",
            "5.61.41.104",
            "5.79.68.55",
            "68.180.230.45",
            "81.177.143.251",
            "81.177.174.9",
            "89.169.36.32",
            "91.204.252.232",
            "95.108.129.196",
            "95.108.213.24"
        };
    }
}
