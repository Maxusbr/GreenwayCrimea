using System;

namespace AdvantShop.Core.Services.Bonuses.Model
{
    public class SmsLog
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public string State { get; set; }
        public long Phone { get; set; }
        public DateTime Created { get; set; }
    }
}
