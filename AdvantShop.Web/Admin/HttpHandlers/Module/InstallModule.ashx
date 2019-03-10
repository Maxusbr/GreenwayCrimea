<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Modules.InstallModule" %>

using System;
using System.Linq;
using System.Text;
using System.Web;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Helpers;


namespace Admin.HttpHandlers.Modules
{
    public class InstallModule : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            if (string.IsNullOrEmpty(context.Request["moduleIdOnRemoteServer"])
                || string.IsNullOrEmpty(context.Request["moduleVersion"])
                || string.IsNullOrEmpty(context.Request["moduleStringId"]))
            {
                ReturnResult(context, "error");
            }

            var moduleStringId = SQLDataHelper.GetString(context.Request["moduleStringId"]);
            var moduleVersion = SQLDataHelper.GetString(context.Request["moduleVersion"]);
            
            
            var moduleInst = AttachedModules.GetModules().FirstOrDefault(
                    item =>
                    ((IModule)Activator.CreateInstance(item, null)).ModuleStringId.ToLower() == moduleStringId.ToLower());

            if (moduleInst != null)
            {
                ModulesService.InstallModule(moduleStringId, moduleVersion);
                ReturnResult(context,
                        UrlService.GetAdminAbsoluteLink("Module.aspx?module=" + moduleStringId));
            }
            else
            {
                ModulesRepository.SetModuleNeedUpdate(moduleStringId, true);
                
                var message = ModulesService.GetModuleArchiveFromRemoteServer(context.Request["moduleIdOnRemoteServer"]);
                if (message.IsNullOrEmpty())
                {
                    HttpRuntime.UnloadAppDomain();

                    context.ApplicationInstance.CompleteRequest();
                    ReturnResult(context,
                        UrlService.GetAdminAbsoluteLink("modulesmanager.aspx?installModule=" + moduleStringId +
                                                        "&version=" + moduleVersion));
                }
            }
            
            ReturnResult(context, "error");
        }

        private static void ReturnResult(HttpContext context, string result)
        {
            context.Response.ContentType = "application/JSON";
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { result }));
            context.Response.End();
        }
    }
}