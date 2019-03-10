using System.Collections.Generic;
using AdvantShop.CMS;
using AdvantShop.Customers;

namespace AdvantShop.ViewModel.Managers
{
    public class ManagersViewModel
    {
        public DepartmentsListViewModel Departments { get; set; }

        public List<Manager> Managers { get; set; }

        public List<BreadCrumbs> BreadCrumbs { get; set; }
    }
}