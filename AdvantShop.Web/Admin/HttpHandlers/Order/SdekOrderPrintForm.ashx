<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Order.SdekOrderPrintForm" %>

using System.Web;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.Shipping;
using AdvantShop.Shipping.Sdek;

namespace Admin.HttpHandlers.Order
{
    public class SdekOrderPrintForm : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;

            var orderId = context.Request["orderId"].TryParseInt();

            var order = OrderService.GetOrder(orderId);
            if (order == null)
            {
                return;
            }

            var shippingMethod = ShippingMethodService.GetShippingMethod(order.ShippingMethodId);
            if (shippingMethod.ShippingType != "Sdek")
            {
                return;
            }
            
            var fileName = (new Sdek(shippingMethod, null)).PrintFormOrder(order);
            var fullFilePath = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp, fileName); //SettingsGeneral.AbsolutePath + "price_temp\\" + fileName;
            if (!System.IO.File.Exists(fullFilePath))
            {
                return;
            }

            CommonHelper.WriteResponseFile(fullFilePath, fileName);
        }
    }
}