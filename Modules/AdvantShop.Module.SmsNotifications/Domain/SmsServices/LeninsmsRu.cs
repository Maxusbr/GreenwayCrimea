using System.Net;
using System.Web;
using AdvantShop.Core.Modules;

namespace AdvantShop.Module.SmsNotifications.Domain.SmsServices
{
    public class LeninsmsRu : SMSSenderService
    {
        private readonly string _login;
        private readonly string _apiKey;
        private readonly string _sender;

        public LeninsmsRu()
        {
            _login = ModuleSettingsProvider.GetSettingValue<string>("LeninsmsRuLogin", SmsNotifications.ModuleId);
            _apiKey = ModuleSettingsProvider.GetSettingValue<string>("LeninsmsRuApiKey", SmsNotifications.ModuleId);
            _sender = ModuleSettingsProvider.GetSettingValue<string>("LeninsmsRuSender", SmsNotifications.ModuleId);
        }

        public override ESMSSenderService Type
        {
            get { return ESMSSenderService.LeninsmsRu; }
        }

        public override string SendSMS(long phone, string text)
        {
            string result;
            using (var client = new WebClient())
            {
                result =
                    client.DownloadString(
                        string.Format(
                            "http://{0}/message/send?user={1}&apikey={2}&sender={3}&recipients={4}&message={5}",
                            "api.leninsms.ru",
                            HttpUtility.UrlEncode(_login),
                            HttpUtility.UrlEncode(_apiKey),
                            HttpUtility.UrlEncode(_sender),
                            phone,
                            HttpUtility.UrlEncode(text)));

            }
            return result;
        }
    }
}
