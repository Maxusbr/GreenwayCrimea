using System.Collections.Generic;
using System.Text;

namespace AdvantShop.Module.SmsNotifications.Domain
{
    public enum ESMSSenderService
    {
        None,
        WwwSms4BRu,
        SmslabRu,
        WwwSmsimpleRu,
        GsmInformRu,
        WwwIqsmsRu,
        LeninsmsRu,
        WwwSmspilotRu,
        RuSmsOnlineCom,
        WwwUnisenderCom,
        WwwEpochtaRu,
        StreamTelecom
    }

    public abstract class SMSSenderService
    {
        public abstract ESMSSenderService Type { get; }
        public abstract string SendSMS(long phone, string text);

        public virtual string SendSMS(List<long> phones, string text)
        {
            var sb = new StringBuilder();
            foreach (var phone in phones)
            {
                sb.AppendFormat("[{0}],", SendSMS(phone, text));
            }
            return sb.ToString().TrimEnd(',');
        }
    }
}
