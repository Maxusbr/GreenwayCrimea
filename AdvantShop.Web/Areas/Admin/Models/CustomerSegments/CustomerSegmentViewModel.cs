using System;
using AdvantShop.Localization;

namespace AdvantShop.Web.Admin.Models.CustomerSegments
{
    public class CustomerSegmentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateFormatted { get { return Culture.ConvertDateWithoutSeconds(CreatedDate); } }
        public int CustomersCount { get; set; }
    }
}
