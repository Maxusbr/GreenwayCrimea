using AdvantShop.Customers;
using AdvantShop.Web.Admin.Models.Users;

namespace AdvantShop.Web.Admin.Handlers.Users
{
    public class GetUserModel
    {
        private readonly Customer _customer;

        public GetUserModel(Customer customer)
        {
            _customer = customer;
        }

        public AdminUserModel Execute()
        {
            var model = new AdminUserModel
            {
                CustomerId = _customer.Id,
                CustomerRole = _customer.CustomerRole,
                Email = _customer.EMail,
                FirstName = _customer.FirstName,
                LastName = _customer.LastName,
                Phone = _customer.Phone,
                Avatar = _customer.Avatar,
                Enabled = _customer.Enabled,
                HeadCustomerId = _customer.HeadCustomerId,
                BirthDay = _customer.BirthDay,
                City = _customer.City,
                EditHimself = CustomerContext.CustomerId == _customer.Id
            };
            var associatedManager = ManagerService.GetManager(_customer.Id);
            if (associatedManager != null)
            {
                model.AssociatedManagerId = associatedManager.ManagerId;
                model.DepartmentId = associatedManager.DepartmentId;
                if (model.DepartmentId.HasValue)
                {
                    var department = DepartmentService.GetDepartment(model.DepartmentId.Value);
                    model.DepartmentName = department != null ? department.Name : string.Empty;
                }
                model.Position = associatedManager.Position;
            }

            return model;
        }
    }
}
