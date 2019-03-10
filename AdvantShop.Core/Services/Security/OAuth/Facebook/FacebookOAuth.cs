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
using AdvantShop.Security.OAuth.Facebook;
using AdvantShop.Security.OpenAuth;
using Newtonsoft.Json;

namespace AdvantShop.Security.OAuth
{
    public class FacebookOAuth
    {
        public static string OpenDialog(string pageToRedirect)
        {
            return OAuth.OpenAuthDialog(
                "https://www.facebook.com/dialog/oauth?",
                SettingsOAuth.FacebookClientId,
                pageToRedirect,
                "public_profile,email",
                "fb");
        }

        public static bool Login(string code, string pageToRedirect)
        {
            var tokenResponse = OAuth.GetTokenPostRequest<OAuthToken>(
                "https://graph.facebook.com/v2.3/oauth/access_token",
                code,
                SettingsOAuth.FacebookClientId,
                SettingsOAuth.FacebookApplicationSecret);

            if (string.IsNullOrEmpty(tokenResponse.AccessToken))
            {
                return false;
            }

            var userData = GetUserData(tokenResponse.AccessToken);

            if (userData == null)
            {
                return false;
            }

            OAuthService.AuthOrRegCustomer(new Customer(CustomerGroupService.DefaultCustomerGroup)
            {
                FirstName = userData.FirstName,
                LastName = userData.LastName,
                EMail = !string.IsNullOrEmpty(userData.Email) ? userData.Email : userData.Id + "@temp.fb",
                Password = Guid.NewGuid().ToString()
            }, userData.Id);

            return true;
        }

        private static FacebookUserData GetUserData(string token)
        {
            var request =
                WebRequest.Create("https://graph.facebook.com/me?access_token=" + token + "&fields=id,name,email,first_name,last_name");

            request.Method = "GET";

            var response = request.GetResponse();
            string str = new StreamReader(response.GetResponseStream()).ReadToEnd();

            Debug.Log.Error("ответ от сервера на получение юзера: " + str);
            return JsonConvert.DeserializeObject<FacebookUserData>(str);
        }
    }
}
