using System.Net;
using System.Web;
using AdvantShop.Core.Modules;

namespace AdvantShop.Module.SmsNotifications.Domain.SmsServices
{
    public class IqsmsmRu : SMSSenderService
    {
        private readonly string _login;
        private readonly string _password;
        private readonly string _sender;

        public IqsmsmRu()
        {
            _login = ModuleSettingsProvider.GetSettingValue<string>("WwwIqsmsRuLogin", SmsNotifications.ModuleId);
            _password = ModuleSettingsProvider.GetSettingValue<string>("WwwIqsmsRuPassword", SmsNotifications.ModuleId);
            _sender = ModuleSettingsProvider.GetSettingValue<string>("WwwIqsmsRuSender", SmsNotifications.ModuleId);
        }

        public override ESMSSenderService Type
        {
            get { return ESMSSenderService.WwwIqsmsRu; }
        }

        public override string SendSMS(long phone, string text)
        {
            string result;
            using (var client = new WebClient())
            {
                result =
                    client.DownloadString(
                        string.Format(
                            "http://{0}/send/?login={1}&password={2}&from={3}&phone={4}&text={5}",
                            "gate.iqsms.ru",
                            HttpUtility.UrlEncode(_login),
                            HttpUtility.UrlEncode(_password),
                            HttpUtility.UrlEncode(_sender),
                            phone,
                            HttpUtility.UrlEncode(text)));

            }
            return result;
        }
    }
}
