//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Configuration;

namespace AdvantShop.Core.Services.Configuration.Settings
{
    public class SettingsMobile
    {
        public static bool IsMobileTemplateActive
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsMobileTemplateActive"]); }
            set { SettingProvider.Items["IsMobileTemplateActive"] = value.ToString(); }
        }

        public static int MainPageProductsCount
        {
            get { return Convert.ToInt32(SettingProvider.Items["Mobile_MainPageProductsCount"]); }
            set { SettingProvider.Items["Mobile_MainPageProductsCount"] = value.ToString(); }
        }

        public static bool DisplayCity
        {
            get { return Convert.ToBoolean(SettingProvider.Items["Mobile_DisplayCity"]); }
            set { SettingProvider.Items["Mobile_DisplayCity"] = value.ToString(); }
        }

        public static bool DisplaySlider
        {
            get { return Convert.ToBoolean(SettingProvider.Items["Mobile_DisplaySlider"]); }
            set { SettingProvider.Items["Mobile_DisplaySlider"] = value.ToString(); }
        }

        public static bool DisplayHeaderTitle
        {
            get { return Convert.ToBoolean(SettingProvider.Items["Mobile_DisplayHeaderTitle"]); }
            set { SettingProvider.Items["Mobile_DisplayHeaderTitle"] = value.ToString(); }
        }

        public static string HeaderCustomTitle
        {
            get { return SettingProvider.Items["Mobile_HeaderCustomTitle"]; }
            set { SettingProvider.Items["Mobile_HeaderCustomTitle"] = value; }
        }

        public static bool IsFullCheckout
        {
            get { return Convert.ToBoolean(SettingProvider.Items["Mobile_IsFullCheckout"]); }
            set { SettingProvider.Items["Mobile_IsFullCheckout"] = value.ToString(); }
        }

        public static bool RedirectToSubdomain
        {
            get { return Convert.ToBoolean(SettingProvider.Items["Mobile_RedirectToSubdomain"]); }
            set { SettingProvider.Items["Mobile_RedirectToSubdomain"] = value.ToString(); }
        }
    }
}
