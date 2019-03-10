using System;

namespace AdvantShop.Core.Services.Webhook.Models
{
    public abstract class WebhookModel
    {
        public Guid CurrentCustomerId { get; set; }
    }
}
