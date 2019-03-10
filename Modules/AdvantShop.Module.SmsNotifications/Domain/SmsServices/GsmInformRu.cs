using System.Net;
using System.Web;
using AdvantShop.Core.Modules;

namespace AdvantShop.Module.SmsNotifications.Domain.SmsServices
{
    public class GsmInformRu : SMSSenderService
    {
        private readonly string _login;
        private readonly string _password;
        private readonly string _sender;

        public GsmInformRu()
        {
            _login = ModuleSettingsProvider.GetSettingValue<string>("GsmInformRuLogin", SmsNotifications.ModuleId);
            _password = ModuleSettingsProvider.GetSettingValue<string>("GsmInformRuPassword", SmsNotifications.ModuleId);
            _sender = ModuleSettingsProvider.GetSettingValue<string>("GsmInformRuSender", SmsNotifications.ModuleId);
        }

        public override ESMSSenderService Type
        {
            get { return ESMSSenderService.GsmInformRu; }
        }

        public override string SendSMS(long phone, string text)
        {
            string result;
            using (var client = new WebClient())
            {
                result =
                    client.DownloadString(
                        string.Format(
                            "https://{0}/send/index.php?login={1}&password={2}&originator={3}&phones={4}&message={5}&codepage=UTF-8&rus=1",
                            "gsm-inform.ru",
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
