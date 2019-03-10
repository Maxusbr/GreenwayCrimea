using System.Collections.Generic;
using AdvantShop.Customers;

namespace AdvantShop.ViewModel.Managers
{
    public class DepartmentsListViewModel
    {
        public List<Department> Departments { get; set; }

        public int Selected { get; set; }
    }
}