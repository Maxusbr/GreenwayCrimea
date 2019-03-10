using System;

namespace AdvantShop.Core.Services.Loging.Smses
{
    public class TextMessage
    {
        public DateTime CreateOn { get; set; }
        public Guid CustomerId { get; set; }
        public long Phone { get; set; }
        public string Body { get; set; }
        public SmsStatus Status { get; set; }
        public string ShopId { get; set; }
    }
}
