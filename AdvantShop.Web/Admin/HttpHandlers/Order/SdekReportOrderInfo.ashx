<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Order.SdekReportOrderInfo" %>

using System.Web;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.Shipping;
using AdvantShop.Shipping.Sdek;
using Newtonsoft.Json;

namespace Admin.HttpHandlers.Order
{
    public class SdekReportOrderInfo : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            var orderId = context.Request["orderId"].TryParseInt();
           

            var order = OrderService.GetOrder(orderId);
            if (order == null)
            {
                ReturnResult(context, new { status = false, message = "error" });
                return;
            }

            var shippingMethod = ShippingMethodService.GetShippingMethod(order.ShippingMethodId);
            if (shippingMethod.ShippingType != "Sdek")
            {
                ReturnResult(context, new {status = false, message = "error"});
                return;
            }

            SdekStatusAnswer result = (new Sdek(shippingMethod, null)).ReportOrdersInfo(order);


            var fileName = System.Convert.ToString(result.Object);
            var fullFilePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp, fileName); //SettingsGeneral.AbsolutePath + "price_temp\\" + fileName;
            if (!System.IO.File.Exists(fullFilePath))
            {
                return;
            }

            CommonHelper.WriteResponseFile(fullFilePath, fileName);
        }

        private static void ReturnResult(HttpContext context, object result)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(JsonConvert.SerializeObject(new { result }));
            context.Response.End();
        }
    }
}