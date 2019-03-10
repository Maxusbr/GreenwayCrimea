using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Customers;
using AdvantShop.Module.RetailRocket.Domain;
using AdvantShop.Orders;
using AdvantShop.Web.Infrastructure.Controllers;

namespace AdvantShop.Module.RetailRocket.Controllers
{
    public class RetailRocketController : ModuleController
    {
        #region Consts

        private const string FormatScript =
@"<script>
        var rrPartnerId = ""{0}"";
        var rrApi = {{}}; 
        var rrApiOnReady = rrApiOnReady || [];
        rrApi.addToBasket = rrApi.order = rrApi.categoryView = rrApi.view = 
            rrApi.recomMouseDown = rrApi.recomAddToCart = function() {{}};
        (function(d) {{
            var ref = d.getElementsByTagName('script')[0];
            var apiJs, apiJsId = 'rrApi-jssdk';
            if (d.getElementById(apiJsId)) return;
            apiJs = d.createElement('script');
            apiJs.id = apiJsId;
            apiJs.async = true;
            apiJs.src = ""//cdn.retailrocket.ru/content/javascript/{1}.js"";
            ref.parentNode.insertBefore(apiJs, ref);
        }}(document));
</script>
<script type=""text/javascript"" src=""modules/retailrocket/js/lib.js"" async></script>";

        private const string FormatOrderEvent =
@"<script type='text/javascript'>
try {{
{0}
}} catch(e) {{console.log(e)}}
rrApiOnReady.push(function() {{
    try {{
        rrApi.order({{
            transaction: {1},
            items: [{2}]
        }});
    }} catch(e) {{console.log(e)}}
}})
</script>";

        private const string FormatProductByIds =
            "<div class=\"rr-products\" data-ids=\"{0}\" data-title=\"{1}\" data-type=\"{2}\" data-visibleitems=\"{3}\"></div>";

        #endregion

        #region Initialize script, view category, product, order pages

        public ActionResult RrScript()
        {
            return Content(string.Format(FormatScript, RRSettings.PartnerId, RRSettings.UseApi ? "tracking" : "api"));
        }

        public ActionResult ProductRight(Product product, Offer offer)
        {
            if (product == null || offer == null)
                return new EmptyResult();

            return Content(
                string.Format(
                    "<script type=\"text/javascript\">rrApiOnReady.push(function() {{ try{{ rrApi.view({0}); }} catch(e) {{}} }})</script>",
                    offer.OfferId));
        }


        public ActionResult OrderFinalStep(IOrder order)
        {
            var strItems = "";
            foreach (var item in order.OrderItems.Where(x => x.ProductID != null))
            {
                var offers = OfferService.GetProductOffers(Convert.ToInt32(item.ProductID));
                if (offers != null)
                {
                    var offer = offers.Find(x => x.ArtNo == item.ArtNo);
                    if (offer != null)
                    {
                        strItems += (string.IsNullOrEmpty(strItems) ? "" : ", ") +
                                    string.Format("{{ id: {0}, qnt: {1},  price: {2} }}", offer.OfferId, item.Amount,
                                        item.Price.ToString("F2").Replace(",", "."));
                    }
                }
            }

            var sendEmail = "";
            var customer = order.GetOrderCustomer();

            if (RRSettings.SendMail && customer != null && !string.IsNullOrWhiteSpace(customer.Email))
            {
                sendEmail = "rrApiOnReady.push(function () { rrApi.setEmail('" + customer.Email + "'); });";
            }

            return Content(string.Format(FormatOrderEvent, sendEmail, order.OrderID, strItems));
        }

        #endregion

        private static string GetMainPageBlock(EMainPageProductsType type, string title, int productsCount)
        {
            var productIds = new List<int>();
            var recomType = "";

            switch (type)
            {
                case EMainPageProductsType.Popular:
                    productIds = RetailRocketService.GetMainPageProducts();
                    recomType = "ItemsToMain";
                    break;

                case EMainPageProductsType.Personal:
                    var cookie = System.Web.HttpContext.Current.Request.Cookies["rrpusid"];
                    if (cookie != null && cookie.Value != null)
                    {
                        productIds = RetailRocketService.GetCartRecomendations(cookie.Value);
                        recomType = "PersonalRecommendation";
                    }
                    break;
            }

            if (productIds == null || productIds.Count == 0)
                return null;

            return string.Format(FormatProductByIds, String.Join(",", productIds), title, recomType, productsCount);
        }

        private static string GetCategoryBlock(Category category, ECategoryProductsType type, string title, int productsCount)
        {
            if (category == null)
                return null;

            var productIds = new List<int>();
            var recomType = "";

            switch (type)
            {
                case ECategoryProductsType.Category:
                    productIds = RetailRocketService.GetRecomendationsByCategory(category.CategoryId);
                    recomType = "ItemsToMain";
                    break;

                case ECategoryProductsType.Personal:
                    var cookie = System.Web.HttpContext.Current.Request.Cookies["rrpusid"];
                    if (cookie != null && cookie.Value != null)
                    {
                        productIds = RetailRocketService.GetCartRecomendations(cookie.Value);
                        recomType = "PersonalRecommendation";
                    }
                    break;
            }

            if (productIds == null || productIds.Count == 0)
                return null;

            return string.Format(FormatProductByIds, String.Join(",", productIds), title, recomType, productsCount);
        }


        public ActionResult MainPageProductsBefore()
        {
            if (!RRSettings.UseApi)
                return new EmptyResult();

            var script = GetMainPageBlock(RRSettings.MainPageBeforeType, RRSettings.MainPageBeforeTitle, SettingsDesign.CountMainPageProductInLine);

            return Content(script);
        }

        public ActionResult MainPageProductsAfter()
        {
            if (!RRSettings.UseApi)
                return new EmptyResult();

            var script = GetMainPageBlock(RRSettings.MainPageAfterType, RRSettings.MainPageAfterTitle, SettingsDesign.CountMainPageProductInLine);
            
            return Content(script);
        }

        public ActionResult CategoryTop(Category category)
        {
            if (!RRSettings.UseApi || category == null)
                return new EmptyResult();

            var script =
                string.Format(
                    "<script type=\"text/javascript\">rrApiOnReady.push(function() {{try{{ rrApi.categoryView({0}); }} catch(e) {{}}}})</script>\n",
                    category.CategoryId);

            var scriptProducts = GetCategoryBlock(category, RRSettings.CategoryTopType, RRSettings.CategoryTopTitle, 0);
            
            return Content(script + scriptProducts);
        }

        public ActionResult CategoryBottom(Category category)
        {
            if (!RRSettings.UseApi) 
                return new EmptyResult();

            var script = GetCategoryBlock(category, RRSettings.CategoryBottomType, RRSettings.CategoryBottomTitle, 0);

            return Content(script);
        }

        public ActionResult SearchTop(string q)
        {
            return Content(
                "<script type=\"text/javascript\"> " +
                "rrApiOnReady.push(function() {{ try {{ rrApi.search(\"" + HttpUtility.HtmlDecode(q) + "\"); }} catch(e) {{}} }}); " +
                "</script> \n");
        }

        public ActionResult CartBottomScript()
        {
            if (CustomerContext.CurrentCustomer == null)
                return new EmptyResult();

            var cart = ShoppingCartService.CurrentShoppingCart;
            if (cart == null || !cart.HasItems)
                return new EmptyResult();

            var offerIds = cart.Select(x => x.OfferId).ToList();

            if (!RRSettings.UseApi && !string.IsNullOrEmpty(RRSettings.ShoppingCartRecoms))
            {
                return Content(RRSettings.ShoppingCartRecoms.ToLower().Replace("<products_id>", String.Join(",", offerIds)));
            }

            var productIds = RetailRocketService.GetShoppingCartRecomendations(offerIds);
            if (productIds == null || productIds.Count == 0)
                return new EmptyResult();

            return Content(string.Format(FormatProductByIds, String.Join(",", productIds), "", "CrossSellItemToItems", 0));
        }
    }
}
