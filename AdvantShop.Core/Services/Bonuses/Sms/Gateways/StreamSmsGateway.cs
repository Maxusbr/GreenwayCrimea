using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using AdvantShop.Core.Common.Extensions;
using Newtonsoft.Json;
using Encoding = System.Text.Encoding;

namespace AdvantShop.Core.Services.Bonuses.Sms.Gateways
{
    public class StreamSmsGateway : BaseSmsGateway
    {
        private const string Host = "http://gateway.api.sc/rest/";
        private readonly string _login;
        private readonly string _password;

        public override SmsProviderType Type
        {
            get { return SmsProviderType.StreamSms; }
        }

        public StreamSmsGateway()
        {
            _login = BonusSystem.StreamSmsLogin;
            _password = BonusSystem.StreamSmsPassword;
        }

        public override string Send(long destination, string message, string from)
        {
            if (_login.IsNullOrEmpty() || _password.IsNullOrEmpty())
                throw new BlException("stream-telecom settings not set");
            var ss = GetSession();
            return SendSingleMessage(ss, destination, message, from);
        }

        //public float Balance()
        //{
        //    var ss = GetSession();
        //    return GetBalance(ss);
        //}

        private string GetSession()
        {
            var url = string.Format("Session//?login={0}&password={1}", _login, _password);
            return MakeRequest<string>(url, method: "GET");
        }

        private float GetBalance(string session)
        {
            var url = string.Format("Balance/?sessionId={0}", session);
            var balance = MakeRequest<string>(url, method: "GET");
            return balance.TryParseFloat();
        }

        private string SendSingleMessage(string session, long destination, string message, string from)
        {
            var url = string.Format("Send/SendSms/");

            var str = new NameValueCollection
            {
                {"sessionId", session},
                {"destinationAddress", GetPhonePrepared(destination)},
                {"data", message},
                {"sourceAddress", from}
            };
            var smsKey = MakeRequest<string[]>(url, ToQueryString(str), contentType: "application/x-www-form-urlencoded; charset=utf-8");
            return smsKey.Aggregate((a, b) => a + " " + b);
        }

        private static T MakeRequest<T>(string url, string data = null, string method = "POST", string contentType = "text/json") where T : class
        {
            try
            {
                var request = WebRequest.Create(Host + url) as HttpWebRequest;
                if (request == null)
                    return null;
                request.Method = method;
                request.ContentType = contentType;

                if (data != null)
                {
                    var bytes = Encoding.UTF8.GetBytes(data);
                    request.ContentLength = bytes.Length;

                    using (var requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(bytes, 0, bytes.Length);
                        requestStream.Close();
                    }
                }

                var responseContent = "";
                using (var response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        if (stream == null)
                            return JsonConvert.DeserializeObject<T>(responseContent);
                        using (var reader = new StreamReader(stream))
                        {
                            responseContent = reader.ReadToEnd();
                        }
                    }
                }

                return JsonConvert.DeserializeObject<T>(responseContent);
            }
            catch (WebException ex)
            {
                using (var eResponse = ex.Response)
                {
                    if (eResponse == null)
                        return null;
                    using (var eStream = eResponse.GetResponseStream())
                        if (eStream != null)
                            using (var reader = new StreamReader(eStream))
                            {
                                var error = reader.ReadToEnd();
                                throw new Exception(error + " " + data + " " + url);
                            }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }
    }
}
