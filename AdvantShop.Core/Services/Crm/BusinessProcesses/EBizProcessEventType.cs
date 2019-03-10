using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.Core.Services.Crm.BusinessProcesses
{
    public enum EBizProcessEventType
    {
        None = 0,
        [Localize("Core.Services.EBizProcessEventType.OrderCreated")]
        OrderCreated = 1,
        [Localize("Core.Services.EBizProcessEventType.OrderStatusChanged")]
        OrderStatusChanged = 2,
        [Localize("Core.Services.EBizProcessEventType.LeadCreated")]
        LeadCreated = 3,
        [Localize("Core.Services.EBizProcessEventType.LeadStatusChanged")]
        LeadStatusChanged = 4,
        [Localize("Core.Services.EBizProcessEventType.CallMissed")]
        CallMissed = 5,
        [Localize("Core.Services.EBizProcessEventType.ReviewAdded")]
        ReviewAdded = 6,
        [Localize("Core.Services.EBizProcessEventType.MessageReply")]
        MessageReply = 7,
    }

}
