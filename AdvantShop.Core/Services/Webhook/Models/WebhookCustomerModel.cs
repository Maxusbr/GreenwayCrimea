using System;
using AdvantShop.Customers;

namespace AdvantShop.Core.Services.Webhook.Models
{
    public class WebhookCustomerModel : WebhookModel
    {
        public Guid Id { get; set; }

        public static explicit operator WebhookCustomerModel(Customer customer)
        {
            return new WebhookCustomerModel
            {
                Id = customer.Id,
            };
        }
    }
}
