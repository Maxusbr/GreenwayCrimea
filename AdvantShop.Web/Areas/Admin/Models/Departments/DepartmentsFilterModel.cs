using AdvantShop.Web.Infrastructure.Admin;

namespace AdvantShop.Web.Admin.Models.Departments
{
    public class DepartmentsFilterModel : BaseFilterModel<int>
    {
        public bool? Enabled { get; set; }
    }
}
