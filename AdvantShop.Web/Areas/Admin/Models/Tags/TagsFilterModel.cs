using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.Tags
{
    public class TagsFilterModel : BaseFilterModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public bool? Enabled { get; set; }
    }
}
