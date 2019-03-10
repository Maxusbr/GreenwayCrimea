using System;
using System.Text;
using AdvantShop.Core.Services.Localization;

namespace AdvantShop.Core.Common.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string ToReadableString(this TimeSpan ts)
        {
            var sb = new StringBuilder();
            if (ts.Days > 0)
                sb.AppendFormat("{0} {1} ", ts.Days, LocalizationService.GetResource("Core.Numerals.Days.Short"));
            if (ts.Hours > 0)
                sb.AppendFormat("{0} {1} ", ts.Hours, LocalizationService.GetResource("Core.Numerals.Hours.Short"));
            if (ts.Minutes > 0)
                sb.AppendFormat("{0} {1} ", ts.Minutes, LocalizationService.GetResource("Core.Numerals.Minutes.Short"));
            if (ts.Seconds > 0)
                sb.AppendFormat("{0} {1}", ts.Seconds, LocalizationService.GetResource("Core.Numerals.Seconds.Short"));
            return sb.ToString().Trim(' ');
        }
    }
}