using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.Core.Services.Loging.Smses
{
    public enum SmsStatus
    {
        [Localize("Core.Loging.SmsStatus.Sent")]
        Sent,
        [Localize("Core.Loging.SmsStatus.Error")]
        Error,
    }
}
