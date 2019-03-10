//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Localization;

namespace AdvantShop.Diagnostics
{
    public class LogEntry
    {
        // Obsolete
        public string TimeStamp { get; set; }


        public DateTime DateTime { get; set; }
        public string DateTimeFormatted { get { return Culture.ConvertDate(DateTime); } }

        public string Level { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }

        public string ErrorMessage { get; set; }
    }
}