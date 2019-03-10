<%@ WebHandler Language="C#" Class="AdvantShop.Admin.HttpHandlers.ChangeLanguage" %>

using System;
using System.Globalization;
using System.Web;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Diagnostics;
using AdvantShop.Localization;
using Newtonsoft.Json;

namespace AdvantShop.Admin.HttpHandlers
{
    public class ChangeLanguage : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            
            var langId = context.Request["langid"].TryParseInt();
            
            var lang = LanguageService.GetLanguage(langId);
            if (lang != null)
            {
                try
                {
                    var culture = new CultureInfo(lang.LanguageCode);
                }
                catch (CultureNotFoundException ex)
                {
                    Debug.Log.Error(ex);

                    context.Response.Write(JsonConvert.SerializeObject(new { result = false, msg = "Culture '" + lang.LanguageCode + "' not correct" }));
                    return;
                }
                
                SettingsMain.Language = lang.LanguageCode;
                Culture.InitializeCulture();
                
                LocalizationService.GenerateJsResourcesFile();

                context.Response.Write(JsonConvert.SerializeObject(new { result = true }));
                return;
            }

            context.Response.Write(JsonConvert.SerializeObject(new { result = false }));
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}