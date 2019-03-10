<%@ WebHandler Language="C#" Class="AdvantShop.Admin.HttpHandlers.Catalog.AddProduct" %>

using System.Collections.Generic;
using System.Web;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.HttpHandlers;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.Trial;
using Newtonsoft.Json;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Admin.HttpHandlers.Catalog
{
    public class AddProduct : AdminHandler, IHttpHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            if (!Authorize(context))
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

            var product = new AdvantShop.Catalog.Product
            {
                Name = name,
                ArtNo = ProductService.GetProduct(artno) == null ? artno : null,
                UrlPath = UrlService.GetAvailableValidUrl(0, ParamType.Product, url.IsNullOrEmpty() ? name : url),
                Enabled = true,
                CurrencyID = CurrencyService.CurrentCurrency.CurrencyId,
                Multiplicity = 1,
                Offers = new List<Offer>
                {
                    new Offer()
                    {
                        ArtNo = OfferService.GetOffer(artno) == null ? artno : null,
                        Amount = 1,
                        BasePrice = 0,
                        Main = true,
                    }
                },
                ModifiedBy = CustomerContext.CustomerId.ToString()
            };

            var categoryId = context.Request["categoryid"].TryParseInt();
            var productId = ProductService.AddProduct(product, true);

            if (productId != 0 && categoryId != 0 && categoryId != CategoryService.DefaultNonCategoryId)
            {
                ProductService.EnableDynamicProductLinkRecalc();
                ProductService.AddProductLink(productId, categoryId, 0, true);
                ProductService.DisableDynamicProductLinkRecalc();
                ProductService.SetProductHierarchicallyEnabled(product.ProductId);
            }

            TrialService.TrackEvent(TrialEvents.AddProduct, "");
            
            context.Response.Write(productId != 0
                ? JsonConvert.SerializeObject(new {result = "success", productId})
                : JsonConvert.SerializeObject(new {result = "error", message = "При добавлении товара возникла ошибка"}));
        }
    }
}