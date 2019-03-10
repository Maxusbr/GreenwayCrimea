using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.News
{
    public class NewsFilterModel : BaseFilterModel
    {
        public string Title { get; set; }

        public bool? ShowOnMainPage { get; set; }

        public bool? Enabled { get; set; }

        public int? NewsCategoryId { get; set; }

        public string AddingDateFrom { get; set; }

        public string AddingDateTo { get; set; }
    }
}
