<%@ WebHandler Language="C#" Class="AdvantShop.Module.RelatedProductsInShoppingCart.GetRelatedProducts" %>

using System.Linq;
using System.Web;
using AdvantShop.Catalog;
using AdvantShop.Core.Modules;
using AdvantShop.Localization;
using AdvantShop.Orders;
using Newtonsoft.Json;

namespace AdvantShop.Module.RelatedProductsInShoppingCart
{
    public class GetRelatedProducts : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            Culture.InitializeCulture();
            context.Response.ContentType = "application/json";

            var relatedProductsInShoppingCartJson =
                RelatedProductsInShoppingCart.RelatedProductsInShoppingCartJson(
                    ShoppingCartService.CurrentShoppingCart.Select(p => p.Offer.ProductId).ToList(),
                    (RelatedType) ModuleSettingsProvider.GetSettingValue<int>("RelatedType", RelatedProductsInShoppingCart.ModuleID));

            if (!string.IsNullOrEmpty(relatedProductsInShoppingCartJson))
            {
                context.Response.Write(relatedProductsInShoppingCartJson);
                return;
            }

            ResponseStatus(context, "error");
        }

        public bool IsReusable
        {
            get { return false; }
        }

        private void ResponseStatus(HttpContext context, string status)
        {
            context.Response.Write(JsonConvert.SerializeObject(new {status}));
        }
    }
}