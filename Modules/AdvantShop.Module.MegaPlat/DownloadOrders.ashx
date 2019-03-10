<%@ WebHandler Language="C#" Class="Advantshop.UserControls.Modules.DownloadOrders" %>

using System.Collections.Generic;
using System.Globalization;
using System;
using System.IO;
using System.Web;
using AdvantShop.Orders;
using AdvantShop.Core.Modules;


namespace Advantshop.UserControls.Modules
{
    public class DownloadOrders : IHttpHandler
    {
        private const string _moduleName = "MegaPlat";

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/xml";

            if (context.Request["apikey"] !=
                ModuleSettingsProvider.GetSettingValue<string>("MegaPlatApiKey", _moduleName))
            {
                context.Response.Write("Неверный apikey");
                return;
            }


            string filename = context.Server.MapPath("~/modules/megaplat/orders.xml");
            if (File.Exists(filename))
                File.Delete(filename);

            List<Order> orders = null;

            if (!string.IsNullOrEmpty(context.Request["from"]) && !string.IsNullOrEmpty(context.Request["to"]))
            {
                DateTime fromDate;
                DateTime toDate;

                if (DateTime.TryParseExact(context.Request["from"], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fromDate) &&
                    DateTime.TryParseExact(context.Request["to"], "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out toDate))
                {
                    orders = OrderService.GetOrdersFor1C(fromDate, toDate.AddDays(1), false);
                }
            }
            else
            {
                orders = OrderService.GetAllOrders();
            }

            if (orders == null)
            {
                orders = new List<Order>();
            }

            using (TextWriter writer = new StreamWriter(filename))
            {
                OrderService.SerializeToXml(orders, writer, true);
            }

            FileInfo file = new FileInfo(filename);

            context.Response.Clear();
            context.Response.AddHeader("Content-Disposition", "attachment; filename=" + "orders.xml");
            context.Response.AddHeader("Content-Length", file.Length.ToString(CultureInfo.InvariantCulture));
            context.Response.ContentType = "application/octet-stream";
            context.Response.WriteFile(file.FullName);
            context.Response.End();

            if (File.Exists(filename))
                File.Delete(filename);

        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}
