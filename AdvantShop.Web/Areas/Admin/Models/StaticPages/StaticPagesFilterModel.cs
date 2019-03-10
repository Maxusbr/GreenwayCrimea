using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.StaticPages
{
    public class StaticPagesFilterModel : BaseFilterModel
    {
        public int StaticPageId { get; set; }
        public string PageName { get; set; }
        public bool? Enabled { get; set; }
        public int? ParentId { get; set; }
        public string ModifyDateFrom { get; set; }
        public string ModifyDateTo { get; set; }
        public int? SortOrder { get; set; }
        public int? Selected { get; set; }
    }
}
