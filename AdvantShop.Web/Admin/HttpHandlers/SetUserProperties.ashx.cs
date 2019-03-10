using AdvantShop.Admin.UserControls.UserInformation;
using AdvantShop.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace AdvantShop.Admin.HttpHandlers
{
    public class SetUserProperties : IHttpHandler
    {
        const string Url = "http://modules.advantshop.net/";
        public void ProcessRequest(HttpContext context)
        {
            string jsonString = String.Empty;

            HttpContext.Current.Request.InputStream.Position = 0;
            using (StreamReader inputStream = new StreamReader(HttpContext.Current.Request.InputStream))
            {
                jsonString = inputStream.ReadToEnd();
            }

            var data = JsonConvert.DeserializeObject<AdditionClientInfo>(jsonString);

            var client = new RestClient(Url);
            var request = new RestRequest(string.Format("Shop/SetClientProperty/{0}", SettingsLic.LicKey), Method.POST);
            request.Timeout = 3000;
            request.AddJsonBody(data);
            var model = client.Execute(request);

            context.Response.Write("true");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}