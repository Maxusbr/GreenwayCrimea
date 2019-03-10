using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp;

namespace AdvantShop.Core.Services.Loging.TrafficSource
{
    public class ActivityTrafficSourceLoger : ActivityTrafficSourceNullLoger
    {
        private const string ApiPath = "http://activity.advantshop.net/api/";
        private const string ApiLogPath = "trafficsourcelog/create";
        private const string ApiGetPath = "trafficsourcelog/get/{0}";
        private const string ApiDeletePath = "trafficsourcelog/delete/{0}";

        // кука последнего визита, чобы не логировать прямые переходы в течении сессии (20мин)
        private const string TrafficSourceCookieName = "advs_v";

        public override void LogTrafficSource()
        {
            var request = HttpContext.Current.Request;
            var referrer = request.UrlReferrer;

            // Логируем источник только если реферрер либо пустой, либо не является страницей сайта (то есть переход с внешнего источника)
            if (referrer == null ||
                referrer.Host.IndexOf(CommonHelper.GetParentDomain(), StringComparison.InvariantCultureIgnoreCase) == -1)
            {
                if (referrer != null || string.IsNullOrEmpty(CommonHelper.GetCookieString(TrafficSourceCookieName)))
                {
                    var source = GetSource(referrer, request);

                    // send to activity
                    Task.Run(() =>
                    {
                        var client = new RestClient(ApiPath);
                        var restRequest = new RestRequest(ApiLogPath, Method.POST)
                        {
                            RequestFormat = DataFormat.Json,
                            Timeout = 3000
                        };
                        restRequest.AddHeader("Authentication", SettingsLic.LicKey);

                        var json = JsonConvert.SerializeObject(source, new JsonSerializerSettings
                        {
                            Formatting = Formatting.Indented,
                            ContractResolver = new DefaultNameContractResolver()
                        });
                        restRequest.AddParameter("application/json", json, ParameterType.RequestBody);

                        var res = client.Execute(restRequest);
                        var a = res.Content;
                    });
                    
                    CommonHelper.SetCookie(TrafficSourceCookieName, "1", new TimeSpan(0, 0, 20, 0), false);
                }
            }
        }

        public override TrafficSource GetTrafficSourceByRule()
        {
            var client = new RestClient(ApiPath);
            var request = new RestRequest(string.Format(ApiGetPath, CustomerContext.CustomerId), Method.GET);
            request.AddHeader("Authentication", SettingsLic.LicKey);
            request.Timeout = 3000;

            var res = client.Execute<TrafficSourceResponse>(request);
            var data = res.Data != null ? res.Data.Data : null;

            if (data == null)
                return null;

            var sources = data.OrderByDescending(x => x.CreateOn).ToList();

            // последний непрямой
            var source = sources.FirstOrDefault(x => !string.IsNullOrEmpty(x.Referrer));
            if (source != null)
                return source;

            // последний
            source = sources.FirstOrDefault();
            
            return source;
        }

        /// <summary>
        /// remove sources from activity
        /// </summary>
        public override void ClearTrafficSources()
        {
            CommonHelper.DeleteCookie(TrafficSourceCookieName);

            var customerId = CustomerContext.CustomerId;

            Task.Run(() =>
            {
                var client = new RestClient(ApiPath);
                var restRequest = new RestRequest(string.Format(ApiDeletePath, customerId), Method.DELETE);
                restRequest.AddHeader("Authentication", SettingsLic.LicKey);

                var res = client.Execute(restRequest);
                var a = res.Content;
            });
        }

        private class TrafficSourceResponse
        {
            public int Status { get; set; }
            public string Message { get; set; }
            public List<TrafficSource> Data { get; set; }
        }
    }

    /// <summary>
    /// Помогает игнорировать JsonProperty PropertyName 
    /// </summary>
    public class DefaultNameContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            // Let the base class create all the JsonProperties 
            // using the short names
            IList<JsonProperty> list = base.CreateProperties(type, memberSerialization);

            // Now inspect each property and replace the 
            // short name with the real property name
            foreach (JsonProperty prop in list)
            {
                prop.PropertyName = prop.UnderlyingName;
            }

            return list;
        }
    }
}