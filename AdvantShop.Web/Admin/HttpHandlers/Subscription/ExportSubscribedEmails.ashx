<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Subscription.ExportSubscribedEmails" %>

using System;
using System.IO;
using System.Web;
using System.Text;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Customers;
using AdvantShop.FilePath;

namespace Admin.HttpHandlers.Subscription
{
    public class ExportSubscribedEmails : AdminHandler, IHttpHandler
    {

        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            var filePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp) + "subscribers" + DateTime.Now.ToShortDateString() + ".csv";
            using (var streamWriter = new StreamWriter(filePath, false, Encoding.GetEncoding("utf-8")))
            {

                streamWriter.WriteLine("{0};{1};{2};{3};{4};",
                    Resources.Resource.Admin_Subscription_Unreg_Email,
                    Resources.Resource.Admin_Subscription_Unreg_Status,
                    Resources.Resource.Admin_Subscription_Date,
                    Resources.Resource.Admin_Subscription_UnsubscribeDate,
                    Resources.Resource.Admin_Subscription_DeactivateReason_Header);

                foreach (var subscriber in SubscriptionService.GetSubscriptions())
                {
                    streamWriter.WriteLine("{0};{1};{2};{3};{4};",
                        subscriber.Email,
                        subscriber.Subscribe ? Resources.Resource.Admin_Yes : Resources.Resource.Admin_No,
                        subscriber.SubscribeDate != DateTime.MinValue ? subscriber.SubscribeDate.ToString() : string.Empty,
                        subscriber.UnsubscribeDate != DateTime.MinValue ? subscriber.UnsubscribeDate.ToString() : string.Empty,
                        subscriber.UnsubscribeReason);
                }

            }
            var fileInfo = new FileInfo(filePath);
            context.Response.Clear();
            context.Response.ContentType = "application/download";
            context.Response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", fileInfo.Name));
            context.Response.TransmitFile(filePath);
        }
    }
}