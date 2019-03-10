//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.IO;
using System.Net;
using AdvantShop.Configuration;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Security.OAuth.Goolgle;
using AdvantShop.Security.OpenAuth;
using Newtonsoft.Json;
using System.Web;

namespace AdvantShop.Security.OAuth
{
    public class GoogleOAuth
    {
        public static string OpenDialog(string pageToRedirect)
        {
            return OAuth.OpenAuthDialog(
                "https://accounts.google.com/o/oauth2/auth",
                SettingsOAuth.GoogleClientId,
                StringHelper.MakeASCIIUrl(UrlService.GetUrl(pageToRedirect)),
                "email%20profile",
                "google");
        }

        public static bool Login(string code, string pageToRedirect)
        {
            var tokenResponse = OAuth.GetTokenPostRequest<OAuthToken>(
                "https://www.googleapis.com/oauth2/v3/token",
                code,
                SettingsOAuth.GoogleClientId,
                SettingsOAuth.GoogleClientSecret);

            if (string.IsNullOrEmpty(tokenResponse.AccessToken))
                return false;

            var userData = GetUserData(tokenResponse.AccessToken);

            if (userData == null)
                return false;

            OAuthService.AuthOrRegCustomer(new Customer(CustomerGroupService.DefaultCustomerGroup)
            {
                FirstName = userData.FirstName,
                LastName = userData.LastName,
                EMail = userData.Email,
                Password = Guid.NewGuid().ToString()
            }, userData.Id);

            return true;
        }

        private static GoogleUserData GetUserData(string token)
        {
            var request = WebRequest.Create("https://www.googleapis.com/oauth2/v3/userinfo?access_token=" + token);
            request.Method = "GET";

            var response = request.GetResponse();

            var str = new StreamReader(response.GetResponseStream()).ReadToEnd();
            
            return JsonConvert.DeserializeObject<GoogleUserData>(str);
        }

        public static string OpenAnalyticsDialog(string pageToRedirect)
        {
            // doc: https://developers.google.com/identity/protocols/OAuth2WebServer

            return string.Format(
                "https://accounts.google.com/o/oauth2/auth?response_type=code&access_type=offline&client_id={0}&redirect_uri={1}&scope={2}&state={3}&approval_prompt=force",
                SettingsOAuth.GoogleClientId,
                HttpUtility.UrlEncode(StringHelper.MakeASCIIUrl(UrlService.GetUrl("login"))), // <- redirect_uri
                HttpUtility.UrlEncode("https://www.googleapis.com/auth/analytics.readonly https://www.googleapis.com/auth/userinfo.email"), // <- scope,
                "googleanalytics," + pageToRedirect);
        }

        public static bool LoginAnalytics(string code, string pageToRedirect)
        {
            var tokenResponse =
                OAuth.GetTokenPostRequest<OAuthToken>("https://www.googleapis.com/oauth2/v3/token", 
                    code,
                    SettingsOAuth.GoogleClientId, 
                    SettingsOAuth.GoogleClientSecret);
            
            if (string.IsNullOrEmpty(tokenResponse.AccessToken))
                return false;

            SettingsOAuth.GoogleAnalyticsAccessToken = JsonConvert.SerializeObject(tokenResponse);
            return true;
        }
    }
}
