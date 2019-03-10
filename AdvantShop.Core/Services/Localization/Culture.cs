//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Globalization;
using System.Threading;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Localization
{

    public class Culture
    {
        public enum SupportLanguage
        {
            Russian = 0,
            English = 1,
            Ukrainian = 2,
            Other = 3
        }

        //private static string GetStringLangByEnum(SupportLanguage lang)
        //{
        //    switch (lang)
        //    {
        //        case SupportLanguage.English:
        //            return "en-US";
        //        case SupportLanguage.Russian:
        //            return "ru-RU";
        //        case SupportLanguage.Ukrainian:
        //            return "uk-UA";
        //        default:
        //            return "ru-RU";
        //    }
        //}

        public static SupportLanguage Language
        {
            get
            {
                switch (SettingsMain.Language)
                {
                    case "en":
                    case "en-US":
                        return SupportLanguage.English;
                    case "ru":
                    case "ru-RU":
                        return SupportLanguage.Russian;
                    case "uk":
                    case "uk-UA":
                        return SupportLanguage.Ukrainian;
                    default:
                        return SupportLanguage.Other;
                }
            }
            //set
            //{
            //    SettingsMain.Language = GetStringLangByEnum(value);
            //    InitializeCulture();
            //}
        }

        public static void InitializeCulture()
        {
            Thread.CurrentThread.SetCulture();
        }

        public static void InitializeCulture(string langValue)
        {
            var lang = langValue;
            if (string.IsNullOrEmpty(lang)) return;
            Thread.CurrentThread.SetCulture(langValue);
        }

        public static CultureInfo CurrentCulture(string lang = "")
        {
            if (string.IsNullOrWhiteSpace(lang))
            {
                return new CultureInfo(SettingsMain.Language);
            }
            return new CultureInfo(lang);
        }

        public static string ConvertDate(DateTime d)
        {
            try
            {
                return d.ToString(SettingsMain.AdminDateFormat);
            }
            catch (FormatException)
            {
                return d.ToString(CultureInfo.CurrentCulture);
            }
        }

        public static string ConvertDateWithoutSeconds(DateTime d)
        {
            return d.ToString(SettingsMain.AdminDateFormat.Replace(":ss", ""));
        }


        public static string ConvertShortDate(DateTime d)
        {
            try
            {
                return d.ToString(SettingsMain.ShortDateFormat);
            }
            catch (FormatException)
            {
                return d.ToShortDateString();
            }
        }

        public static string ConvertDateFromString(string s)
        {
            var d = DateTime.Parse(s, CultureInfo.GetCultureInfo(SettingsMain.Language));  // GetStringLangByEnum(Language)
            return d.ToString(SettingsMain.AdminDateFormat);
        }
    }
}
