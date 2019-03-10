//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Core.Services.Mails;

namespace AdvantShop.Customers
{
    public class Subscription : ISubscriber
    {
        public int Id { get; set; }

        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public EMailRecipientType CustomerType { get; set; }

        public bool Subscribe { get; set; }
        public DateTime SubscribeDate { get; set; }
        public DateTime UnsubscribeDate { get; set; }
        public string UnsubscribeReason { get; set; }
    }
}