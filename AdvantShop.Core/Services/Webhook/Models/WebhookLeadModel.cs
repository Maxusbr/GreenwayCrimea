using AdvantShop.Core.Services.Crm;

namespace AdvantShop.Core.Services.Webhook.Models
{
    public class WebhookLeadModel : WebhookModel
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public float Sum { get; set; }

        public static explicit operator WebhookLeadModel(Lead lead)
        {
            return new WebhookLeadModel
            {
                Id = lead.Id,
                Phone = lead.Phone,
                Sum = lead.Sum
            };
        }
    }
}
