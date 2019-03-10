//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

namespace AdvantShop.Mails
{
    public class MailFormat
    {
        public int MailFormatId { get; set; }

        public string FormatName { get; set; }

        public string FormatSubject { get; set; }

        public string FormatText { get; set; }

        public int MailFormatTypeId { get; set; }

        public int SortOrder { get; set; }

        public bool Enable { get; set; }

        public DateTime AddDate { get; set; }

        public DateTime ModifyDate { get; set; }
    }
}
