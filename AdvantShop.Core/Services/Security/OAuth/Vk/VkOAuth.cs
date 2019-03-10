//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

using System.IO;
using System.Linq;
using System.Net;
using AdvantShop.Configuration;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Security.OAuth.Vk;
using AdvantShop.Security.OpenAuth;
using Newtonsoft.Json;

namespace AdvantShop.Security.OAuth
{
    public class VkOAuth
    {
        public static string OpenDialog(string pageToRedirect)
        {
            return OAuth.OpenAuthDialog(
                "https://oauth.vk.com/authorize",
                SettingsOAuth.VkontakeClientId,
                pageToRedirect,
                "email,offline",
                "vk");
        }

        public static bool Login(string code, string pageToRedirect)
        {
            try
            {
                var tokenResponse = OAuth.GetTokenPostRequest<VkOAuthToken>(
                    "https://oauth.vk.com/access_token",
                    code,
                    SettingsOAuth.VkontakeClientId,
                    SettingsOAuth.VkontakeSecret);

                if (string.IsNullOrEmpty(tokenResponse.AccessToken))
                    return false;

                var userData = GetUserData(tokenResponse.AccessToken, tokenResponse.UserId);
                if (userData == null)
                    return false;

                OAuthService.AuthOrRegCustomer(
                    new Customer(CustomerGroupService.DefaultCustomerGroup)
                    {
                        FirstName = userData.FirstName,
                        LastName = userData.LastName,
                        EMail = !string.IsNullOrEmpty(tokenResponse.Email) ? tokenResponse.Email : tokenResponse.UserId + "@temp.vk",
                        Password = Guid.NewGuid().ToString()
                    },
                    userData.Id);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return false;
            }

            return true;
        }

        private static VkUserData GetUserData(string token, string userId)
        {
            var request = WebRequest.Create(string.Format("https://api.vk.com/method/getProfiles?uid={0}&access_token={1}&v=5.73", userId, token));
            request.Method = "GET";

            var response = request.GetResponse();
            var json = new StreamReader(response.GetResponseStream()).ReadToEnd();
            
            var users = JsonConvert.DeserializeObject<VkUserDataResponse>(json);

            if (users != null && users.UsersList != null && users.UsersList.Count > 0)
                return users.UsersList[0];

            return null;
        }
    }
}
