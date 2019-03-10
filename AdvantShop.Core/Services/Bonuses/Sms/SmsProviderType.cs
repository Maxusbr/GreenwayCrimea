using System.ComponentModel.DataAnnotations;

namespace AdvantShop.Core.Services.Bonuses.Sms
{
    public enum SmsProviderType : byte
    {
        [Display(Name = "Не указано")]
        None = 0,
        [Display(Name = "stream-telecom.ru")]
        StreamSms = 2,
        [Display(Name = "Sms4B")]
        Sms4B = 1,
        [Display(Name = "ePochta")]
        EPochta = 3,
        [Display(Name = "UniSender")]
        UniSender = 4,
    }
}
