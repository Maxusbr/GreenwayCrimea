using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Core.Services.Bonuses.Sms.Gateways
{
    public class Sms4BGateway : BaseSmsGateway
    {
        private const string Host = "https://sms4b.ru/";

        private readonly string _login;
        private readonly string _password;

        public override SmsProviderType Type
        {
            get { return SmsProviderType.Sms4B; }
        }

        public Sms4BGateway()
        {
            _login = BonusSystem.Sms4BLogin;
            _password = BonusSystem.Sms4BPassword;
        }

        public override string Send(long destination, string message, string from)
        {
            if (_login.IsNullOrEmpty() || _password.IsNullOrEmpty())
                throw new BlException("Sms4B settings not set");
            var nvc = new NameValueCollection
            {
                {"Login", HttpUtility.UrlEncode(_login)},
                {"Password", HttpUtility.UrlEncode(_password)},
                {"Source", HttpUtility.UrlEncode(from)},
                {"Phone", GetPhonePrepared(destination)},
                {"Text", HttpUtility.UrlEncode(message)},
            };

            var result = MakeRequest("ws/sms.asmx/SendSMS", ToQueryString(nvc), contentType: "application/x-www-form-urlencoded");
            if (result.IsNotEmpty() && result.StartsWith("-"))
            {
                throw new BlException(string.Format("Error. Code: {0}", result));
            }

            return result;
        }

        private string MakeRequest(string path, string data = null, string method = "POST", string contentType = "text/json")
        {
            try
            {
                var request = WebRequest.Create(Host + path) as HttpWebRequest;
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

                using (var response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var xml = new XmlDocument();
                            xml.LoadXml(reader.ReadToEnd());
                            return xml.InnerText;
                        }
                    }
                }
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
                                throw new Exception(error + " " + data + " " + path);
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
