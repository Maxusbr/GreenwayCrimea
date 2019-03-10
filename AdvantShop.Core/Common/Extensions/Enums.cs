//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
//using Resources;
using AdvantShop.Core.Services.Localization;

namespace AdvantShop.Core.Common.Extensions
{
    public static class Enums
    {
        [Obsolete("Use Localize method")]
        public static string GetLocalizedName(this Enum val)
        {
            return LocalizationService.GetResource(string.Format("Enums_{0}_{1}", val.GetType().Name, val)).Default(val.ToString());  //Resource.ResourceManager.GetString(string.Format("Enums_{0}_{1}", val.GetType().Name, val)).Default(val.ToString());
        }

        [Obsolete("Use Localize method")]
        public static IEnumerable<string> GetLocalizedNames(this Enum val)
        {
            return new List<string>();
            //var resourceManager = Resource.ResourceManager;
            //var type = val.GetType();
            //return from object value in Enum.GetValues(type) select resourceManager.GetString(string.Format("Enums_{0}_{1}", type.Name, value));
        }

        public static IEnumerable<string> GetValues(this Enum val)
        {
            var type = val.GetType();
            return from object value in Enum.GetValues(type) select value.ToString();
        }

        public static T Parse<T>(this string strType)
        {
            return strType.Parse(default(T));
        }

        public static T Parse<T>(this string strType, T defaultValue)
        {
            if (string.IsNullOrWhiteSpace(strType))
                return defaultValue;

            var strTypeFixed = strType.Replace(' ', '_').Replace('-', '_');
            if (Enum.IsDefined(typeof(T), strTypeFixed))
            {
                return (T)Enum.Parse(typeof(T), strTypeFixed, true);
            }
            foreach (var value in Enum.GetNames(typeof(T)))
            {
                if (!value.Equals(strTypeFixed, StringComparison.OrdinalIgnoreCase)) continue;
                return (T)Enum.Parse(typeof(T), value);
            }
            foreach (var value in Enum.GetValues(typeof(T)))
            {
                if (!((int)value).ToString().Equals(strTypeFixed, StringComparison.OrdinalIgnoreCase)) continue;
                return (T)value;
            }
            return default(T);
        }
        
        public static string ConvertIntString(this Enum e)
        {
            return Convert.ToInt16(e).ToString(CultureInfo.InvariantCulture);
        }
    }
}