using System;
using System.Collections.Specialized;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Core.Services.Bonuses.Sms.Gateways
{
    public class UniSenderGateway : BaseSmsGateway
    {
        private const string _apiKey2 = "5psdahmx3su6eri1ycbx4syu6teph7o6j5u1towo";
        private const string Host = "https://api.unisender.com/ru/api/";

        private readonly string _apiKey;

        public override SmsProviderType Type { get { return SmsProviderType.UniSender; } }

        public UniSenderGateway()
        {
            _apiKey = BonusSystem.UniSenderApiKey;
        }

        public override string Send(long destination, string message, string from)
        {
            if (_apiKey.IsNullOrEmpty())
                throw new BlException("UniSender settings not set");
            var nvc = new NameValueCollection
            {
                {"format", "json"},
                {"api_key", HttpUtility.UrlEncode(_apiKey)},
                {"phone", GetPhonePrepared(destination)},
                {"sender", HttpUtility.UrlEncode(from)},
                {"text", HttpUtility.UrlEncode(message)},
            };

            var res = MakeRequest("sendSms", nvc);

            if (res.error != null)
                throw new BlException((string)res.error);

            return res.result.sms_id;
        }

        public string Register(string email, string login, string password)
        {
            var nvc = new NameValueCollection
            {
                {"format", "json"},
                {"api_key", _apiKey2},
                {"email", HttpUtility.UrlEncode(email)},
                {"login", HttpUtility.UrlEncode(login)},
                {"password", HttpUtility.UrlEncode(password)},
                {"api_mode", "on"},
            };

            var res = MakeRequest("register", nvc);

            if (res.error != null)
                throw new BlException((string)res.error);

            return (string)res.result.api_key;
        }

        private dynamic MakeRequest(string path, NameValueCollection nvc)
        {
            var url = Host + path + (nvc.Count > 0 ? "?" + ToQueryString(nvc) : string.Empty);
            using (var client = new WebClient())
            {
                try
                {
                    return JsonConvert.DeserializeObject<dynamic>(client.DownloadString(url));
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}
