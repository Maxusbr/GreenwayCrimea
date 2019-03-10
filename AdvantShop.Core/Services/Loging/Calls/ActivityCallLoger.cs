using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdvantShop.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace AdvantShop.Core.Services.Loging.Calls
{
    partial class ActivityCallLoger : ActivityCallNullLoger
    {
        private const string CallApiPath = "http://activity.advantshop.net/api/";
        private const string CallApiResourceLog = "calllog/create";
        private const string CallApiResourceGetByCustomerid = "calllog/get/{0}/call/{1}";

        public override void LogCall(Call call)
        {
            Task.Run(() => LogCallInternal(call));
        }

        public override List<Call> GetCalls(Guid customerId, string call)
        {
            var client = new RestClient(CallApiPath);
            var request = new RestRequest(string.Format(CallApiResourceGetByCustomerid, customerId, call), Method.GET);
            request.AddHeader("Authentication", SettingsLic.LicKey);
            request.Timeout = 3000;
            
            var res = client.Execute<CallLogResponse>(request);

            return res.Data != null ? res.Data.Data : null;
        }
        
        private void LogCallInternal(Call call)
        {
            call.ShopId = SettingsLic.LicKey;
            
            var client = new RestClient(CallApiPath);
            var request = new RestRequest(CallApiResourceLog, Method.POST)
            {
                RequestFormat = DataFormat.Json,
                Timeout = 3000
            };
            request.AddHeader("Authentication", SettingsLic.LicKey);

            var json = JsonConvert.SerializeObject(call);
            request.AddParameter("application/json", json, ParameterType.RequestBody);

            IRestResponse res = client.Execute(request);
            string a = res.Content;
        }

        private class CallLogResponse
        {
            public int Status { get; set; }
            public string Message { get; set; }
            public List<Call> Data { get; set; }
        }
    }
}
