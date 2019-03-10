using AdvantShop.Customers;
using AdvantShop.Web.Admin.Models.Departments;

namespace AdvantShop.Web.Admin.Handlers.Departments
{
    public class GetDepartmentModel
    {
        private readonly Department _department;

        public GetDepartmentModel(Department department)
        {
            _department = department;
        }

        public AdminDepartmentModel Execute()
        {
            var model = new AdminDepartmentModel
            {
                DepartmentId = _department.DepartmentId,
                Name = _department.Name,
                Sort = _department.Sort,
                Enabled = _department.Enabled,
            };

            return model;
        }
    }
}
