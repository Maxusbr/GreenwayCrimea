using System;
using AdvantShop.Core.Services.SEO;

namespace AdvantShop.Core.Services.Loging.Events
{
    public class Event
    {
        public DateTime CreateOn { get; set; }
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public ePageType EvenType { get; set; }
        public string ShopId { get; set; }
        public string IP { get; set; }
    }
}
