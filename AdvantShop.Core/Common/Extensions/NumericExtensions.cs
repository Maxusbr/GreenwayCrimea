using System.Globalization;

namespace AdvantShop.Core.Common.Extensions
{
    public static class NumericExtensions
    {
        public static string ToInvariantString(this float value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        public static string ToInvariantString(this float? value)
        {
            return value.ToInvariantString(0);
        }

        public static string ToInvariantString(this float? value, float def)
        {
            var v = value != null ? ((float)value) : def;
            return v.ToString(CultureInfo.InvariantCulture);
        }

        public static string ToInvariantString(this double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
