using System;

namespace AdvantShop.Web.Admin.Models.Common
{
    public class CustomerFiledFilterModel
    {
        public string Value { get; set; }
        public string ValueExact { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}