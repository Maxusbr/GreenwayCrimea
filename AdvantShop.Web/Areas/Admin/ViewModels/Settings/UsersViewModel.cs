
namespace AdvantShop.Web.Admin.ViewModels.Settings
{
    public class UsersViewModel
    {
        public bool ManagersLimitation { get; set; }
        public int ManagersCount { get; set; }
        public int ManagersLimit { get; set; }

        public int EmployeesCount { get; set; }
        public int EmployeesLimit { get; set; }
        public bool EnableEmployees { get; set; }

        public bool ShowManagersPage { get; set; }
        public bool EnableManagersModule { get; set; }

    }
}
