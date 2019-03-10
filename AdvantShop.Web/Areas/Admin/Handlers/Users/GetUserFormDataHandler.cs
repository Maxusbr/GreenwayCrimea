using System;
using System.Linq;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.Saas;
using AdvantShop.Trial;
using AdvantShop.Web.Admin.Models;
using AdvantShop.Web.Infrastructure.Handlers;

namespace AdvantShop.Web.Admin.Handlers.Users
{
    public class GetUserFormDataHandler : ICommandHandler<object>
    {
        private readonly Guid? _customerId;

        public GetUserFormDataHandler(Guid? customerId)
        {
            _customerId = customerId;
        }

        public object Execute()
        {
            var currentCustomer = CustomerContext.CurrentCustomer;

            var departments = DepartmentService.GetDepartmentsList().Where(x => x.Enabled)
                .Select(x => new SelectItemModel(x.Name, x.DepartmentId.ToString())).ToList();

            // исключая редактируемого сотрудника
            var users = CustomerService.GetCustomersByRoles(Role.Administrator, Role.Moderator)
                .Where(x => !_customerId.HasValue || x.Id != _customerId.Value)
                .Select(x => new SelectItemModel(StringHelper.AggregateStrings(" ", x.LastName, x.FirstName), x.Id.ToString()))
                .OrderBy(x => x.label).ToList();

            var roleActionKeys = new GetUserRoleActions(_customerId).Execute();

            return new
            {
                departments,
                users,
                roles = ManagerRoleService.GetManagerRoles(),
                roleActionKeys,
                managersAvailable = !SaasDataService.IsSaasEnabled || ManagerService.GetManagersCount() < SaasDataService.CurrentSaasData.EmployeesCount,
                isAdmin = currentCustomer.IsAdmin,
                trialEnabled = TrialService.IsTrialEnabled,
                moderatorsAvailable = !SaasDataService.IsSaasEnabled || SaasDataService.CurrentSaasData.RoleActions
            };

        }
    }
}
