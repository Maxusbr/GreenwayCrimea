using System.Net;
using System.Web;
using AdvantShop.Core.Modules;

namespace AdvantShop.Module.SmsNotifications.Domain.SmsServices
{
    public class SmsimpleRu : SMSSenderService
    {
        private readonly string _login;
        private readonly string _password;
        private readonly string _originId;

        public SmsimpleRu()
        {
            _login = ModuleSettingsProvider.GetSettingValue<string>("WwwSmsimpleRuLogin", SmsNotifications.ModuleId);
            _password = ModuleSettingsProvider.GetSettingValue<string>("WwwSmsimpleRuPassword", SmsNotifications.ModuleId);
            _originId = ModuleSettingsProvider.GetSettingValue<string>("WwwSmsimpleRuOriginId", SmsNotifications.ModuleId);
        }

        public override ESMSSenderService Type
        {
            get { return ESMSSenderService.WwwSmsimpleRu; }
        }

        public override string SendSMS(long phone, string text)
        {
            string result;
            using (var client = new WebClient())
            {
                result =
                    client.DownloadString(
                        string.Format(
                            "https://{0}/http_send.php?user={1}&pass={2}&or_id={3}&phone={4}&message={5}",
                            "smsimple.ru",
                            HttpUtility.UrlEncode(_login),
                            HttpUtility.UrlEncode(_password),
                            HttpUtility.UrlEncode(_originId),
                            phone,
                            HttpUtility.UrlEncode(text)));
            }
            return result;
        }
    }
}
