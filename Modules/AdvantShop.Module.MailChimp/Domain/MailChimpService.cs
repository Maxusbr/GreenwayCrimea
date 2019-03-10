//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using AdvantShop.Customers;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using AdvantShop.Diagnostics;

namespace AdvantShop.Modules
{
    public class MailChimpService
    {
        public enum TypeCompaign
        {
            regular,
            plaintext,
            absplit,
            rss,
            auto
        }

        public enum MemberStatus
        {
            subscribed,
            unsubscribed,
            cleaned,
            updated
        }

        /// <summary>
        /// 2.0
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public static MailChimpLists GetLists(string apiKey)
        {
            var lists = new MailChimpLists { total = 0, data = new List<MailChimpList>() };

            var responseString = PostRequest("lists/list", string.Format("{{\"apikey\":\"{0}\"}}", apiKey));
            lists = JsonConvert.DeserializeObject<MailChimpLists>(responseString);
            if (lists != null && lists.data != null)
            {
                lists.data.Insert(0,
                    new MailChimpList
                    {
                        name = CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ru" ? "Нет привязки к списку" : "No binding to the list",
                        id = "0"
                    });
            }

            return lists;
        }

        /// <summary>
        /// 2.0
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="listid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static MailChimpListMembers GetListMembers(string apiKey, string listid, MemberStatus status = MemberStatus.subscribed)
        {
            var listMembersLists = new MailChimpListMembers();

            var responseString = PostRequest("lists/members",
                        string.Format("{{\"apikey\":\"{0}\",\"id\":\"{1}\",\"status\":\"{2}\"}}", apiKey, listid,
                                      status));

            listMembersLists = JsonConvert.DeserializeObject<MailChimpListMembers>(responseString);

            return listMembersLists;
        }

        public static bool SubscribeListMembers(string apiKey, string listId, List<string> members)
        {
            return SubscribeListMembers(apiKey, listId, members.Select(member => (ISubscriber)(new MailChimpListSubscriber { Email = member })).ToList());
        }

        /// <summary>
        /// 2.0
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="listId"></param>
        /// <param name="members"></param>
        /// <returns></returns>
        public static bool SubscribeListMembers(string apiKey, string listId, List<ISubscriber> members)
        {
            var batch = members.Aggregate(string.Empty, (current, member) => current +
                string.Format("{{\"email\":{{\"email\":\"{0}\",\"euid\":\"\",\"leid\":\"\"}},\"email_type\":\"html\",\"merge_vars\":{{\"EMAIL\":\"{0}\",\"FNAME\":\"{1}\",\"LNAME\":\"{2}\" }} }},",
                member.Email,
                !string.IsNullOrEmpty(member.FirstName) ? member.FirstName : string.Empty,
                !string.IsNullOrEmpty(member.LastName) ? member.LastName : string.Empty));

            var responseString = PostRequest("lists/batch-subscribe",
                        string.Format(
                            "{{\"apikey\":\"{0}\",\"id\":\"{1}\",\"batch\":[{2}],\"double_optin\":\"false\"}}",
                            apiKey,
                            listId,
                            batch.TrimEnd(new[] { ',' })));

            return true;
        }

        /// <summary>
        /// 2.0 overload
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="listId"></param>
        /// <returns></returns>
        public static bool UnsubscribeListMembers(string accountId, string listId)
        {
            if (string.IsNullOrEmpty(accountId) || string.IsNullOrEmpty(listId))
            {
                return false;
            }
            return UnsubscribeListMembers(accountId, listId, GetListMembers(accountId, listId));
        }

        /// <summary>
        /// 2.0
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="listId"></param>
        /// <param name="members"></param>
        /// <param name="deleteMember"></param>
        /// <param name="sendGoodbye"></param>
        /// <param name="sendNotify"></param>
        /// <returns></returns>
        public static bool UnsubscribeListMembers(string apiKey, string listId, MailChimpListMembers members, bool deleteMember = true, bool sendGoodbye = false, bool sendNotify = false)
        {
            if (members == null || members.Data == null)
            {
                return false;
            }

            var batch_array = members.Data.Aggregate(string.Empty, (current, member) => current + string.Format("{{\"email\":\"{0}\",\"euid:\"{1}\",\"leid\":\"{2}\"}},", member.email, member.euid, member.leid));

            var responseString = PostRequest("lists/batch-unsubscribe",
                        string.Format(
                            "{{\"apikey\":\"{0}\",\"id\":\"{1}\",\" batch\":[{2}],\"delete_member\":\"{3}\",\"send_goodbye\":\"{4}\",\"send_notify\":\"{5}\"}}",
                            apiKey,
                            listId,
                            batch_array,
                            deleteMember,
                            sendGoodbye,
                            sendNotify));

            return true;
        }

        /// <summary>
        /// 2.0 ----
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="listId"></param>
        /// <param name="subject"></param>
        /// <param name="fromEmail"></param>
        /// <param name="fromName"></param>
        /// <param name="toName"></param>
        /// <param name="htmlContent"></param>
        /// <param name="textContent"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string CreateCampaign(string apiKey, string listId, string subject, string fromEmail, string fromName, string toName, string htmlContent, string textContent = "", TypeCompaign type = TypeCompaign.regular)
        {
            var mailchimpCampaign = new MailchimpCreateCampaignObject
            {
                apikey = apiKey,
                type = type.ToString(),
                options = new MailchimpCreateCampaignObjectOptions
                {
                    list_id = listId,
                    subject = subject.Length > 150 ? subject.Substring(0, 149) : subject,
                    from_email = fromEmail,
                    from_name = fromName,
                    to_name = toName
                },
                content = new MailchimpCreateCampaignObjectContent
                {
                    html = htmlContent
                }
            };

            var responseString = PostRequest("campaigns/create",
                                             JsonConvert.SerializeObject(mailchimpCampaign));

            var campaign = JsonConvert.DeserializeObject<MailchimpCampaign>(responseString);
            if (campaign != null)
            {
                return campaign.id;
            }

            return string.Empty;
        }

        /// <summary>
        /// 2.0
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="listId"></param>
        /// <param name="subject"></param>
        /// <param name="fromEmail"></param>
        /// <param name="fromName"></param>
        /// <param name="toName"></param>
        /// <param name="htmlContent"></param>
        /// <param name="textContent"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool SendMail(string apiKey, string listId, string subject,
            string fromEmail, string fromName,
            string toName, string htmlContent, string textContent = "", TypeCompaign type = TypeCompaign.regular)
        {

            fromName = fromName.Replace("\"", "");
            var compaignId = CreateCampaign(apiKey, listId, subject, fromEmail, fromName, toName, htmlContent, textContent, type);

            if (string.IsNullOrEmpty(compaignId))
            {
                return false;
            }

            return SendMail(apiKey, compaignId);
        }

        /// <summary>
        /// 2.0
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="compaignId"></param>
        /// <returns></returns>
        public static bool SendMail(string apiKey, string compaignId)
        {
            if (string.IsNullOrEmpty(compaignId))
            {
                return false;
            }

            string errorString = string.Empty;

            var responseString = PostRequest("campaigns/send", string.Format("{{\"apikey\": \"{0}\",\"cid\": \"{1}\"}}", apiKey, compaignId));
            responseString = responseString
                .Replace("\n", string.Empty)
                .Replace("\"", string.Empty)
                .Replace("{", string.Empty)
                .Replace("}", string.Empty);
            foreach (var pair in responseString.Split(new[] { ',' }))
            {
                var keyValue = pair.Split(new[] { ':' });
                if (keyValue.Count() > 1 && string.Equals(keyValue[0], "complete") &&
                    string.Equals(keyValue[1], "true"))
                {
                    return true;
                }
                else if (keyValue.Count() > 1 && string.Equals(keyValue[0], "error"))
                {
                    errorString = keyValue[1];
                }
            }

            return false;
        }

        /// <summary>
        /// 2.0
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        public static MailChimpAccount GetAccountDetails(string apiKey)
        {
            MailChimpAccount account = null;

            var responseString = PostRequest("helper/account-details", string.Format("{{\"apikey\":\"{0}\"}}", apiKey));
            account = JsonConvert.DeserializeObject<MailChimpAccount>(responseString);

            return account;
        }

        public static bool PingMailchimpAccount(string apiKey)
        {
            var responseString = PostRequest("helper/ping", string.Format("{{\"apikey\":\"{0}\"}}", apiKey));
            var responce = JsonConvert.DeserializeObject<MailChimpErrors>(responseString);
            return responce != null && string.IsNullOrEmpty(responce.Error);
        }

        /// <summary>
        /// additional function
        /// </summary>
        /// <param name="apiKey"></param>
        /// <returns></returns>
        private static string GetApiDc(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey) || !apiKey.Contains("-") || apiKey.LastIndexOf("-") + 1 >= apiKey.Length)
            {
                return string.Empty;
            }

            return apiKey.Substring(apiKey.LastIndexOf("-") + 1, apiKey.Length - apiKey.LastIndexOf("-") - 1);
        }

        /// <summary>
        /// additional function
        /// </summary>
        /// <param name="method"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static string PostRequest(string method, string data)
        {
            var dc = GetApiDc(MailChimpSettings.ApiKey);
            if (string.IsNullOrEmpty(dc)) return string.Empty;

            var request = (HttpWebRequest)WebRequest.Create(string.Format("https://{0}.api.mailchimp.com/2.0/{1}", dc, method));

            request.Method = "POST";
            request.Timeout = 5000;
            request.ContentType = "application/x-www-form-urlencoded";

            try
            {
                var bytes = Encoding.UTF8.GetBytes(data);

                using (var newStream = request.GetRequestStream())
                {
                    newStream.Write(bytes, 0, bytes.Length);
                }

                var responseStream = request.GetResponse().GetResponseStream();
                if (responseStream != null)
                {
                    return new StreamReader(responseStream).ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                Debug.Log.Error(ex);
                //var result = "";

                //using (var eResponse = ex.Response)
                //{
                //    if (eResponse != null)
                //    {
                //        using (var eStream = eResponse.GetResponseStream())
                //        {
                //            if (eStream != null)
                //                using (var reader = new StreamReader(eStream))
                //                {
                //                    result = reader.ReadToEnd();
                //                }
                //        }
                //    }
                //}
            }

            return string.Empty;
        }
    }
}