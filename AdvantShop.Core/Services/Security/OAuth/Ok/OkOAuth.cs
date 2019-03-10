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
using AdvantShop.Helpers;
using AdvantShop.Security.OAuth.Mail;
using AdvantShop.Security.OpenAuth;
using Newtonsoft.Json;

namespace AdvantShop.Security.OAuth
{
    public class OkOAuth
    {
        public static string OpenDialog(string pageToRedirect)
        {
            return OAuth.OpenAuthDialog("http://www.odnoklassniki.ru/oauth/authorize",
                SettingsOAuth.OdnoklassnikiClientId,
                pageToRedirect,
                "VALUABLE_ACCESS",
                "ok");
        }

        public static bool Login(string code, string pageToRedirect)
        {
            var tokenResponse = OAuth.GetTokenPostRequest<OAuthToken>(
                "https://api.odnoklassniki.ru/oauth/token.do",
                code,
                SettingsOAuth.OdnoklassnikiClientId,
                SettingsOAuth.OdnoklassnikiSecret);

            var userData = GetUserData(tokenResponse.AccessToken);

            if (userData == null || string.IsNullOrEmpty(userData.Id))
            {
                return false;
            }

            OAuthService.AuthOrRegCustomer(new Customer(CustomerGroupService.DefaultCustomerGroup)
            {
                FirstName = userData.FirstName,
                LastName = userData.LastName,
                EMail = userData.Id + "@temp.odnoklassniki",
                Password = Guid.NewGuid().ToString()
            }, userData.Id);

            return true;
        }

        private static OkUserData GetUserData(string token)
        {
            var data = string.Format("application_key={0}method=users.getCurrentUser{1}",
                SettingsOAuth.OdnoklassnikiPublicApiKey,
                OAuth.GetMd5Hash(token + SettingsOAuth.OdnoklassnikiSecret));

            var request =
               WebRequest.Create(string.Format("http://api.odnoklassniki.ru/fb.do?method=users.getCurrentUser&access_token={0}&application_key={1}&sig={2}",
               token,
               SettingsOAuth.OdnoklassnikiPublicApiKey,
                OAuth.GetMd5Hash(data).ToLower()));

            request.Method = "GET";
            request.ContentType = "applcation/json";

            var response = request.GetResponse();

            return JsonConvert.DeserializeObject<OkUserData>(new StreamReader(response.GetResponseStream()).ReadToEnd());
        }
    }
}
