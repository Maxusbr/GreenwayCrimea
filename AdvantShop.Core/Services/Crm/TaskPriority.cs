using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.Core.Services.Crm
{
    public enum TaskPriority
    {
        [Localize("Core.Crm.TaskPriority.Low")]
        Low = 0,
        [Localize("Core.Crm.TaskPriority.Medium")]
        Medium = 1,
        [Localize("Core.Crm.TaskPriority.High")]
        High = 2
    }
}
