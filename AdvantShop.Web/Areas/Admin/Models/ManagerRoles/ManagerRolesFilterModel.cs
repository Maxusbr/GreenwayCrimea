using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.ManagerRoles
{
    public class ManagerRolesFilterModel : BaseFilterModel<int>
    {
        public string Name { get; set; }
    }
}
