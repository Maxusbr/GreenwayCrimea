//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Configuration
{
    public enum CaptchaMode
    {
        [Localize("Core.Configuration.CaptchaMode.Numeric")]
        Numeric = 0,
        
        [Localize("Core.Configuration.CaptchaMode.AlphaNumericEn")]
        AlphaNumericEn = 1,

        [Localize("Core.Configuration.CaptchaMode.AlphaNumericRu")]
        AlphaNumericRu = 2,
    }

    public class SettingsMain
    {
        public static bool EnableInplace
        {
            get { return Convert.ToBoolean(SettingProvider.Items["EnableInplace"]); }
            set { SettingProvider.Items["EnableInplace"] = value.ToString(); }
        }

        public static bool EnablePhoneMask
        {
            get { return Convert.ToBoolean(SettingProvider.Items["EnablePhoneMask"]); }
            set { SettingProvider.Items["EnablePhoneMask"] = value.ToString(); }
        }

        public static bool EnableCaptcha
        {
            get { return Convert.ToBoolean(SettingProvider.Items["EnableCheckOrderConfirmCode"]); }
            set { SettingProvider.Items["EnableCheckOrderConfirmCode"] = value.ToString(); }
        }

        public static bool EnableCaptchaInCheckout
        {
            get { return Convert.ToBoolean(SettingProvider.Items["EnableCaptchaInCheckout"]); }
            set { SettingProvider.Items["EnableCaptchaInCheckout"] = value.ToString(); }
        }

        public static bool EnableCaptchaInRegistration
        {
            get { return Convert.ToBoolean(SettingProvider.Items["EnableCaptchaInRegistration"]); }
            set { SettingProvider.Items["EnableCaptchaInRegistration"] = value.ToString(); }
        }

        public static bool EnableCaptchaInPreOrder
        {
            get { return Convert.ToBoolean(SettingProvider.Items["EnableCaptchaInPreOrder"]); }
            set { SettingProvider.Items["EnableCaptchaInPreOrder"] = value.ToString(); }
        }

        public static bool EnableCaptchaInGiftCerticate
        {
            get { return Convert.ToBoolean(SettingProvider.Items["EnableCaptchaInGiftCerticate"]); }
            set { SettingProvider.Items["EnableCaptchaInGiftCerticate"] = value.ToString(); }
        }

        public static bool EnableCaptchaInFeedback
        {
            get { return Convert.ToBoolean(SettingProvider.Items["EnableCaptchaInFeedback"]); }
            set { SettingProvider.Items["EnableCaptchaInFeedback"] = value.ToString(); }
        }

        public static CaptchaMode CaptchaMode
        {
            get { return (CaptchaMode)Convert.ToInt32(SettingProvider.Items["CaptchaMode"]); }
            set { SettingProvider.Items["CaptchaMode"] = ((int)value).ToString(); }
        }

        public static int CaptchaLength
        {
            get { return Convert.ToInt32(SettingProvider.Items["CaptchaLength"]); }
            set { SettingProvider.Items["CaptchaLength"] = value.ToString(); }
        }



        public static bool EnableAutoUpdateCurrencies
        {
            get { return Convert.ToBoolean(SettingProvider.Items["EnableAutoUpdateCurrencies"]); }
            set { SettingProvider.Items["EnableAutoUpdateCurrencies"] = value.ToString(); }
        }

        public static string LogoImageName
        {
            get { return SettingProvider.Items["MainPageLogoFileName"]; }
            set
            {
                SettingProvider.Items["MainPageLogoFileName"] = value;
                IsDefaultLogo = false;
            }
        }

        public static string PreviewLogoImageName
        {
            get { return SettingProvider.Items["PreviewLogoImageName"]; }
            set { SettingProvider.Items["PreviewLogoImageName"] = value; }
        }

        public static bool IsDefaultLogo
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsDefaultLogo"]); }
            set { SettingProvider.Items["IsDefaultLogo"] = value.ToString(); }
        }

        public static string FaviconImageName
        {
            get { return SettingProvider.Items["MainFaviconFileName"]; }
            set { SettingProvider.Items["MainFaviconFileName"] = value; }
        }

        public static string SiteUrl
        {
            get { return SettingProvider.Items["ShopURL"].Trim('/'); }
            set { SettingProvider.Items["ShopURL"] = value.Trim('/'); }
        }

        public static bool IsStoreClosed
        {
            get { return Convert.ToBoolean(SettingProvider.Items["IsStoreClosed"]); }
            set { SettingProvider.Items["IsStoreClosed"] = value.ToString(); }
        }

        public static string SiteUrlPlain
        {
            get { return SiteUrl.Replace("http://", "").Replace("https://", "").Replace("www.", ""); }
        }


        public static string ShopName
        {
            get { return SettingProvider.Items["ShopName"]; }
            set { SettingProvider.Items["ShopName"] = value; }
        }

        public static string AdminShopName
        {
            get { return SettingProvider.Items["AdminShopName"]; }
            set { SettingProvider.Items["AdminShopName"] = value; }
        }

        public static string LogoImageAlt
        {
            get { return SettingProvider.Items["ImageAltText"]; }
            set { SettingProvider.Items["ImageAltText"] = value; }
        }

        public static string Language
        {
            get { return SettingProvider.Items["Language"]; }
            set { SettingProvider.Items["Language"] = value; }
        }

        public static string AdminDateFormat
        {
            get { return SettingProvider.Items["AdminDateFormat"]; }
            set { SettingProvider.Items["AdminDateFormat"] = value; }
        }

        public static string ShortDateFormat
        {
            get { return SettingProvider.Items["ShortDateFormat"]; }
            set { SettingProvider.Items["ShortDateFormat"] = value; }
        }

        public static int SellerCountryId
        {
            get { return int.Parse(SettingProvider.Items["SellerCountryId"]); }
            set { SettingProvider.Items["SellerCountryId"] = value.ToString(); }
        }

        public static int SellerRegionId
        {
            get { return int.Parse(SettingProvider.Items["SellerRegionId"]); }
            set { SettingProvider.Items["SellerRegionId"] = value.ToString(); }
        }

        public static string Phone
        {
            get { return SettingProvider.Items["Phone"]; }
            set { SettingProvider.Items["Phone"] = value; }
        }

        public static string MobilePhone
        {
            get { return SettingProvider.Items["MobilePhone"]; }
            set { SettingProvider.Items["MobilePhone"] = value; }
        }

        public static string City
        {
            get { return SettingProvider.Items["City"]; }
            set { SettingProvider.Items["City"] = value; }
        }

        public static string SearchPage
        {
            get { return SettingProvider.Items["SearchPage"]; }
            set { SettingProvider.Items["SearchPage"] = value; }
        }

        public static string SearchArea
        {
            get { return SettingProvider.Items["SearchArea"]; }
            set { SettingProvider.Items["SearchArea"] = value; }
        }

        public static string Achievements
        {
            get { return SettingProvider.Items["Achievements"]; }
            set { SettingProvider.Items["Achievements"] = value; }
        }

        public static string AchievementsPoints
        {
            get { return SettingProvider.Items["AchievementsPoints"]; }
            set { SettingProvider.Items["AchievementsPoints"] = value; }
        }

        public static string AchievementsDescription
        {
            get { return SettingProvider.Items["AchievementsDescription"]; }
            set { SettingProvider.Items["AchievementsDescription"] = value; }
        }

        public static string AchievementsPopUp
        {
            get { return SettingProvider.Items["AchievementsPopUp"]; }
            set { SettingProvider.Items["AchievementsPopUp"] = value; }
        }

        public static bool EnableCyrillicUrl
        {
            get { return Convert.ToBoolean(SettingProvider.Items["EnableCyrillicUrl"]); }
            set { SettingProvider.Items["EnableCyrillicUrl"] = value.ToString(); }
        }

        /// <summary>
        /// По умолчанию не логируем незначительные засоряющие лог ошибки. (сдек, едост и тд.)
        /// </summary>
        public static bool LogAllErrors
        {
            get { return Convert.ToBoolean(SettingProvider.Items["LogAllErrors"]); }
            set { SettingProvider.Items["LogAllErrors"] = value.ToString(); }
        }

        /// <summary>
        /// Текущий размер файлов хранилища
        /// </summary>
        public static long CurrentFilesStorageSize
        {
            get { return Convert.ToInt64(SettingProvider.Items["CurrentFilesStorageSize"]); }
            set { SettingProvider.Items["CurrentFilesStorageSize"] = value.ToString(); }
        }

        public static double CurrentFilesStorageSwTime
        {
            get { return Convert.ToDouble(SettingProvider.Items["CurrentFilesStorageSwTime"]); }
            set { SettingProvider.Items["CurrentFilesStorageSwTime"] = value.ToString(); }
        }

        public static DateTime CurrentFilesStorageLastUpdateTime
        {
            get { return SettingProvider.Items["CurrentFilesStorageLastUpdateTime"].TryParseDateTime(); }
            set { SettingProvider.Items["CurrentFilesStorageLastUpdateTime"] = value.ToString(); }
        }
    }
}