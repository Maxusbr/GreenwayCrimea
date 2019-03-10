using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.Core.Services.Crm
{
    public enum TaskStatus
    {
        [Localize("Core.Crm.TaskStatus.Open")]
        Open = 0,
        [Localize("Core.Crm.TaskStatus.InProgress")]
        InProgress = 1,
        [Localize("Core.Crm.TaskStatus.Completed")]
        Completed = 2,
    }
}
