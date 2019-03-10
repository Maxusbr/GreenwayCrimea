using System;
using AdvantShop.Localization;
using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.App.Landing.Models.LandingAdmin
{
    public class LandingsAdminFilterModel : BaseFilterModel
    {
        public bool? Enabled { get; set; }
        public string CreatedDateFrom { get; set; }
        public string CreatedDateTo { get; set; }
    }

    public class LandingsFilterResult : FilterResult<LandingAdminItemModel>
    {

    }


    public partial class LandingAdminItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Enabled { get; set; }
        public string Url { get; set; }
        public string Template { get; set; }

        public int ProductsCount { get; set; }
        public int Visitors { get; set; }
        public int Views { get; set; }
        public int Goals { get; set; }
        public int Conversion { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateFormatted { get { return Culture.ConvertDate(CreatedDate); } }
    }
}
