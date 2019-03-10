<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.AdminMessage.SetViewedAdminMessage" %>

using System;
using System.Web;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Notifications;

namespace Admin.HttpHandlers.AdminMessage
{
    public class SetViewedAdminMessage : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            if (string.IsNullOrEmpty(context.Request["ids"]))
            {
                ReturnResult(context, "error");
            }

            foreach (var id in context.Request["ids"].Split(','))
            {
                AdminMessagesService.SetViewedAdminMesssage(Convert.ToInt32(id));
            }

            ReturnResult(context, "success");
        }

        private static void ReturnResult(HttpContext context, string result)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { result }));
            context.Response.End();
        }

    }
}