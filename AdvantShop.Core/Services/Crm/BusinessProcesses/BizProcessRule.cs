using System;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Scheduler;

namespace AdvantShop.Core.Services.Crm.BusinessProcesses
{
    public enum EBizProcessObjectType
    {
        None = 0,
        Order = 1,
        Lead = 2,
        Call = 3,
        Review = 4,
        Customer = 5,
    }

    public abstract class BizProcessRule
    {
        public virtual EBizProcessObjectType ObjectType
        {
            get { return EBizProcessObjectType.None; }
        }

        public virtual EBizProcessEventType EventType
        {
            get { return EBizProcessEventType.None; }
        }

        public int Id { get; set; }
        public int? EventObjId { get; set; }

        public TimeInterval TaskDueDateInterval { get; set; }
        public TimeInterval TaskCreateInterval { get; set; }
        public int Priority { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public TaskPriority TaskPriority { get; set; }
        public int? TaskGroupId { get; set; }

        public ManagerFilter ManagerFilter { get; set; }

        public virtual IBizObjectFilter Filter { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public virtual string EventName { get { return EventType.Localize(); } }

        public virtual string[] AvailableVariables { get { return null; } }
        public virtual string ReplaceVariables(string value, IBizObject bizObject) { return value; }
    }
}
