using System;

namespace AdvantShop.Core.Services.Loging.Calls
{
    public class Call
    {
        public DateTime CreateOn { get; set; }
        public Guid CustomerId { get; set; }
        public string CallId { get; set; }
        public string Phone { get; set; }
        public string DstNumber { get; set; }
        public string CallRecordLink { get; set; }
        public string Event { get; set; }
        public string ShopId { get; set; }
    }
}
