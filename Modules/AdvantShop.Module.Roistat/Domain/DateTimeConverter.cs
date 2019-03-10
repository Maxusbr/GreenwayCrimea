using System;

namespace AdvantShop.Module.Roistat.Domain
{
    internal static class DateTimeConverter
    {
        public static DateTime JavaTimeStampToDateTime(double javaTimeStamp)
        {
            var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dt = dt.AddMilliseconds(javaTimeStamp).ToLocalTime();
            return dt;
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            var dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dt = dt.AddSeconds(unixTimeStamp).ToLocalTime();
            return dt;
        }
    }
}
