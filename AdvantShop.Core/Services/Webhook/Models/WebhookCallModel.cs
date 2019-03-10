using AdvantShop.Core.Services.IPTelephony;

namespace AdvantShop.Core.Services.Webhook.Models
{
    public class WebhookCallModel : WebhookModel
    {
        public int Id { get; set; }
        public string CallId { get; set; }
        public string Phone { get; set; }

        public static explicit operator WebhookCallModel(Call call)
        {
            return new WebhookCallModel
            {
                Id = call.Id,
                CallId = call.CallId,
                Phone = call.Phone,
            };
        }
    }
}
