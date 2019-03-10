using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Web.Hosting;
using AdvantShop.Core.SQL;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using Newtonsoft.Json;

namespace AdvantShop.Core.Services.Localization
{
    public static class LocalizationService
    {
        private static LocalizedSetPair _localizedSet;  // last used resources

        public static string GetResource(string resourceKey)
        {
            var cultureName = Thread.CurrentThread.CurrentUICulture.Name;

            return GetResource(resourceKey, cultureName);
        }

        public static string GetResource(string resourceKey, string cultureName)
        {
            //resourceKey = resourceKey.ToLowerInvariant();

            if (_localizedSet == null || _localizedSet.Culture != cultureName)
                _localizedSet = new LocalizedSetPair()
                {
                    Culture = cultureName,
                    Resources = GetResources(cultureName)
                };

            string value;

            if (_localizedSet.Resources.TryGetValue(resourceKey, out value))
                return value;
            
            return resourceKey; // or empty string?
        }

        public static string GetResourceFormat(string resourceKey, object param1)
        {
            return string.Format(GetResource(resourceKey), param1);
        }

        public static string GetResourceFormat(string resourceKey, params object[] parametres)
        {
            return string.Format(GetResource(resourceKey), parametres);
        }

        public static Dictionary<string, string> GetResources(string cultureName)
        {
            var dict = new Dictionary<string, string>();

            var resources =
                SQLDataAccess.Query<LocalizedResource>(
                    "Select Localization.LanguageId,ResourceKey,ResourceValue From Settings.Localization " +
                    "Left Join [Settings].[Language] On [Language].[LanguageID] = [Localization].[LanguageId] " +
                    "Where LanguageCode=@cultureName", new { cultureName });

            foreach (var resource in resources)
            {
                var key = resource.ResourceKey; //.ToLowerInvariant();

                if (!dict.ContainsKey(key))
                    dict.Add(key, resource.ResourceValue);
            }

            return dict;
        }

        public static void AddOrUpdateResource(int languageId, string resourceKey, string resourceValue)
        {
            SQLDataAccess.ExecuteNonQuery(
                "IF ((Select Count(*) From [Settings].[Localization] Where LanguageId=@LanguageId and ResourceKey=@ResourceKey) > 0) " +
                "Begin " +
                "   Update [Settings].[Localization] Set ResourceKey=@ResourceKey, ResourceValue=@ResourceValue Where LanguageId=@LanguageId and ResourceKey=@ResourceKey " +
                "End " +
                "Else " +
                "Begin " +
                "   Insert Into [Settings].[Localization] ([LanguageId],[ResourceKey],[ResourceValue]) Values (@LanguageId,@ResourceKey,@ResourceValue) " +
                "End ",
                CommandType.Text,
                new SqlParameter("@LanguageId", languageId),
                new SqlParameter("@ResourceKey", resourceKey),
                new SqlParameter("@ResourceValue", resourceValue));
            
            _localizedSet = null;
        }

        public static void RemoveByPattern(string key)
        {
            SQLDataAccess.ExecuteNonQuery(
                "Delete From [Settings].[Localization] Where ResourceKey Like '" + key.ToLower() + "%'", CommandType.Text);
            
            _localizedSet = null;
        }

        public static void GenerateJsResourcesFile()
        {
            var cultureName = Thread.CurrentThread.CurrentUICulture.Name.ToLower();
            var localizationDirPath =  HostingEnvironment.MapPath("~/userfiles/");
            var localizationFilePath = localizationDirPath + "\\" + cultureName + ".js";

            FileHelpers.CreateDirectory(localizationDirPath);

            Dictionary <string, string> jsResources = null;

            try
            {
                jsResources = SQLDataAccess.ExecuteReadDictionary<string, string>(
                    "Select Localization.LanguageId,ResourceKey,ResourceValue From Settings.Localization " +
                    "Left Join [Settings].[Language] On [Language].[LanguageID] = [Localization].[LanguageId] " +
                    "Where LanguageCode=@CultureName and ResourceKey like 'Js.%' ",
                    CommandType.Text,
                    "ResourceKey", "ResourceValue",
                    new SqlParameter("@CultureName", cultureName));
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }


            var jsResourceObject = string.Format("window.AdvantshopResource = {0};", JsonConvert.SerializeObject(jsResources));

            using (var sw = new StreamWriter(localizationFilePath))
            {
                sw.Write(jsResourceObject);
            }
        }
    }
}
