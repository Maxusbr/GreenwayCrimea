using System.Net;
using System.Web;
using AdvantShop.Core.Modules;
using AdvantShop.Diagnostics;
using Newtonsoft.Json;

namespace AdvantShop.Module.SmsNotifications.Domain.SmsServices
{
    public class UnisenderCom : SMSSenderService
    {
        private readonly string _apiKey;
        private readonly string _apiKey2 = "5psdahmx3su6eri1ycbx4syu6teph7o6j5u1towo";
        private readonly string _sender;

        public UnisenderCom()
        {
            _apiKey = ModuleSettingsProvider.GetSettingValue<string>("WwwUnisenderComApiKey", SmsNotifications.ModuleId);
            _sender = ModuleSettingsProvider.GetSettingValue<string>("WwwUnisenderComSender", SmsNotifications.ModuleId);
        }

        public override ESMSSenderService Type
        {
            get { return ESMSSenderService.WwwUnisenderCom; }
        }

        public override string SendSMS(long phone, string text)
        {
            string result;
            using (var client = new WebClient())
            {
                result =
                    client.DownloadString(
                        string.Format(
                            "https://{0}/ru/api/sendSms?format=json&api_key={1}&phone={3}&sender={2}&text={4}",
                            "api.unisender.com",
                            HttpUtility.UrlEncode(_apiKey),
                            HttpUtility.UrlEncode(_sender),
                            phone,
                            HttpUtility.UrlEncode(text)));

            }
            return result;
        }


        public string Register(string email, string login, string password)
        {
            string result;
            using (var client = new WebClient())
            {
                result =
                    client.DownloadString(
                        string.Format(
                            "https://{0}/ru/api/register?format=json&api_key={1}&email={2}&login={3}&password={4}&api_mode=on",
                            "api.unisender.com",
                            HttpUtility.UrlEncode(_apiKey2),
                            HttpUtility.UrlEncode(email), HttpUtility.UrlEncode(login), HttpUtility.UrlEncode(password)));

            }

            if (!string.IsNullOrWhiteSpace(result) && !result.Contains("error"))
            {
                var res = JsonConvert.DeserializeObject<RegisterResponse>(result);
                return res.result.api_key;
            }
            else
            {
                Debug.Log.Error(result);
                return string.Empty;
            }
        }

        public class RegisterResponse
        {
            public RegisterResult result { get; set; }
        }
        public class RegisterResult
        {
            public string api_key { get; set; }
        }
    }
}
