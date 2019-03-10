using AdvantShop.CMS;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Handlers.AdminNotifications;

namespace AdvantShop.Web.Admin.Handlers.Tasks
{
    public class TaskAddedNotificationsHandler
    {
        private readonly Task _task;

        public TaskAddedNotificationsHandler(Task task)
        {
            _task = task;
        }

        public bool Execute()
        {
            if (_task.IsDeferred)
                return false;

            var modifier = CustomerContext.CurrentCustomer;

            if (_task.LeadId != null)
            {
                LeadEventService.AddEvent(new LeadEvent()
                {
                    LeadId = _task.LeadId.Value,
                    Title = LocalizationService.GetResource("Admin.Handlers.Tasks.AddTaskHandler.TaskAdded"),
                    Message = _task.Name,
                    Type = LeadEventType.Task,
                    CreatedBy = modifier.GetShortName(),
                    TaskId = _task.Id
                });
            }

            var notificationsHandler = new AdminNotificationsHandler();
            if (_task.AssignedManager != null && _task.AssignedManager.CustomerId != modifier.Id && 
                _task.AssignedManager.Customer != null && _task.AssignedManager.Customer.HasRoleAction(RoleAction.Tasks))
            {
                notificationsHandler.NotifyCustomers(new OnSetTaskNotification(_task, modifier), _task.AssignedManager.Customer.Id);
                TaskService.OnTaskCreated(_task);
            }
            notificationsHandler.UpdateTasks();

            return true;
        }
    }
}
