//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using Newtonsoft.Json;

namespace AdvantShop.Security.OAuth
{
    class OAuth
    {
        public static string OpenAuthDialog(string url, string clientId, string pageToRedirect, string scope, string state)
        {
            return string.Format(url + "?response_type=code&client_id={0}&redirect_uri={1}&scope={2}&state={3}",
                clientId,
                HttpUtility.UrlEncode(StringHelper.MakeASCIIUrl(UrlService.GetUrl("login"))),
                scope,
                state + "," + pageToRedirect + "," + Customers.CustomerContext.CurrentCustomer.Id);
        }

        public static T GetTokenGetRequest<T>(string url, string code, string clientId, string clientSecret, string redirectUri)
        {
            var request =
                WebRequest.Create(
                    string.Format(url + "?grant_type=authorization_code&code={0}&client_id={1}&client_secret={2}&redirect_uri={3}",
                                        code, clientId, clientSecret, redirectUri));

            request.Method = "GET";

            var response = request.GetResponse();

            var str = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return JsonConvert.DeserializeObject<T>(str);
        }

        public static T GetTokenPostRequest<T>(string url, string code, string clientId, string clientSecret) where T : class, new()
        {
            var dataStr =
                string.Format(
                    "grant_type=authorization_code&code={0}&client_id={1}&client_secret={2}&redirect_uri={3}",
                    code, 
                    clientId, 
                    clientSecret, 
                    StringHelper.MakeASCIIUrl(UrlService.GetUrl("login")));

            var responseResult = "";

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                var data = Encoding.UTF8.GetBytes(dataStr);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.KeepAlive = false;
                request.ServicePoint.ConnectionLimit = 1;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
                //request.ProtocolVersion = HttpVersion.Version10;

                using (var stream = request.GetRequestStream())
                    stream.Write(data, 0, data.Length);

                var response = request.GetResponse();
                responseResult = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch(WebException ex)
            {
                var result = "";

                using (var eResponse = ex.Response)
                {
                    if (eResponse != null)
                    {
                        using (var eStream = eResponse.GetResponseStream())
                        {
                            if (eStream != null)
                                using (var reader = new StreamReader(eStream))
                                {
                                    result = reader.ReadToEnd();
                                    Debug.Log.Error(result);
                                }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(
                    string.Format("InnerException: {0}; StackTrace: {1}; url: {2}; data: {3};",
                        ex.InnerException, ex.StackTrace, url, dataStr));
            }

            return responseResult != "" ? JsonConvert.DeserializeObject<T>(responseResult) : new T();
        }

        public static T RefreshToken<T>(string url, string refreshToken, string clientId, string clientSecret) where T : class, new()
        {
            var dataStr =
                string.Format(
                    "client_id={1}&client_secret={2}&refresh_token={0}&grant_type=refresh_token",
                    refreshToken, clientId, clientSecret);

            var responseResult = "";

            try
            {
                var request = WebRequest.Create(url);
                var data = Encoding.UTF8.GetBytes(dataStr);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;                

                using (var stream = request.GetRequestStream())
                    stream.Write(data, 0, data.Length);

                var response = request.GetResponse();
                responseResult = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (Exception ex)
            {
                Debug.Log.Error(
                    string.Format("InnerException: {0}; StackTrace: {1}; url: {2}; data: {3};",
                        ex.InnerException, ex.StackTrace, url, dataStr));
            }

            return responseResult != "" ? JsonConvert.DeserializeObject<T>(responseResult) : new T();
        }

        public static string GetMd5Hash(string input)
        {
            byte[] data = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input));

            var sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}
