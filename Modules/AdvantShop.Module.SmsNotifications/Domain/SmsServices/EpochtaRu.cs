using System.Net;
using System.Text;
using System.Web;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;

namespace AdvantShop.Module.SmsNotifications.Domain.SmsServices
{
    public class EpochtaRu : SMSSenderService
    {
        private readonly string _publicKey;
        private readonly string _privateKey;
        private readonly string _sender;

        public EpochtaRu()
        {
            _publicKey = ModuleSettingsProvider.GetSettingValue<string>("WwwEpochtaRuApiKey", SmsNotifications.ModuleId);
            _privateKey = ModuleSettingsProvider.GetSettingValue<string>("WwwEpochtaRuPrivatKey", SmsNotifications.ModuleId);
            _sender = ModuleSettingsProvider.GetSettingValue<string>("WwwEpochtaRuSender", SmsNotifications.ModuleId);
        }

        public override ESMSSenderService Type
        {
            get { return ESMSSenderService.WwwEpochtaRu; }
        }

        public override string SendSMS(long phone, string text)
        {
            string result;
            using (var client = new WebClient())
            {
                result =
                    client.DownloadString(
                        string.Format("https://{0}/api/sms/3.0/sendSMS?userapp=advantshop&key={1}&sum={5}&sender={2}&text={4}&phone={3}&datetime=&sms_lifetime=0",
                            "atompark.com",
                            HttpUtility.UrlEncode(_publicKey),
                            HttpUtility.UrlEncode(_sender),
                            phone,
                            HttpUtility.UrlEncode(text),
                            string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", "sendSMS", "", _publicKey, phone, _sender, "0", text, "3.0", _privateKey)
                                    .Md5(false, Encoding.UTF8)));

            }
            return result;
        }
    }
}
