//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

namespace AdvantShop.Statistic
{
    public class ApplicationLogEntry
    {
        public int EntryId { get; set; }
        public DateTime EntryDate { get; set; }
        public string ServerIp { get; set; }
        public string ServerName { get; set; }
    }
}