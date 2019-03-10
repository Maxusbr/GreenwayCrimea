using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Orders;

namespace AdvantShop.Module.AdwordsRemarketing.Controllers
{
    public class AdwordsRemarketingController : Controller
    {
        private enum pageTypes
        {
            home, searchresults, category, product, cart, purchase, other
        }

        private const string BasicCounter = "<div style=\"display:none\">" + "\r\n" +
                                "<script type=\"text/javascript\">" + "\r\n" +
                                "var google_tag_params = {" + "\r\n" +
                                "ecomm_prodid: #PRODUCT_ID#," + "\r\n" +
                                "ecomm_pagetype: '#PAGE_TYPE#'," + "\r\n" +
                                "ecomm_totalvalue: #TOTAL_VALUE#" + "\r\n" +
                                "};" + "\r\n" +
                                "</script>" + "\r\n" +
                                "<script type =\"text/javascript\">" + "\r\n" +
                                "/* <![CDATA[ */" + "\r\n" +
                                "var google_conversion_id = #CONVERSION_ID#;" + "\r\n" +
                                "        var google_custom_params = window.google_tag_params;" + "\r\n" +
                                "        var google_remarketing_only = true;" + "\r\n" +
                                "/* ]]> */" + "\r\n" +
                                "</script>" + "\r\n" +
                                "<script type =\"text/javascript\" src=\"//www.googleadservices.com/pagead/conversion.js\">" + "\r\n" +
                                "</script>" + "\r\n" +
                                "<noscript>" + "\r\n" +
                                "<div style =\"display:inline;\">" + "\r\n" +
                                "<img height =\"1\" width=\"1\" style=\"border-style:none;\" alt=\"\" src=\"//googleads.g.doubleclick.net/pagead/viewthroughconversion/#CONVERSION_ID#/?value=0&amp;guid=ON&amp;script=0\"/>" + "\r\n" +
                                "</div>" + "\r\n" +
                                "</noscript>" + "\r\n" +
                                "</div>";


        public ActionResult AfterBodyStart(string url, int? color, int? size)
        {
            var conversionId = ModuleSettingsProvider.GetSettingValue<string>("СonversionId", Modules.AdwordsRemarketing.ModuleID);

            if (string.IsNullOrEmpty(conversionId))
                return Content(string.Empty);

            string counter = BasicCounter;

            counter = counter.Replace("#CONVERSION_ID#", conversionId);

            var useDynx = ModuleSettingsProvider.GetSettingValue<bool>("UseDynx", Modules.AdwordsRemarketing.ModuleID);
            if (useDynx)
            {
                counter = counter.Replace("ecomm_prodid", "dynx_itemid");
                counter = counter.Replace("ecomm_pagetype", "dynx_pagetype");
                counter = counter.Replace("ecomm_totalvalue", "dynx_totalvalue");
            }


            string controller = ControllerContext.ParentActionViewContext.RouteData.Values["controller"].ToString().ToLower();
            string action = ControllerContext.ParentActionViewContext.RouteData.Values["action"].ToString().ToLower();

            if (controller == "home")
            {
                counter = counter.Replace("#PAGE_TYPE#", pageTypes.home.ToString());
                counter = counter.Replace("#PRODUCT_ID#", "''");
                counter = counter.Replace("#TOTAL_VALUE#", "0");
            }
            //else if (controller == "catalog")
            //{
            //    counter = counter.Replace("#PAGE_TYPE#", pageTypes.category.ToString());
            //    counter = counter.Replace("#PRODUCT_ID#", string.Empty);
            //    counter = counter.Replace("#TOTAL_VALUE#", string.Empty);
            //}
            else if (controller == "product")
            {
                counter = counter.Replace("#PAGE_TYPE#", pageTypes.product.ToString());

                Offer mainOffer;
                var product = ProductService.GetProductByUrl(url);
                if (product != null &&
                    (mainOffer = OfferService.GetMainOffer(product.Offers, product.AllowPreOrder, color, size)) != null)
                {
                    counter = counter.Replace("#PRODUCT_ID#", "'" + mainOffer.OfferId.ToString() + "'");
                    counter = counter.Replace("#TOTAL_VALUE#", mainOffer.RoundedPrice.ToInvariantString());
                }
                else
                {
                    counter = counter.Replace("#PRODUCT_ID#", "''");
                    counter = counter.Replace("#TOTAL_VALUE#", "0");
                }
            }
            else if (controller == "cart")
            {
                counter = counter.Replace("#PAGE_TYPE#", pageTypes.cart.ToString());
                counter = counter.Replace("#PRODUCT_ID#", "[" + ShoppingCartService.CurrentShoppingCart.Select(item=> "'" + item.OfferId+ "'").AggregateString(", ") + "]");
                counter = counter.Replace("#TOTAL_VALUE#", ShoppingCartService.CurrentShoppingCart.TotalPrice.ToInvariantString());
            }
            else if (controller == "checkout" && action == "success")
            {
                counter = string.Empty;
            }
            else if (controller == "checkout" && action != "success")
            {
                counter = counter.Replace("#PAGE_TYPE#", pageTypes.cart.ToString());
                counter = counter.Replace("#PRODUCT_ID#", "[" + ShoppingCartService.CurrentShoppingCart.Select(item => "'" + item.OfferId + "'").AggregateString(", ") + "]");
                counter = counter.Replace("#TOTAL_VALUE#", ShoppingCartService.CurrentShoppingCart.TotalPrice.ToInvariantString());
            }
            else
            {
                counter = counter.Replace("#PAGE_TYPE#", pageTypes.other.ToString());
                counter = counter.Replace("#PRODUCT_ID#", "''");
                counter = counter.Replace("#TOTAL_VALUE#", "0");
            }

            return Content(counter);
        }

        public ActionResult CheckoutFinalStep(IOrder order)
        {

            var conversionId = ModuleSettingsProvider.GetSettingValue<string>("СonversionId", Modules.AdwordsRemarketing.ModuleID);

            if (string.IsNullOrEmpty(conversionId))
                return Content(string.Empty);

            string counter = BasicCounter;

            counter = counter.Replace("#CONVERSION_ID#", conversionId);

            counter = counter.Replace("#PAGE_TYPE#", pageTypes.purchase.ToString());
            counter = counter.Replace("#PRODUCT_ID#", "[" + order.OrderItems.Select(item => "'" + 
                                    ProductService.GetProduct((int)item.ProductID).Offers.FirstOrDefault().OfferId + "'").AggregateString(", ") + "]");
            counter = counter.Replace("#TOTAL_VALUE#", order.OrderItems.Sum(item => item.Price * item.Amount).ToInvariantString());

            return Content(counter);
        }
    }
}
