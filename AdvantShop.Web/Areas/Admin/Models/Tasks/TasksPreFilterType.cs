using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.Web.Admin.Models.Tasks
{
    public enum TasksPreFilterType
    {
        [Localize("Admin.Models.Tasks.TasksPreFilterType.None")]
        None = 0,
        [Localize("Admin.Models.Tasks.TasksPreFilterType.AssignedToMe")]
        AssignedToMe = 1,
        [Localize("Admin.Models.Tasks.TasksPreFilterType.AppointedByMe")]
        AppointedByMe = 2,
        [Localize("Admin.Models.Tasks.TasksPreFilterType.Completed")]
        Completed = 3,
        [Localize("Admin.Models.Tasks.TasksPreFilterType.Accepted")]
        Accepted = 4,
        
        Order = 5,
        Lead = 6
    }
}
