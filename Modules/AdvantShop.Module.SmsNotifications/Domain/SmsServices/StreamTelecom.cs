using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using AdvantShop.Core.Modules;
using AdvantShop.Diagnostics;

namespace AdvantShop.Module.SmsNotifications.Domain.SmsServices
{
    public class StreamTelecom : SMSSenderService
    {
        private readonly string _login;
        private readonly string _password;
        private readonly string _sender;

        public StreamTelecom()
        {
            _login = ModuleSettingsProvider.GetSettingValue<string>("StreamTelecomLogin", SmsNotifications.ModuleId);
            _password = ModuleSettingsProvider.GetSettingValue<string>("StreamTelecomPassword", SmsNotifications.ModuleId);
            _sender = ModuleSettingsProvider.GetSettingValue<string>("StreamTelecomSender", SmsNotifications.ModuleId);
        }

        public override ESMSSenderService Type
        {
            get { return ESMSSenderService.StreamTelecom; }
        }

        public override string SendSMS(long phone, string text)
        {
            //string sesssionId = GetSessionId();
            //if (string.IsNullOrEmpty(sesssionId))
            //    return null;

            //var request = (HttpWebRequest)WebRequest.Create("http://gateway.api.sc/rest/Send/SendSms/");

            //var postData = string.Format("&sessionId={0}&destinationAddress={1}&sourceAddress={2}&data={3}",
            //    sesssionId, phone, _sender, text);
            //    //HttpUtility.UrlEncode(text));

            //var data = Encoding.UTF8.GetBytes(postData);  //Encoding.UTF8 Encoding.ASCII

            //request.Method = "POST";
            //request.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
            //request.ContentLength = data.Length;

            //using (var stream = request.GetRequestStream())
            //{
            //    stream.Write(data, 0, data.Length);
            //    stream.Close();
            //}

            //var response = (HttpWebResponse)request.GetResponse();
            //string result = new StreamReader(response.GetResponseStream()).ReadToEnd();

            var requestUrl =
                string.Format("http://gateway.api.sc/get/?user={0}&pwd={1}&sadr={2}&dadr={3}&text={4}",
                    _login, _password, _sender, phone, HttpUtility.UrlEncode(text));

            var request = (HttpWebRequest)WebRequest.Create(requestUrl);
            
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded; charset=utf-8";

            var response = (HttpWebResponse)request.GetResponse();
            string result = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return result;
        }

        public override string SendSMS(List<long> phones, string text)
        {
            foreach (var phone in phones)
            {
                try
                {
                    SendSMS(phone, text);
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(ex);
                    return "error";
                }
            }

            //string sesssionId = GetSessionId();
            //if (string.IsNullOrEmpty(sesssionId))
            //    return null;

            //var request = (HttpWebRequest)WebRequest.Create("http://gateway.api.sc/rest/Send/SendBulk/");

            //var postData = string.Format("sessionId={0}&sourceAddress={1}&destinationAddresses={2}&data={3}",
            //    sesssionId, HttpUtility.UrlEncode(_sender),
            //    phones.Aggregate(new StringBuilder(), (curr, val) => curr.Append(val.ToString()).Append(','), curr => curr.ToString().TrimEnd(',')),
            //    HttpUtility.UrlEncode(text));

            //var data = Encoding.ASCII.GetBytes(postData);

            //request.Method = "POST";
            //request.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
            //request.ContentLength = data.Length;

            //using (var stream = request.GetRequestStream())
            //{
            //    stream.Write(data, 0, data.Length);
            //}

            //var response = (HttpWebResponse)request.GetResponse();
            //string result = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return "";
        }

        private string GetSessionId()
        {
            string sessionId;
            using (var client = new WebClient())
            {
                sessionId =
                    client.DownloadString(string.Format("http://gateway.api.sc/rest/Session/?login={0}&password={1}",
                        _login, _password)).Trim('"');

            }
            return sessionId;
        }
    }
}
