//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Core.SQL;
using AdvantShop.Customers;

namespace AdvantShop.Configuration
{
    /// <summary>
    /// Setting provider
    /// </summary>
    /// <remarks></remarks>
    public class SettingProvider
    {
        private const string ConfigSettingValueCacheKey = "ConfigSettingValue_";
        private static Dictionary<string, string> _settings; 
        private static object _lock = new object();

        public sealed class SettingIndexer
        {
            public string this[string name]
            {
                get { return GetSqlSettingValue(name); }
                set { SetSqlSettingValue(name, value); }
            }
        }

        private static SettingIndexer _staticIndexer;
        public static SettingIndexer Items
        {
            get { return _staticIndexer ?? (_staticIndexer = new SettingIndexer()); }
        }

        #region  SQL storage

        /// <summary>
        /// Save settings into DB
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <remarks></remarks>
        public static void SetSqlSettingValue(string name, string value)
        {
            lock (_lock)
            {
                if (value == null)
                {
                    SQLDataAccess.ExecuteNonQuery("Delete from settings.settings where Name=@Name", CommandType.Text,
                        new SqlParameter("@Name", name.Trim()));
                }
                else
                {
                    SQLDataAccess.ExecuteNonQuery("[Settings].[sp_UpdateSettings]", CommandType.StoredProcedure,
                        new SqlParameter("@Name", name.Trim()), new SqlParameter("@Value", value));
                }
                _settings = null;
            }
        }

        /// <summary>
        /// Get setting value by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSqlSettingValue(string key)
        {
            var settings = _settings ?? (_settings = GetAllSettings());

            string value;

            if (settings.TryGetValue(key, out value))
                return value;

            return null;
        }

        private static Dictionary<string, string> GetAllSettings()
        {
            var settings =
                SQLDataAccess.ExecuteReadDictionary<string, string>("SELECT [Name],[Value] FROM [Settings].[Settings]",
                    CommandType.Text, "Name", "Value");

            if (settings == null)
                return new Dictionary<string, string>();

            return settings;
        }

        #endregion

        #region  Web.config storage

        /// <summary>
        /// Read settings from appSettings node web.config file.
        /// On Err: Function will return an empty string
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetConfigSettingValue(string strKey)
        {
            var cacheKey = ConfigSettingValueCacheKey + strKey;

            string value;

            if (!CacheManager.TryGetValue(cacheKey, out value))
            {
                var config = new AppSettingsReader();
                value = config.GetValue(strKey, typeof (String)).ToString();

                if (value != null)
                    CacheManager.Insert(cacheKey, value);
            }

            return value;
        }
        
        public static T GetConfigSettingValue<T>(string strKey)
        {
            var cacheKey = ConfigSettingValueCacheKey + strKey;

            T value;

            if (!CacheManager.TryGetValue(cacheKey, out value))
            {
                var config = new AppSettingsReader();
                value = (T) config.GetValue(strKey, typeof (T));

                if (value != null)
                    CacheManager.Insert(cacheKey, value);
            }

            return value;
        }

        /// <summary>
        /// Save settings from appSettings node web.config
        /// </summary>
        /// <param name="strKey"></param>
        /// <param name="strValue"></param>
        /// <remarks></remarks>
        public static bool SetConfigSettingValue(string strKey, string strValue)
        {
            System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            var myAppSettings = (AppSettingsSection)config.GetSection("appSettings");
            myAppSettings.Settings[strKey].Value = strValue;
            config.Save();

            return true;
        }

        #endregion
        
    }
}