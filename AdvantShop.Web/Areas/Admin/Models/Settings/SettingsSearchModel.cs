using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.Settings
{
    public class SettingsSearchModel : BaseFilterModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string KeyWords { get; set; }
        public string Link { get; set; }
        public int SortOrder { get; set; }

    }
}