<%@ WebHandler Language="C#" Class="Admin.HttpHandlers.Order.ProductsSearch" %>

using System.Web;
using System.Linq;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Core.Services.FullSearch;
using AdvantShop.Catalog;

namespace Admin.HttpHandlers.Order
{
    public class ProductsSearch : AdminHandler, IHttpHandler
    {

        public new void ProcessRequest(HttpContext context)
        {
            if (!Authorize(context))
                return;


            context.Response.ContentType = "text/plain";

            if (string.IsNullOrWhiteSpace(context.Request["q"]))
            {
                context.Response.Write("\n");
                context.Response.End();
                return;
            }
            
            var productIds = ProductSeacher.Search(context.Request["q"]).SearchResultItems.Aggregate("", (current, item) => current + (item.Id + "/"));

            var products = ProductService.GetForAutoCompleteProductsByIds(productIds);

            if (products.Count == 0)
            {
                context.Response.Write("\n");
                context.Response.End();
                return;
            }

            foreach (var product in products.Take(10))
            {
                foreach (var offer in product.Offers)
                {
                    context.Response.Write(string.Format("{0} <span data-offerid=\"{4}\">{1} {2} {3}</span>\n", product.Name,
                                                    (offer.Color != null ? offer.Color.ColorName : ""),
                                                    (offer.Size != null ? offer.Size.SizeName : ""),
                                                    offer.ArtNo, offer.OfferId));
                }
            }

            context.Response.End();
        }
    }
}