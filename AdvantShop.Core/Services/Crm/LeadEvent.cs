using System;
using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.Core.Services.Crm
{
    public enum LeadEventType
    {
        [Localize("Core.Services.Crm.LeadEventType.Orher")]
        None = 0,

        [Localize("Core.Services.Crm.LeadEventType.Comment")]
        Comment = 1,

        [Localize("Core.Services.Crm.LeadEventType.Call")]
        Call = 2,

        [Localize("Core.Services.Crm.LeadEventType.SMS")]
        Sms = 3,

        [Localize("Core.Services.Crm.LeadEventType.Email")]
        Email = 4,

        [Localize("Core.Services.Crm.LeadEventType.Task")]
        Task = 5,

        [Localize("Core.Services.Crm.LeadEventType.Vk")]
        Vk = 6,

        [Localize("")]
        Other = 1000,
    }

    public class LeadEvent
    {
        public int Id { get; set; }
        public int LeadId { get; set; }
        public LeadEventType Type { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public int? TaskId { get; set; }
    }
}
