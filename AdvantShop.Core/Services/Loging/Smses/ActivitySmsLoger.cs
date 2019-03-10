using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdvantShop.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace AdvantShop.Core.Services.Loging.Smses
{
    partial class ActivitySmsLoger : ActivitySmsNullLoger
    {
        private const string SmsApiPath = "http://activity.advantshop.net/api/";
        private const string SmsApiResourceLog = "smslog/create";
        private const string SmsApiResourceGetByCustomerid = "smslog/get/{0}/phone/{1}";
        
        public override void LogSms(TextMessage message)
        {
            Task.Run(() => LogEmailInternal(message));
        }

        public override List<TextMessage> GetSms(Guid customerId, long phone)
        {
            var client = new RestClient(SmsApiPath);
            var request = new RestRequest(string.Format(SmsApiResourceGetByCustomerid, customerId, phone), Method.GET);
            request.AddHeader("Authentication", SettingsLic.LicKey);
            request.Timeout = 3000;

            var res = client.Execute<SmsLogResponse>(request);
            return res.Data != null ? res.Data.Data : null;
        }

        private void LogEmailInternal(TextMessage message)
        {
            message.ShopId = SettingsLic.LicKey;

            var client = new RestClient(SmsApiPath);
            var request = new RestRequest(SmsApiResourceLog, Method.POST)
            {
                RequestFormat = DataFormat.Json,
                Timeout = 3000
            };
            request.AddHeader("Authentication", SettingsLic.LicKey);

            var json = JsonConvert.SerializeObject(message);
            request.AddParameter("application/json", json, ParameterType.RequestBody);

            IRestResponse res = client.Execute(request);
            string a = res.Content;
        }

        private class SmsLogResponse
        {
            public int Status { get; set; }
            public string Message { get; set; }
            public List<TextMessage> Data { get; set; }
        }

    }
}
