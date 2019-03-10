//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Globalization;
using AdvantShop.Helpers;
using AdvantShop.Permission;
using AdvantShop.Core.Common.Extensions;
using System;
using AdvantShop.Diagnostics;

namespace AdvantShop.Configuration
{
    public class SettingsLic
    {
        public static string LicKey
        {
            get { return SettingProvider.Items["LicKey"]; }
            set { SettingProvider.Items["LicKey"] = value; }
        }

        public static bool ActiveLic
        {
            get { return SQLDataHelper.GetBoolean(SettingProvider.Items["ActiveLic"]); }
            set { SettingProvider.Items["ActiveLic"] = value.ToString(CultureInfo.InvariantCulture); }
        }

        public static string ClientCode
        {
            get { return SettingProvider.Items["ClientCode"]; }
            set { SettingProvider.Items["ClientCode"] = value; }
        }


        public static bool ShowAdvantshopJivoSiteForm
        {
            get { return SQLDataHelper.GetBoolean(SettingProvider.Items["ShowAdvantshopJivoSiteForm"]); }
            set { SettingProvider.Items["ShowAdvantshopJivoSiteForm"] = value.ToString(CultureInfo.InvariantCulture); }
        }

        public static bool Activate(string key = null)
        {
            try
            {
                var accountNumber = PermissionAccsess.ActiveLic(key ?? LicKey, SettingsMain.SiteUrl,
                                                                    SettingsMain.ShopName, SettingsGeneral.SiteVersion, SettingsGeneral.SiteVersionDev);

                if (accountNumber.IsNotEmpty() && accountNumber != "false")
                {
                    ClientCode = accountNumber;
                    if (key != null)
                        LicKey = key;
                    ActiveLic = true;
                    return true;
                }
                ActiveLic = false;
                return false;
            }
            catch (Exception ex)
            {
                Debug.Log.Error("Error at license check", ex);
                return true;
            }
        }

        public static string BasePlatformUrl
        {
            get
            {
                return SettingProvider.GetConfigSettingValue("BasePlatformUrl");
            }
        }

        public static string AccountPlatformUrl
        {
            get
            {
                return SettingProvider.GetConfigSettingValue("AccountPlatformUrl");
            }
        }



        
    }
}