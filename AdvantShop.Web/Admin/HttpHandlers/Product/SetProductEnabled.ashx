<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Product.SetProductEnabled" %>

using System;
using System.Web;
using AdvantShop.Catalog;
using AdvantShop.Core.HttpHandlers;

namespace Admin.HttpHandlers.Product
{
    public class SetProductEnabled : AdminHandler, IHttpHandler
    {
        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;
            
            var enabled = 0;
            var productId = 0;

            if (!Int32.TryParse(context.Request["enabled"], out enabled) || !Int32.TryParse(context.Request["productId"], out productId))
            {
                ReturnResult(context, "error");
            }

            ProductService.EnableDynamicProductLinkRecalc();
            ProductService.SetActive(productId, enabled == 1);
            ProductService.DisableDynamicProductLinkRecalc();
        }

        private static void ReturnResult(HttpContext context, string result)
        {
            context.Response.ContentType = "application/text";
            context.Response.Write(result);
            context.Response.End();
        }
    }
}