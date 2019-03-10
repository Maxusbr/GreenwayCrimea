using System;
using AdvantShop.Configuration;
using System.Globalization;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Catalog
{
    public class DiscountByTimeService
    {
        #region Fields

        public static bool Enabled
        {
            get { return Convert.ToBoolean(SettingProvider.Items["SettingsDiscountByTime_Enabled"]); }
            set { SettingProvider.Items["SettingsDiscountByTime_Enabled"] = value.ToString(); }
        }

        public static DateTime FromDateTime
        {
            get 
            {
                DateTime dateTime;
                DateTime.TryParse(SettingProvider.Items["SettingsDiscountByTime_FromDateTime"], CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
                return dateTime;
            }
            set { SettingProvider.Items["SettingsDiscountByTime_FromDateTime"] = value.ToString(CultureInfo.InvariantCulture); }
        }

        public static DateTime ToDateTime
        {
            get
            {
                DateTime dateTime;
                DateTime.TryParse(SettingProvider.Items["SettingsDiscountByTime_ToDateTime"], CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
                return dateTime;
            }
            set { SettingProvider.Items["SettingsDiscountByTime_ToDateTime"] = value.ToString(CultureInfo.InvariantCulture); }
        }

        public static float DiscountByTime
        {
            get
            {
                float discountByTime = 0;
                float.TryParse(SettingProvider.Items["SettingsDiscountByTime_DiscountByTime"], out discountByTime);
                return discountByTime;
            }
            set { SettingProvider.Items["SettingsDiscountByTime_DiscountByTime"] = value.ToString("#0.00") ?? "0.00"; }
        }

        public static string PopupText
        {
            get { return SettingProvider.Items["SettingsDiscountByTime_PopupText"]; }
            set { SettingProvider.Items["SettingsDiscountByTime_PopupText"] = value; }
        }

        public static bool ShowPopup
        {
            get { return Convert.ToBoolean(SettingProvider.Items["SettingsDiscountByTime_ShowPopup"]); }
            set { SettingProvider.Items["SettingsDiscountByTime_ShowPopup"] = value.ToString(); }
        }

        #endregion

        public static float GetDiscountByTime()
        {
            if (!Enabled)
                return 0;

            return DateTime.Now.TimeBetween(FromDateTime, ToDateTime)
                       ? DiscountByTime
                       : 0;
        }
    }
}