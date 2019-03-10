//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using AdvantShop.Configuration;
using AdvantShop.Core;
using AdvantShop.Core.Caching;
using AdvantShop.Core.SQL;
using AdvantShop.Diagnostics;
using Newtonsoft.Json;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.Catalog;

namespace AdvantShop.Saas
{
    public class SaasDataService
    {
        private static readonly object SyncObject = new object();
        private const string SaasDataKey = "SaasDataKey";

        private const string RequestUrl = "http://modules.advantshop.net/Saas/GetParams/";

        public static bool IsSaasEnabled
        {
            get { return ModeConfigService.IsModeEnabled(ModeConfigService.Modes.SaasMode); }
        }

        public static bool IsTechDomain
        {
            get { return SettingsMain.SiteUrl.Contains("on-advantshop.net"); }
        }

        public static bool IsExist()
        {
            return SQLDataAccess.ExecuteScalar<int>("SELECT COUNT(*) FROM [dbo].[SaasData]", CommandType.Text) > 0;
        }

        public static Dictionary<string, string> GetDbSaasdata()
        {
            if (IsExist())
                return SQLDataAccess.ExecuteReadDictionary<string, string>("Select [Key],[Value] from dbo.SaasData", CommandType.Text, "Key", "Value");

            return null;
        }

        private static void AddUpdateSaasKey(string key, string value)
        {
            SQLDataAccess.ExecuteNonQuery(
                "If ((Select count(*) from dbo.SaasData where [key]=@key) > 0) " +
                "Update dbo.SaasData set value=@value where [key]=@key " +
                "Else " +
                "Insert Into dbo.SaasData ([key],value) Values (@key,@value) ",
                CommandType.Text,
                new SqlParameter("@key", key),
                new SqlParameter("@value", value));
        }


        public static void UpdateSaasData(SaasData serverData)
        {
            if (!serverData.IsCorrect)
            {
                SettingsLic.ActiveLic = false;
                return;
            }
            var saasData = AsDictionary(serverData);
            if (saasData.Keys.Count > 1)
                ClearSaasData();

            foreach (var key in saasData.Keys)
            {
                AddUpdateSaasKey(key, saasData[key]);
            }
        }

        public static void ClearSaasData()
        {
            SQLDataAccess.ExecuteNonQuery("Delete from dbo.SaasData", CommandType.Text);
            CacheManager.RemoveByPattern(SaasDataKey);
        }

        public static SaasData CurrentSaasData
        {
            get
            {
                if (!IsSaasEnabled) return new SaasData();
                //if muiltitenant add lickey to SaasDataKey
                SaasData saasData;
                if (CacheManager.TryGetValue(SaasDataKey, out saasData))
                {
                    return saasData;
                }
                saasData = GetSaasData();
                CacheManager.Insert(SaasDataKey, saasData);
                return saasData;
            }
        }

        public static SaasData GetSaasData(bool forceUpdate = false)
        {
            var saasDataDb = GetDbSaasdata();
            var saasData = ToObject(saasDataDb); //new SaasData(saasDataDb);
            var now = DateTime.Now;
            if ((saasData.LastUpdate > now) && saasData.IsWork && !forceUpdate)
            {                
                return saasData;
            }
            lock (SyncObject)
            {
                var saasDataNew = GetSaasDataFromService();
                if (saasDataNew.IsCorrect)
                {
                    CacheManager.RemoveByPattern(SaasDataKey);
                    saasDataNew.LastUpdate = saasDataNew.LastUpdate.AddMinutes(30);
                    UpdateSaasData(saasDataNew);

                    DeactivateNotInTariff(saasDataNew);
                    return saasDataNew;
                }
            }
            
            return saasData;
        }

        public static SaasData GetNextSaasDataFromService()
        {
            try
            {
                var key = SettingsLic.LicKey;
                var request = WebRequest.Create("http://modules.advantshop.net/Saas/GetNextPlanParams/" + key);
                request.Method = "GET";

                using (var dataStream = request.GetResponse().GetResponseStream())
                {
                    if (dataStream != null)
                        using (var reader = new StreamReader(dataStream))
                        {
                            var responseFromServer = reader.ReadToEnd();
                            if (!string.IsNullOrEmpty(responseFromServer))
                            {
                                var newSaasData = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseFromServer);
                                return ToObject(newSaasData);
                            }
                        }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
            return new SaasData { Error = "Unexpected error" };
        }

        public static bool IsEnabledFeature(ESaasProperty saasProperty)
        {
            if (!IsSaasEnabled)
                return true;

            var saasFeatures = AsDictionary(CurrentSaasData); // GetSaasData()

            var isEnabled = false;
            if (saasFeatures.ContainsKey(saasProperty.ToString()))
            {
                bool.TryParse(saasFeatures[saasProperty.ToString()], out isEnabled);
            }

            return isEnabled;
        }


        public static string GetMyAccountLink()
        {
            var customer = CustomerContext.CurrentCustomer;
            if (!customer.IsAdmin)
                return "";

            return SettingsLic.AccountPlatformUrl +
                      "/login?email=" + customer.EMail +
                      "&hash=" + SecurityHelper.EncodeWithHmac(customer.EMail ?? "", customer.Password ?? "") +
                      "&shopid=" + SettingsLic.LicKey;
        }

        private static SaasData GetSaasDataFromService()
        {
            try
            {
                var key = SettingsLic.LicKey;
                var request = WebRequest.Create(RequestUrl + key);
                request.Method = "GET";

                using (var dataStream = request.GetResponse().GetResponseStream())
                {
                    if (dataStream != null)
                        using (var reader = new StreamReader(dataStream))
                        {
                            var responseFromServer = reader.ReadToEnd();
                            if (!string.IsNullOrEmpty(responseFromServer))
                            {
                                var newSaasData = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseFromServer);
                                var serverData = ToObject(newSaasData);
                                return serverData;
                            }
                        }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
            return new SaasData { Error = "Unexpected error" };
        }

        private static SaasData ToObject(IDictionary<string, string> source)
        {
            var someObject = new SaasData();
            if (source == null)
            {
                someObject.Error = "Have not data";
                return someObject;
            }
            var someObjectType = someObject.GetType();
            var avalibleFeatures = Enum.GetValues(typeof(ESaasProperty)).Cast<ESaasProperty>().Select(x => x.ToString()).ToList();
            foreach (var key in avalibleFeatures)
            {
                if (!source.ContainsKey(key)) continue;
                var value = source[key];
                var property = someObjectType.GetProperty(key);
                if (property == null) continue;

                property.SetValue(someObject, Convert(value, property.PropertyType), null);

            }
            return someObject;
        }

        private static object Convert(string input, Type t)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(t);
                if (converter != null)
                    return converter.ConvertFromInvariantString(input);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }
            return t.IsValueType ? Activator.CreateInstance(t) : null;
        }

        private static Dictionary<string, string> AsDictionary(SaasData source, BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
        {
            return CacheManager.Get(SaasDataKey + "_AsDictionary_" + bindingAttr, () =>
            {
                var avalibleFeatures = Enum.GetValues(typeof(ESaasProperty)).Cast<ESaasProperty>().Select(x => x.ToString()).ToList();
                var properties = source.GetType().GetProperties(bindingAttr);
                var res = new Dictionary<string, string>();
                foreach (var propInfo in properties)
                {
                    if (!avalibleFeatures.Contains(propInfo.Name)) continue;
                    var value = propInfo.GetValue(source, BindingFlags.GetProperty, null, null, CultureInfo.InvariantCulture);

                    var valueStr = value == null ? "" : System.Convert.ToString(value, CultureInfo.InvariantCulture);
                    res.Add(propInfo.Name, valueStr);
                }
                return res;
            });
        }

        private static void DeactivateNotInTariff(SaasData saasData)
        {
            ProductService.DeactivateGoodsMoreThan(saasData.ProductsCount);
            EmployeeService.DeactivateEmployeeMoreThan(saasData.EmployeesCount);
        }
    }
}

