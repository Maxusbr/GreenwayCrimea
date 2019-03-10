using System;
using System.Net;
using System.Web;
using System.Collections.Generic;
using System.Xml.Serialization;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Diagnostics;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using AdvantShop.Helpers;

namespace AdvantShop.Module.SmsNotifications.Domain.SmsServices
{
    public class RuSmsOnlineCom : SMSSenderService
    {
        private readonly string _login;
        private readonly string _secretKey;
        private readonly string _sender;

        public RuSmsOnlineCom()
        {
            _login = HttpUtility.UrlEncode(ModuleSettingsProvider.GetSettingValue<string>("RuSmsOnlineComLogin", SmsNotifications.ModuleId));
            _secretKey = ModuleSettingsProvider.GetSettingValue<string>("RuSmsOnlineComSecretKey", SmsNotifications.ModuleId);
            _sender = HttpUtility.UrlEncode(ModuleSettingsProvider.GetSettingValue<string>("RuSmsOnlineComSender", SmsNotifications.ModuleId));
        }

        public override ESMSSenderService Type
        {
            get { return ESMSSenderService.RuSmsOnlineCom; }
        }

        public override string SendSMS(long phone, string text)
        {
            var request = new RequestClass
            {
                User = _login,
                From = StringHelper.Translit(_sender),
                Phone = phone,
                Txt = HttpUtility.UrlEncode(text),
                Sign = (_login + _sender + phone + HttpUtility.UrlEncode(text) + _secretKey).Md5(false)
            };

            var serializer = new XmlSerializer(typeof(ResponceStatus));
            var response = PostRequestGetStream("https://bulk.sms-online.com/", JsonConvert.SerializeObject(request));
            if (response == null)
                return "Request Fail";
            var result = (ResponceStatus)serializer.Deserialize(new StreamReader(response));
            if (result.Code < 0)
                Debug.LogError("Module: SmsNotifications; Status Response: " + result.Message);
            return result.Message;
        }

        private Stream PostRequestGetStream(string url, string data)
        {
            try
            {
                var request = WebRequest.Create(url);
                request.Method = "POST";
                request.Timeout = 30000;

                byte[] byteArray = Encoding.UTF8.GetBytes(data);

                request.ContentType = "application/json";
                request.ContentLength = byteArray.Length;
                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                }

                return request.GetResponse().GetResponseStream();
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                return null;
            }
        }

        [Serializable]
        [XmlRoot("response", IsNullable = false)]
        public class ResponceStatus
        {
            [XmlElement("code")]
            public int Code { get; set; }
            [XmlElement("tech_message")]
            public string Message { get; set; }
            [XmlElement("msg_id")]
            public List<string> MessageId { get; set; }
        }

        [Serializable]
        public class RequestClass
        {
            [JsonProperty(PropertyName = "txt")]
            public string Txt { get; set; }
            [JsonProperty(PropertyName = "user")] 
            public string User { get; set; }
            [JsonProperty(PropertyName = "from")]
            public string From { get; set; }
            [JsonProperty(PropertyName = "phone")]
            public long Phone { get; set; }
            [JsonProperty(PropertyName = "sign")]
            public string Sign { get; set; }
            [JsonProperty(PropertyName = "sending_method")]
            public string SendingMethod = "sms";
            [JsonProperty(PropertyName = "charset")]
            public string Charset = "UTF8";
        }
    }
}
