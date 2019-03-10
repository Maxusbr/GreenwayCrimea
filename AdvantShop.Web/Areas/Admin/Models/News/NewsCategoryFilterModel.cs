using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.News
{
    public class NewsCategoryFilterModel : BaseFilterModel
    {
        public string NewsCategoryId { get; set; }
        
        public string Name { get; set; }

        public int SortOrder { get; set; }

        public string SortOrderFrom { get; set; }

        public string SortOrderTo { get; set; }
        
        public string UrlPath { get; set; }
    }
}