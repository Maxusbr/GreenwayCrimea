using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.Core.Services.Loging.Emails
{
    public enum EmailStatus
    {
        [Localize("Core.Loging.EmailStatus.Sent")]
        Sent,
        [Localize("Core.Loging.EmailStatus.Error")]
        Error,
    }
}
