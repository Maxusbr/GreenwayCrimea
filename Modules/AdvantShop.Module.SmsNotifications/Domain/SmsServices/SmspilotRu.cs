using System.Net;
using System.Web;
using AdvantShop.Core.Modules;

namespace AdvantShop.Module.SmsNotifications.Domain.SmsServices
{
    public class SmspilotRu : SMSSenderService
    {
        private readonly string _login;
        private readonly string _password;
        private readonly string _sender;

        public SmspilotRu()
        {
            _login = ModuleSettingsProvider.GetSettingValue<string>("WwwSmspilotRuLogin", SmsNotifications.ModuleId);
            _password = ModuleSettingsProvider.GetSettingValue<string>("WwwSmspilotRuPassword", SmsNotifications.ModuleId);
            _sender = ModuleSettingsProvider.GetSettingValue<string>("WwwSmspilotRuSender", SmsNotifications.ModuleId);
        }

        public override ESMSSenderService Type
        {
            get { return ESMSSenderService.WwwSmspilotRu; }
        }

        public override string SendSMS(long phone, string text)
        {
            string result;
            using (var client = new WebClient())
            {
                result =
                    client.DownloadString(
                        string.Format(
                            "https://{0}/api.php?login={1}&password={2}&from={3}&to={4}&send={5}",
                            "smspilot.ru",
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
