<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.AdminMessage.GetAdminMessage" %>

using System;
using System.Web;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Notifications;

namespace Admin.HttpHandlers.AdminMessage
{
    public class GetAdminMessage : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            var amId = 0;
            if (!Int32.TryParse(context.Request["amId"], out amId))
            {
                ReturnResult(context, "error");
            }

            var adminMessage = AdminMessagesService.GetAdminMessage(amId);
            if (adminMessage.Items != null && adminMessage.Items.Count > 0)
            {
                context.Response.ContentType = "application/json";
                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        adminMessage.Items[0].Id,
                        adminMessage.Items[0].DateAdded,
                        DateChange = adminMessage.Items[0].DateChange.ToShortDateString(),
                        adminMessage.Items[0].Enabled,
                        adminMessage.Items[0].Message,
                        adminMessage.Items[0].MessageType,
                        adminMessage.Items[0].MessageTypeString,
                        adminMessage.Items[0].Subject,
                        adminMessage.Items[0].Viewed
                    }));
                context.Response.End();
            }
            else
            {
                ReturnResult(context, "error");
            }
        }

        private static void ReturnResult(HttpContext context, string result)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new { result }));
            context.Response.End();
        }
    }
}
