<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Settings.ResetMeta" %>

using System;
using System.Web;

using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.SEO;


namespace Admin.HttpHandlers.Settings
{
    public class ResetMeta : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            if (context.Request["metatype"].IsNullOrEmpty())
            {
                ReturnResult(context, Resources.Resource.Admin_TemplateSettings_ErrorSaveSettings);
                return;
            }

            MetaType metaType;

            if (!Enum.TryParse(context.Request["metatype"], true, out metaType))
            {
                ReturnResult(context, "ошибка, неверный тип");
                return;
            }

            MetaInfoService.DeleteMetaInfoByType(metaType);
            
            ReturnResult(context, "мета информация сброшена");
        }

        private static void ReturnResult(HttpContext context, string result)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { result }));
            context.Response.End();
        }

    }
}