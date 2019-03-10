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
using AdvantShop.Security.OAuth.Yandex;
using AdvantShop.Security.OpenAuth;
using Newtonsoft.Json;

namespace AdvantShop.Security.OAuth
{
    public class YandexOAuth
    {
        public static string OpenDialog(string pageToRedirect)
        {
            return OAuth.OpenAuthDialog(
                "https://oauth.yandex.ru/authorize",
                SettingsOAuth.YandexClientId,
                pageToRedirect,
                string.Empty,
                "yandex");
        }

        public static bool Login(string code, string pageToRedirect)
        {
            var tokenResponse = OAuth.GetTokenPostRequest<OAuthToken>(
                "https://oauth.yandex.ru/token",
                code,
                SettingsOAuth.YandexClientId,
                SettingsOAuth.YandexClientSecret);
            Debug.Log.Error("GetTokenPostRequest : " + tokenResponse.AccessToken);
            if (string.IsNullOrEmpty(tokenResponse.AccessToken))
            {
                return false;
            }

            var userData = GetUserData(tokenResponse.AccessToken);
            Debug.Log.Error("GetUserData : " + userData.Email);
            OAuthService.AuthOrRegCustomer(new Customer(CustomerGroupService.DefaultCustomerGroup)
            {
                FirstName = userData.FirstName,
                LastName = userData.LastName,
                EMail = userData.Email,
                Password = Guid.NewGuid().ToString()
            }, userData.Id);

            Debug.Log.Error("AuthOrRegCustomer : " + userData.Id);
            return true;
        }

        private static YandexUserData GetUserData(string token)
        {
            var request = WebRequest.Create("https://login.yandex.ru/info?format=json&oauth_token=" + token);
            request.Method = "GET";

            var response = request.GetResponse();

            var str = new StreamReader(response.GetResponseStream()).ReadToEnd();
            Debug.Log.Error("GetUserData string: " + str);
            return JsonConvert.DeserializeObject<YandexUserData>(str);
        }
    }
}
