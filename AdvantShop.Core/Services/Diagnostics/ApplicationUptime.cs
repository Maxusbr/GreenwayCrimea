//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

namespace AdvantShop.Diagnostics
{
    public class ApplicationUptime
    {
        private static DateTime _applicationStartTime;

        public static void SetApplicationStartTime()
        {
            _applicationStartTime = DateTime.Now;
        }

        public static TimeSpan GetUptime()
        {
            return (DateTime.Now - _applicationStartTime);
        }
    }
}