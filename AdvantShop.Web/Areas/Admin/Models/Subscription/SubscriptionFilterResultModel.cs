using AdvantShop.Core.Services.Mails;
using System;

namespace AdvantShop.Web.Admin.Models.Subscription
{
    public class SubscriptionFilterResultModel
    {
        public int Id { get; set; }

        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public EMailRecipientType CustomerType { get; set; }

        public bool Enabled { get; set; }
        public DateTime? SubscribeDate { get; set; }
        public string SubscribeDateStr
        {
            get { return SubscribeDate.HasValue ? SubscribeDate.Value.ToString("dd.MM.yyyy HH:mm") : string.Empty; }
        }
        public DateTime? UnsubscribeDate { get; set; }
        public string UnsubscribeDateStr
        {
            get { return UnsubscribeDate.HasValue ? UnsubscribeDate.Value.ToString("dd.MM.yyyy HH:mm"): string.Empty; }
        }
        public string UnsubscribeReason { get; set; }
    }
}
