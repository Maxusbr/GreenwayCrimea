using AdvantShop.Core.Services.Crm;
using AdvantShop.Customers;
using AdvantShop.Orders;
using AdvantShop.Web.Admin.Models.Tasks;

namespace AdvantShop.Web.Admin.Handlers.Tasks
{
    public class GetTaskModel
    {
        private readonly Customer _currentCustomer;
        private readonly Task _task;

        public GetTaskModel(Task task)
        {
            _currentCustomer = CustomerContext.CurrentCustomer;
            _task = task;
        }

        public TaskModel Execute()
        {
            var model = (TaskModel)_task;

            if (!_currentCustomer.IsModerator)
                return model;

            if (_task.OrderId.HasValue && (_task.Order == null || !_currentCustomer.HasRoleAction(RoleAction.Orders) || !OrderService.CheckAccess(_task.Order)))
            {
                model.OrderId = null;
                model.OrderNumber = null;
            }
            Lead lead;
            if (_task.LeadId.HasValue && 
                ((lead = LeadService.GetLead(_task.LeadId.Value)) == null || !_currentCustomer.HasRoleAction(RoleAction.Crm) || !LeadService.CheckAccess(lead)))
            {
                model.LeadId = null;
            }
            if (_task.CustomerId.HasValue && !_currentCustomer.HasRoleAction(RoleAction.Customers))
            {
                model.ClientCustomerId = null;
                model.ClientName = null;
            }
            if (_task.ReviewId.HasValue && !_currentCustomer.HasRoleAction(RoleAction.Catalog))
            {
                model.ReviewId = null;
            }

            return model;
        }
    }
}
