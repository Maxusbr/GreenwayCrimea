//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Saas;

namespace AdvantShop.Configuration
{
    public class SettingsLandingPage
    {
        public static bool ActiveLandingPage
        {
            get
            {
                return Convert.ToBoolean(SettingProvider.Items["ActiveLandingPage"]) &&
                       (!SaasDataService.IsSaasEnabled ||
                        (SaasDataService.IsSaasEnabled && SaasDataService.CurrentSaasData.LandingPage));
            }
            set { SettingProvider.Items["ActiveLandingPage"] = value.ToString(); }
        }

        public static string LandingPageCommonStatic
        {
            get { return SettingProvider.Items["LandingPageCommonStatic"]; }
            set { SettingProvider.Items["LandingPageCommonStatic"] = value; }
        }
    }
}