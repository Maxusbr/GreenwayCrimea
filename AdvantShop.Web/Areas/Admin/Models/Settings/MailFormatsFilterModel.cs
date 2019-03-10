using AdvantShop.Web.Infrastructure.Admin;
using System;


namespace AdvantShop.Web.Admin.Models.Settings
{
    public class MailFormatsFilterModel : BaseFilterModel
    {
        public int MailFormatID { get; set; }
        public string FormatName { get; set; }
        public int? MailFormatTypeId { get; set; }
        public string TypeName { get; set; }
        public int SortOrder { get; set; }
        public bool? Enable { get; set; }
    }
}
