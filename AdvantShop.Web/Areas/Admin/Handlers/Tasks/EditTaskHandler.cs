using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.CMS;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Customers;
using AdvantShop.Localization;
using AdvantShop.Web.Admin.Handlers.AdminNotifications;
using AdvantShop.Web.Admin.Models.Tasks;

namespace AdvantShop.Web.Admin.Handlers.Tasks
{
    public class EditTaskHandler
    {
        private readonly TaskModel _model;

        public EditTaskHandler(TaskModel model)
        {
            _model = model;
        }

        public bool Execute()
        {
            var task = TaskService.GetTask(_model.Id);
            if (task == null)
                return false;
            var prevState = (Task)task.Clone();

            task.Name = _model.Name;
            task.AssignedManagerId = _model.AssignedManagerId;
            task.AppointedManagerId = _model.AppointedManagerId;
            task.DueDate = _model.DueDate;
            task.Description = _model.Description;
            task.TaskGroupId = _model.TaskGroupId;
            task.Priority = _model.Priority;
            task.DateModified = DateTime.Now;
            task.Status = _model.Status;
            task.Accepted = _model.Accepted;
            task.ResultShort = _model.ResultShort;
            task.ResultFull = _model.ResultFull;

            TaskService.UpdateTask(task);
            ProcessNotifications(prevState, task);

            AddLeadEvent(prevState, task);

            return true;
        }


        private void ProcessNotifications(Task prevState, Task newState)
        {
            var modifier = CustomerContext.CurrentCustomer;
            var notificationsHandler = new AdminNotificationsHandler();

            var assignedCustomer = newState.AssignedManager != null ? newState.AssignedManager.Customer : null;
            var appointedCustomer = newState.AppointedManager != null ? newState.AppointedManager.Customer : null;

            // сменился исполнитель
            if (prevState.AssignedManagerId != newState.AssignedManagerId && assignedCustomer != null && assignedCustomer.Id != modifier.Id &&
                assignedCustomer.HasRoleAction(RoleAction.Tasks))
            {
                notificationsHandler.NotifyCustomers(new OnSetTaskNotification(prevState, modifier), assignedCustomer.Id);
            }

            TaskService.OnTaskChanged(modifier, prevState, newState, prevState.Attachments);

            var customerIds = new List<Guid>();
            if (assignedCustomer != null && assignedCustomer.Id != modifier.Id && assignedCustomer.HasRoleAction(RoleAction.Tasks))
                customerIds.Add(assignedCustomer.Id);
            if (appointedCustomer != null && appointedCustomer.Id != modifier.Id && appointedCustomer.HasRoleAction(RoleAction.Tasks))
                customerIds.Add(appointedCustomer.Id);

            if (!customerIds.Any())
                return;

            var onChangeNotifications = new List<AdminNotification>();
            if (prevState.Status != newState.Status)
                onChangeNotifications.Add(new OnTaskChangeNotification(prevState, modifier, LocalizationService.GetResource("Admin.Tasks.TaskChanges.Status"), 
                    prevState.Status.Localize(), newState.Status.Localize()));
            if (prevState.DueDate != newState.DueDate)
                onChangeNotifications.Add(new OnTaskChangeNotification(prevState, modifier, LocalizationService.GetResource("Admin.Tasks.TaskChanges.DueDate"),
                    prevState.DueDate.HasValue ? Culture.ConvertShortDate(prevState.DueDate.Value) : LocalizationService.GetResource("Admin.Tasks.TaskChanges.DueDateNotSet"),
                    newState.DueDate.HasValue ? Culture.ConvertShortDate(newState.DueDate.Value) : LocalizationService.GetResource("Admin.Tasks.TaskChanges.DueDateNotSet")));
            if (prevState.Priority != newState.Priority)
                onChangeNotifications.Add(new OnTaskChangeNotification(prevState, modifier, LocalizationService.GetResource("Admin.Tasks.TaskChanges.Priority"),
                    prevState.Priority.Localize(), newState.Priority.Localize()));
            if (prevState.Description != newState.Description)
                onChangeNotifications.Add(new OnTaskChangeNotification(prevState, modifier, LocalizationService.GetResource("Admin.Tasks.TaskChanges.Description"), null, null));
            if (prevState.Name != newState.Name)
                onChangeNotifications.Add(new OnTaskChangeNotification(prevState, modifier, LocalizationService.GetResource("Admin.Tasks.TaskChanges.Name"), null, newState.Name));
            if (!prevState.Accepted && newState.Accepted)
                onChangeNotifications.Add(new OnTaskAcceptedNotification(prevState, modifier));

            notificationsHandler.NotifyCustomers(onChangeNotifications.ToArray(), customerIds.ToArray());
        }

        private void AddLeadEvent(Task prevState, Task task)
        {
            if (task.LeadId == null)
                return;

            if (prevState.Status != TaskStatus.Completed && task.Status == TaskStatus.Completed)
            {
                LeadEventService.AddEvent(new LeadEvent()
                {
                    LeadId = task.LeadId.Value,
                    Title = LocalizationService.GetResource("Admin.Handlers.Tasks.EditTaskHandler.TaskCompleted"),
                    Message =
                        LocalizationService.GetResourceFormat("Admin.Handlers.Tasks.EditTaskHandler.TaskMessage",
                            task.Name, task.ResultFull),
                    Type = LeadEventType.Task,
                    CreatedBy = CustomerContext.CurrentCustomer.GetShortName(),
                    TaskId = task.Id,
                });
            }
        }
    }
}
