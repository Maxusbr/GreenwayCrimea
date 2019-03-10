using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Web;
using AdvantShop.Core.Common.Extensions;
using Newtonsoft.Json;

namespace AdvantShop.Core.Services.Bonuses.Sms.Gateways
{
    public class EPochtaGateway : BaseSmsGateway
    {
        private const string Host = "https://atompark.com/";

        private readonly string _publicKey;
        private readonly string _privateKey;

        public override SmsProviderType Type { get { return SmsProviderType.EPochta; } }

        public EPochtaGateway()
        {
            _publicKey = BonusSystem.EPochtaPublicKey;
            _privateKey = BonusSystem.EPochtaPrivateKey;
        }

        public override string Send(long destination, string message, string from)
        {
            if (_publicKey.IsNullOrEmpty() || _privateKey.IsNullOrEmpty())
                throw new BlException("ePochta settings not set");

            var phone = GetPhonePrepared(destination);
            var nvc = new NameValueCollection
            {
                {"userapp", "advantshop"},
                {"key", HttpUtility.UrlEncode(_publicKey)},
                {"sum", string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", "sendSMS", "", _publicKey, phone, from, "0", message, "3.0", _privateKey).Md5(false, Encoding.UTF8)},
                {"sender", HttpUtility.UrlEncode(from)},
                {"text", HttpUtility.UrlEncode(message)},
                {"phone", phone},
                {"datetime", ""},
                {"sms_lifetime", "0"},
            };

            var res = MakeRequest("api/sms/3.0/sendSMS", nvc);
            if (res.error != null)
                throw new BlException((string)res.error);

            return (string)res.result.id;
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
