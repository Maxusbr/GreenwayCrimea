using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.Core.Services.Webhook
{
    public enum EWebhookType
    {
        /// <summary>
        /// System
        /// </summary>
        [Localize("Core.Services.Webhook.EWebhookType.None")]
        None = 0,
        [Localize("Core.Services.Webhook.EWebhookType.BizProcess")]
        BizProcess = 1,
    }
}
