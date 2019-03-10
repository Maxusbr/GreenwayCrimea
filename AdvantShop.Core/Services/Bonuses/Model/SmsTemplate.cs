using AdvantShop.Core.Services.Bonuses.Sms;

namespace AdvantShop.Core.Services.Bonuses.Model
{
    public class SmsTemplate
    {
        public ESmsType SmsTypeId { get; set; }
        public string SmsBody { get; set; }
    }
}
