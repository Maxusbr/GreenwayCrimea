//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------


using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Core.SQL;
using AdvantShop.Diagnostics;
using AdvantShop.Saas;
using Newtonsoft.Json;

namespace AdvantShop.Notifications
{
    public class AdminMessagesService
    {
        #region requests to remount server
        private const string Url_GetAdminMessagesIds = "http://modules.advantshop.net/AdminMessage/GetActiveAdminMessagesIds/{0}";
        private const string Url_GetAdminMessages = "http://modules.advantshop.net/AdminMessage/GetAdminMessages/{0}";
        private const string Url_GetListAdminMessages = "http://modules.advantshop.net/AdminMessage/GetListActiveAdminMessages/{0}";
        private const string Url_GetAdminMessage = "http://modules.advantshop.net/AdminMessage/GetAdminMessage?id={0}&amId={1}";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static AdminMessageBox GetAdminMessagesFromRemoteServer()
        {
            var adminMessages = new AdminMessageBox();
            try
            {
                var request = WebRequest.Create(string.Format(Url_GetAdminMessages, SaasDataService.IsSaasEnabled ? SettingsGeneral.CurrentSaasId : SettingsLic.LicKey));
                request.Method = "GET";

                using (var dataStream = request.GetResponse().GetResponseStream())
                {
                    using (var reader = new StreamReader(dataStream))
                    {
                        var responseFromServer = reader.ReadToEnd();
                        if (!string.IsNullOrEmpty(responseFromServer))
                        {
                            adminMessages = JsonConvert.DeserializeObject<AdminMessageBox>(responseFromServer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
            return adminMessages;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static AdminMessageBox GetAdminMessagesWithoutMessagesFromRemoteServer()
        {
            var adminMessages = new AdminMessageBox();
            try
            {
                var request = WebRequest.Create(string.Format(Url_GetListAdminMessages, SaasDataService.IsSaasEnabled ? SettingsGeneral.CurrentSaasId : SettingsLic.LicKey));
                request.Method = "GET";

                using (var dataStream = request.GetResponse().GetResponseStream())
                {
                    using (var reader = new StreamReader(dataStream))
                    {
                        var responseFromServer = reader.ReadToEnd();
                        if (!string.IsNullOrEmpty(responseFromServer))
                        {
                            adminMessages = JsonConvert.DeserializeObject<AdminMessageBox>(responseFromServer);
                            adminMessages.Items = adminMessages.Items.OrderByDescending(item => item.DateChange).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
            return adminMessages;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static List<int> GetAdminMessagesIdsFromRemoteServer()
        {
            var activAdminMessagesIds = new List<int>();
            try
            {
                var request = WebRequest.Create(string.Format(Url_GetAdminMessagesIds, SaasDataService.IsSaasEnabled ? SettingsGeneral.CurrentSaasId : SettingsLic.LicKey));
                request.Method = "GET";

                using (var dataStream = request.GetResponse().GetResponseStream())
                {
                    using (var reader = new StreamReader(dataStream))
                    {
                        var responseFromServer = reader.ReadToEnd();
                        if (!string.IsNullOrEmpty(responseFromServer))
                        {
                            activAdminMessagesIds = JsonConvert.DeserializeObject<List<int>>(responseFromServer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
            return activAdminMessagesIds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static AdminMessageBox GetAdminMessageFromRemoteServer(int amId)
        {
            var adminMessages = new AdminMessageBox();
            try
            {
                var request = WebRequest.Create(string.Format(Url_GetAdminMessage, SaasDataService.IsSaasEnabled ? SettingsGeneral.CurrentSaasId : SettingsLic.LicKey, amId));
                request.Method = "GET";

                using (var dataStream = request.GetResponse().GetResponseStream())
                {
                    using (var reader = new StreamReader(dataStream))
                    {
                        var responseFromServer = reader.ReadToEnd();
                        if (!string.IsNullOrEmpty(responseFromServer))
                        {
                            adminMessages = JsonConvert.DeserializeObject<AdminMessageBox>(responseFromServer);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
            return adminMessages;
        }

        #endregion

        public static AdminMessageBox GetAdminMessage(int id)
        {
            return GetAdminMessageFromRemoteServer(id);
        }

        public static AdminMessageBox GetAdminMessages()
        {
            var adminMessagesBox = GetAdminMessagesFromRemoteServer();
            var viewedAdminMessagesIds = GetAdminMesssagesFromDatabase(true);
            foreach (var adminMessage in adminMessagesBox.Items)
            {
                adminMessage.Viewed = viewedAdminMessagesIds.Contains(adminMessage.Id);
            }
            return adminMessagesBox;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static AdminMessageBox GetAdminMessagesWithoutMessages()
        {
            var adminMessagesBox = GetAdminMessagesWithoutMessagesFromRemoteServer();
            var viewedAdminMessagesIds = GetAdminMesssagesFromDatabase(true);
            foreach (var adminMessage in adminMessagesBox.Items)
            {
                adminMessage.Viewed = viewedAdminMessagesIds.Contains(adminMessage.Id);
            }
            return adminMessagesBox;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int GetNotViewedAdminMessagesCount()
        {
            var activAdminMessagesIds = GetAdminMessagesIdsFromRemoteServer();
            var viewedAdminMessagesIds = GetAdminMesssagesFromDatabase(true);

            return activAdminMessagesIds.Count(item => !viewedAdminMessagesIds.Contains(item));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewed"></param>
        /// <returns></returns>
        public static List<int> GetAdminMesssagesFromDatabase(bool viewed)
        {
            return SQLDataAccess.ExecuteReadColumn<int>("SELECT [ID] FROM [dbo].[AdminMessages] WHERE [Viewed] = @Viewed", CommandType.Text, "ID", new SqlParameter("@Viewed", viewed));
        }

        public static void SetViewedAdminMesssage(int amId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "if(SELECT COUNT([ID]) FROM [dbo].[AdminMessages] WHERE [ID]=@ID) < 1  BEGIN INSERT INTO [dbo].[AdminMessages] ([ID],[Viewed]) VALUES (@ID,1) END",
                CommandType.Text,
                new SqlParameter("@ID", amId));
        }

        public static void SetNotViewedAdminMesssage(int amId)
        {
            SQLDataAccess.ExecuteNonQuery(
                "DELETE FROM [dbo].[AdminMessages] WHERE [ID] = @ID",
                CommandType.Text,
                new SqlParameter("@ID", amId));
        }
    }
}