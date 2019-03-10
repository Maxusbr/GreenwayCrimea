using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.SEO;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;
using Newtonsoft.Json;
using RestSharp;

namespace AdvantShop.Core.Services.Loging.Events
{
    partial class ActivityEventLoger : ActivityEventNullLoger
    {
        private const string EventApiPath = "http://activity.advantshop.net/api/";
        private const string EventApiResourceLog = "eventlog/create";
        private const string EventApiResourceGetByCustomerid = "eventlog/get/{0}";

        public override void AddToCart(ShoppingCartItem item, string url)
        {
            LogEvent(new Event
            {
                Name = item.Offer.Product.Name,
                EvenType = ePageType.addToCart,
                Url = url
            });
        }

        public override void AddToCompare(ShoppingCartItem item, string url)
        {
            LogEvent(new Event
            {
                Name = item.Offer.Product.Name,
                EvenType = ePageType.addToCompare,
                Url = url
            });
        }

        public override void AddToWishList(ShoppingCartItem item, string url)
        {
            LogEvent(new Event
            {
                Name = item.Offer.Product.Name,
                EvenType = ePageType.addToWishlist,
                Url = url
            });
        }

        public override void Search(string searchTerm, int resultsCount)
        {
            LogEvent(new Event
            {
                Name = "",
                EvenType = ePageType.searchresults,
                Url = HttpContext.Current.Request.Url.AbsoluteUri
            });
        }

        public override void LogEvent(Event @event)
        {
            @event.CreateOn = DateTime.Now;
            @event.CustomerId = CustomerContext.CustomerId;
            @event.IP = HttpContext.Current != null ? HttpContext.Current.Request.UserHostAddress : null;

            Task.Run(() => LogEventInternal(@event));
        }

        public override List<Event> GetEvents(Guid customerId)
        {
            var client = new RestClient(EventApiPath);
            var request = new RestRequest(string.Format(EventApiResourceGetByCustomerid, customerId), Method.GET);
            request.AddHeader("Authentication", SettingsLic.LicKey);
            request.Timeout = 5000;

            IRestResponse<EventLogResponse> res = client.Execute<EventLogResponse>(request);

            if (res.ErrorException != null)
                Debug.Log.Error(res.ErrorException);

            return res.Data != null ? res.Data.Data : null;
        }

        private void LogEventInternal(Event @event)
        {
            @event.ShopId = SettingsLic.LicKey;
            if (string.IsNullOrWhiteSpace(@event.Name))
                @event.Name = "none";

            var client = new RestClient(EventApiPath);
            var request = new RestRequest(EventApiResourceLog, Method.POST)
            {
                RequestFormat = DataFormat.Json,
                Timeout = 3000
            };
            request.AddHeader("Authentication", SettingsLic.LicKey);

            var json = JsonConvert.SerializeObject(@event);
            request.AddParameter("application/json", json, ParameterType.RequestBody);

            IRestResponse res = client.Execute(request);
            string a = res.Content;
        }

        private class EventLogResponse
        {
            public int Status { get; set; }
            public string Message { get; set; }
            public List<Event> Data { get; set; }
        }
    }
}