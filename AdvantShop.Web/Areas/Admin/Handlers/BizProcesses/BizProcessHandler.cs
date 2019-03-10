using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Crm.BusinessProcesses;
using AdvantShop.Web.Admin.Handlers.Tasks;
using AdvantShop.Web.Admin.Handlers.Users;
using AdvantShop.Web.Admin.Models.Tasks;
using AdvantShop.Web.Admin.Models.Users;

namespace AdvantShop.Web.Admin.Handlers.BizProcesses
{
    public class BizProcessHandler<TRule> where TRule: BizProcessRule
    {
        private readonly TRule _rule;
        private readonly IBizObject _bizObject;

        protected readonly AdminUserModel Employee;
        protected TaskModel TaskModel { get; set; }

        public BizProcessHandler(List<TRule> rules, IBizObject bizObject)
        {
            _bizObject = bizObject;
            _rule = rules.FirstOrDefault(x => x.Filter == null || x.Filter.Check(_bizObject));

            var customer = _rule != null && _rule.ManagerFilter != null ? _rule.ManagerFilter.GetCustomer(bizObject) : null;
            if (customer != null)
                Employee = new GetUserModel(customer).Execute();
        }

        public virtual TaskModel GenerateTask()
        {
            if (Employee == null || _rule == null)
                return null;
            if (TaskModel == null)
                TaskModel = new TaskModel();
            TaskModel.Name = _rule.ReplaceVariables(_rule.TaskName, _bizObject);
            TaskModel.Description = _rule.ReplaceVariables(_rule.TaskDescription, _bizObject);
            TaskModel.AssignedManagerId = Employee.AssociatedManagerId;
            TaskModel.TaskGroupId = _rule.TaskGroupId ?? SettingsTasks.DefaultTaskGroup;
            TaskModel.Priority = _rule.TaskPriority;
            TaskModel.IsAutomatic = true;

            if (_rule.TaskCreateInterval == null)
            {
                TaskModel.DateAppointed = DateTime.Now;
                TaskModel.IsDeferred = false;
            }
            else
            {
                TaskModel.DateAppointed = _rule.TaskCreateInterval.GetDateTime(DateTime.Now);
                TaskModel.IsDeferred = true;
            }
            // крайний срок с даты назначения задачи
            TaskModel.DueDate = _rule.TaskDueDateInterval != null 
                ? _rule.TaskDueDateInterval.GetDateTime(TaskModel.DateAppointed) 
                : (DateTime?)null;

            var task = new AddTaskHandler(TaskModel).Execute();

            new TaskAddedNotificationsHandler(task).Execute();

            return TaskModel;
        }

        public virtual void ProcessBizObject()
        {

        }
    }


}
