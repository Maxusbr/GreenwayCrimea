using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.Modules
{
    public class ModulesFilterModel : BaseFilterModel
    {
        public string Name { get; set; }

        public ModulesPreFilterType FilterBy { get; set; }
    }
}
