using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdvantShop.Configuration;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Extensions;

namespace AdvantShop.Core.Services.Loging.Emails
{
    partial class ActivityEmailLoger : ActivityEmailNullLoger
    {
        private const string EmailApiPath = "http://activity.advantshop.net/api/";
        private const string EmailApiResourceLog = "emaillog/create";
        private const string EmailApiResourceGetByCustomerid = "emaillog/get/{0}/email/{1}";

        public override void LogEmail(Email email)
        {
            Task.Run(() => LogEmailInternal(email));
        }

        public override List<Email> GetEmails(Guid customerId, string email)
        {
            var client = new RestClient(EmailApiPath);
            var request = new RestRequest(string.Format(EmailApiResourceGetByCustomerid, customerId, email.UrlEncode()), Method.GET);
            request.AddHeader("Authentication", SettingsLic.LicKey);
            request.Timeout = 3000;

            var res = client.Execute<EmailLogResponse>(request);

            return res.Data != null ? res.Data.Data : null;
        }

        private void LogEmailInternal(Email email)
        {
            email.ShopId = SettingsLic.LicKey;

            var client = new RestClient(EmailApiPath);
            var request = new RestRequest(EmailApiResourceLog, Method.POST)
            {
                RequestFormat = DataFormat.Json,
                Timeout = 3000
            };
            request.AddHeader("Authentication", SettingsLic.LicKey);

            var json = JsonConvert.SerializeObject(email);
            request.AddParameter("application/json", json, ParameterType.RequestBody);

            IRestResponse res = client.Execute(request);
            string a = res.Content;
        }

        private class EmailLogResponse
        {
            public int Status { get; set; }
            public string Message { get; set; }
            public List<Email> Data { get; set; }
        }
    }
}
