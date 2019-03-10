//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

using AdvantShop.Configuration;
using AdvantShop.Customers;
using AdvantShop.Security.OAuth.Mail;
using AdvantShop.Security.OpenAuth;
using Newtonsoft.Json;

namespace AdvantShop.Security.OAuth
{
    public class MailOAuth
    {
        public static string OpenDialog(string pageToRedirect)
        {
            return OAuth.OpenAuthDialog("https://connect.mail.ru/oauth/authorize",
                SettingsOAuth.MailAppId,
                pageToRedirect,
                string.Empty,
                "mail");
        }

        public static bool Login(string code, string pageToRedirect)
        {
            var tokenResponse = OAuth.GetTokenPostRequest<MailOAuthToken>(
               "https://connect.mail.ru/oauth/token",
               code,
               SettingsOAuth.MailAppId,
               SettingsOAuth.MailClientSecret);

            var userData = GetUserData(tokenResponse.AccessToken);
            if (userData == null)
            {
                return false;
            }

            OAuthService.AuthOrRegCustomer(new Customer(CustomerGroupService.DefaultCustomerGroup)
            {
                FirstName = userData.FirstName,
                LastName = userData.LastName,
                EMail = userData.Email,
                Password = Guid.NewGuid().ToString()
            }, userData.Id);

            return true;
        }

        private static MailUserData GetUserData(string token)
        {
            var data = string.Format("app_id={0}method=users.getInfosecure=1session_key={1}",
                SettingsOAuth.MailAppId,
                token);

            var request = WebRequest.Create(
                string.Format("http://www.appsmail.ru/platform/api?method=users.getInfo&app_id={0}&session_key={1}&secure=1&sig={2}",
                SettingsOAuth.MailAppId,
                token,
                OAuth.GetMd5Hash(data + SettingsOAuth.MailClientSecret)));

            request.Method = "GET";

            var response = request.GetResponse();

            var str = new StreamReader(response.GetResponseStream()).ReadToEnd();

            //возвращается массив, берем первый
            var users = JsonConvert.DeserializeObject<List<MailUserData>>(str);
            if (users != null && users.Any())
            {
                return users[0];
            }

            return null;
        }
    }
}
