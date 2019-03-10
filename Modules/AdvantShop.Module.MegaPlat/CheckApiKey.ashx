<%@ WebHandler Language="C#" Class="Advantshop.UserControls.Modules.CheckApiKey" %>

using System.Web;
using AdvantShop.Core.Modules;
using AdvantShop.Diagnostics;


namespace Advantshop.UserControls.Modules
{
    public class CheckApiKey : IHttpHandler
    {
        private const string _moduleName = "MegaPlat";

        public void ProcessRequest(HttpContext context)
        {
            Debug.LogError(context.Request["apikey"] + " - " + ModuleSettingsProvider.GetSettingValue<string>("MegaPlatApiKey", _moduleName));
            context.Response.ContentType = "text/html";
            context.Response.Write(context.Request["apikey"] != ModuleSettingsProvider.GetSettingValue<string>("MegaPlatApiKey", _moduleName) ? "Неверный apikey" : "OK");
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}