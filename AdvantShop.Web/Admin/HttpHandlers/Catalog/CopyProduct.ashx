<%@ WebHandler Language="C#" Class="AdvantShop.Admin.HttpHandlers.Catalog.CopyProduct" %>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AdvantShop.Admin.Handlers.Catalog;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using Newtonsoft.Json;

namespace AdvantShop.Admin.HttpHandlers.Catalog
{
    public class CopyProduct : AdminHandler, IHttpHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            if (!Authorize(context))
            {
                context.Response.Write(JsonConvert.SerializeObject(new {result = "error"}));
                return;
            }

            var sourceProduct = ProductService.GetProduct(context.Request["productId"].TryParseInt());
            if (sourceProduct == null)
            {
                context.Response.Write(JsonConvert.SerializeObject(new {result = "error"}));
                return;
            }

            var name = context.Request["name"];
            var artno = context.Request["artno"];
            var url = context.Request["url"];
            
            bool valid = true;
            valid &= name.IsNotEmpty();

            if (!valid)
            {
                context.Response.Write(JsonConvert.SerializeObject(new { result = "error", message = "Заполните обязательные поля" }));
                return;
            }

            var handler = new CopyProductHandler();
            var productId = handler.Exectute(sourceProduct, name, artno, url);
            if (productId == 0)
            {
                context.Response.Write(JsonConvert.SerializeObject(new { result = "error", message = "При добавлении товара возникла ошибка" }));
                return;
            }

            context.Response.Write(JsonConvert.SerializeObject(new {result = "success", productId = productId}));
        }
    }
}