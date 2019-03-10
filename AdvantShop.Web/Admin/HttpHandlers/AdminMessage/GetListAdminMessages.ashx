<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.AdminMessage.GetListAdminMessages" %>

using System.Collections.Generic;
using System.Web;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Notifications;

namespace Admin.HttpHandlers.AdminMessage
{
    public class GetListAdminMessages : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            var adminMessage = AdminMessagesService.GetAdminMessagesWithoutMessages();
            if (adminMessage.Items != null && adminMessage.Items.Count > 0)
            {
                context.Response.ContentType = "application/json";
                var messages = new List<object>();

                foreach (var message in adminMessage.Items)
                {
                    messages.Add(new
                        {
                            Subject = message.Subject,
                            DateChange = message.DateChange.ToShortDateString(),
                            MessageTypeString = message.MessageTypeString,
                        });
                }

                context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(messages));
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
