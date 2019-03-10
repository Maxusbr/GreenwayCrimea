using System;
using AdvantShop.Core.Scheduler;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Crm.BusinessProcesses;
using AdvantShop.Core.Services.Localization;

namespace AdvantShop.Web.Admin.Models.BizProcessRules
{
    public class BizProcessRuleModel
    {
        public int Id { get; set; }
        public EBizProcessObjectType ObjectType { get; set; }
        public EBizProcessEventType EventType { get; set; }
        public string EventTypeString { get { return EventType.ToString(); } }
        public int? EventObjId { get; set; }

        public TimeInterval TaskDueDateInterval { get; set; }
        public string TaskDueDateIntervalSerialized { get; set; }
        public TimeInterval TaskCreateInterval { get; set; }
        public string TaskCreateIntervalSerialized { get; set; }

        public int Priority { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public TaskPriority TaskPriority { get; set; }
        public int? TaskGroupId { get; set; }

        public ManagerFilter ManagerFilter { get; set; }
        public string ManagerFilterSerialized { get; set; }

        public IBizObjectFilter Filter { get; set; }
        public string FilterSerialized { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public string EventName { get; set; }

        public string[] AvailableVariables { get; set; }

        public string TaskDueDateIntervalFormatted
        {
            get
            {
                return TaskDueDateInterval == null || TaskDueDateInterval.Interval == 0
                    ? string.Empty 
                    : string.Format("{0} {1}", TaskDueDateInterval.Interval, TaskDueDateInterval.Numeral());
            }
        }

        public string TaskCreateIntervalFormatted
        {
            get
            {
                return TaskCreateInterval == null || TaskCreateInterval.Interval == 0
                    ? LocalizationService.GetResource("Admin.BizProcessRules.TaskCreateInterval.NotSet")
                    : LocalizationService.GetResourceFormat("Admin.BizProcessRules.TaskCreateInterval.After", TaskCreateInterval.Interval == 1 ? string.Empty : TaskCreateInterval.Interval + " ", TaskCreateInterval.Numeral());
            }
        }

        public static explicit operator BizProcessRuleModel(BizProcessRule rule)
        {
            return new BizProcessRuleModel
            {
                Id = rule.Id,
                ObjectType = rule.ObjectType,
                EventType = rule.EventType,
                EventObjId = rule.EventObjId,
                TaskDueDateInterval = rule.TaskDueDateInterval,
                TaskCreateInterval = rule.TaskCreateInterval,
                Priority = rule.Priority,
                TaskName = rule.TaskName,
                TaskDescription = rule.TaskDescription,
                TaskPriority = rule.TaskPriority,
                TaskGroupId = rule.TaskGroupId,
                ManagerFilter = rule.ManagerFilter,
                Filter = rule.Filter,
                DateCreated = rule.DateCreated,
                DateModified = rule.DateModified,
                EventName = rule.EventName,
                AvailableVariables = rule.AvailableVariables
            };
        }
    }
}
