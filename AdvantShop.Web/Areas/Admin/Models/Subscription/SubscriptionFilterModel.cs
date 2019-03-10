using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Core.Services.Mails;
using System;

namespace AdvantShop.Web.Admin.Models.Subscription
{
    public class SubscriptionFilterModel : BaseFilterModel
    {

        public int? Id { get; set; }

        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public EMailRecipientType? CustomerType { get; set; }

        public bool? Enabled { get; set; }
        public DateTime? SubscribeDate { get; set; }
        public string SubscribeFrom { get; set; }
        public string SubscribeTo { get; set; }
        public DateTime? UnsubscribeDate { get; set; }
        public string UnSubscribeFrom { get; set; }
        public string UnsubscribeTo { get; set; }
        public string UnsubscribeReason { get; set; }
    }
}
