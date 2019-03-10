using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Core.Modules;
using AdvantShop.Module.YaMetrika.Model;
using AdvantShop.Orders;
using AdvantShop.Web.Infrastructure.Controllers;
using Newtonsoft.Json;
using AdvantShop.Core.Services.Catalog;

namespace AdvantShop.Module.YaMetrika.Controllers
{
    public class YaMetrikaController : ModuleController
    {
        private const string ModuleId = "YaMetrika";

        private bool ModuleActive()
        {
            var counterId = ModuleSettingsProvider.GetSettingValue<string>("COUNTER_ID", ModuleId);
            return !string.IsNullOrEmpty(counterId);
        }

        [ChildActionOnly]
        public ActionResult YaMetrikaScript()
        {
            var counterId = ModuleSettingsProvider.GetSettingValue<string>("COUNTER_ID", ModuleId);
            var counter = ModuleSettingsProvider.GetSettingValue<string>("COUNTER", ModuleId);
            var collectip = ModuleSettingsProvider.GetSettingValue<bool>("CollectIp", ModuleId);

            if (string.IsNullOrEmpty(counterId) || string.IsNullOrEmpty(counter))
                return new EmptyResult();

            return Content(string.Format(
                        (collectip ? "<script>var yaParams={{ip_adress: '" + Request.UserHostAddress + "'}}</script>" : "") +
                       "<div style='display:none !important;'>{0}</div>" +
                       "<script type=\"text/javascript\" src=\"modules/yametrika/js/tracking.js?v=2\" async></script> " +
                       "<div class=\"yacounterid\" data-counterId=\"{1}\"></div>", counter, counterId));
        }

        [ChildActionOnly]
        public ActionResult CheckoutFinalStep(IOrder order)
        {
            var counterId = ModuleSettingsProvider.GetSettingValue<string>("COUNTER_ID", ModuleId);
            if (string.IsNullOrEmpty(counterId) || !ModuleSettingsProvider.GetSettingValue<bool>("OldApiEnabled", ModuleId))
                return new EmptyResult();

            return Content(
                string.Format(
                    "<script type=\"text/javascript\">\r\n" +
                        "(function yaMetrikaWatcher()\r\n" +
                        "{{\r\n" +
                            "if (typeof(window.yaCounter{4}) != \"undefined\")\r\n" +
                            "{{\r\n" +
                                "var yaParams = {{ order_id:\"{0}\", order_price: {1}, currency: \"{2}\", exchange_rate: 1, goods: [{3}]}};\r\n" +
                                "yaCounter{4}.reachGoal('Order', yaParams);\r\n" +
                            "}}\r\n" +
                            "else\r\n" +
                            "{{\r\n" +
                                "setTimeout(yaMetrikaWatcher, 1000);\r\n" +
                            " }}\r\n" +
                        "}})();\r\n" +
                    "</script>\r\n",
                    order.OrderID,
                    (int)order.Sum,
                    order.OrderCurrency.CurrencyCode,
                    string.Join(", ",
                        order.OrderItems.Select(
                            orderItem => string.Format("{{ id: '{0}', name: '{1}', price: {2}, quantity: {3} }}",
                                HttpUtility.HtmlEncode(orderItem.ArtNo), HttpUtility.HtmlEncode(orderItem.Name),
                                (int)orderItem.Price,
                                (int)orderItem.Amount))),
                    counterId));
        }

        #region Ecommerce Api

        [ChildActionOnly]
        public ActionResult EcommerceDataLayer()
        {
            if (!ModuleActive() || !ModuleSettingsProvider.GetSettingValue<bool>("EcomerceApiEnabled", ModuleId))
                return new EmptyResult();

            return Content("\n<script> window.dataLayer = window.dataLayer || []; </script>\n ");
        }

        [ChildActionOnly]
        public ActionResult EcommercePropductDetail(Product product, Offer offer, float finalPrice, Discount finalDiscount)
        {
            if (!ModuleActive() || !ModuleSettingsProvider.GetSettingValue<bool>("EcomerceApiEnabled", ModuleId))
                return new EmptyResult();

            if (product == null)
                return new EmptyResult();

            var categories = product.ProductCategories;
            if (categories.Count > 5)
                categories = categories.Skip(categories.Count - 5).ToList();

            int? price = finalPrice != 0 ? (int)finalPrice : (int?)null;

            if (price == null && offer != null)
                price = (int)offer.RoundedPrice;


            var ecProduct = new EcommerceProduct()
            {
                Id = offer != null ? offer.ArtNo : product.ArtNo,
                Name = product.Name,
                Price = price.HasValue ? (int)price : 0,
                Category = String.Join("/", categories.Select(x => x.Name)),
                Brand = product.Brand != null ? product.Brand.Name : null,
            };

            var model = new EcommerceProductsModel() { Products = new List<EcommerceProduct>() { ecProduct } };

            var result =
                string.Format("\n<script> window.dataLayer.push({{ \"ecommerce\": {{ \"detail\": {0} }} }}); </script>\n ",
                    JsonConvert.SerializeObject(model));

            return Content(result);
        }

        [ChildActionOnly]
        public ActionResult EcommerceCheckoutFinalStep(IOrder order)
        {
            if (!ModuleActive() || !ModuleSettingsProvider.GetSettingValue<bool>("EcomerceApiEnabled", ModuleId))
                return new EmptyResult();

            var ecOrder = OrderService.GetOrder(order.OrderID);

            var actionField = new EcommerceActionField()
            {
                Id = order.Number,
                Shipping = ecOrder.ShippingCost != 0 ? (int)ecOrder.ShippingCost : 0,
                Coupon = order.Coupon != null ? order.Coupon.Code : null
            };

            var products = order.OrderItems.Select(x => new EcommerceProduct()
            {
                Id = x.ArtNo,
                Name = x.Name,
                Price = (int)x.Price,
                Category = x.ProductID != null ? GetCategory(x.ProductID.Value) : null,
                Brand = x.ProductID != null ? GetBrand(x.ProductID.Value) : null,
				Quantity = Convert.ToInt32(x.Amount)
            });

            var result =
                string.Format(
                    "\n<script> \n window.dataLayer.push({{\"ecommerce\": {{ \"purchase\": \n {{ \"actionField\": {0}, \n  \"products\":{1} }} \n }} \n }}); </script>\n ",
                    JsonConvert.SerializeObject(actionField),
                    JsonConvert.SerializeObject(products));

            return Content(result);
        }

        private string GetCategory(int productId)
        {
            var categories = ProductService.GetCategoriesByProductId(productId);
            if (categories.Count > 5)
                categories = categories.Skip(categories.Count - 5).ToList();

            return String.Join("/", categories.Select(x => x.Name));
        }

        private string GetBrand(int productId)
        {
            var product = ProductService.GetProduct(productId);
            if (product == null || product.Brand == null)
                return null;

            return product.Brand.Name;
        }

        #endregion

        [HttpGet]
        public JsonResult GetProductById(int productId, int offerId, int? cartId)
        {
            var customer = Customers.CustomerContext.CurrentCustomer;
            var currentCart = ShoppingCartService.GetAllShoppingCarts(customer.Id);
            var cartItem = currentCart != null ? currentCart.Where(x => x.ShoppingCartItemId == cartId).ToList() : null;
            if (offerId != 0)
            {
                var offer = OfferService.GetOffer(offerId);
                if (offer != null && cartItem.Count > 0)
                {
                    return EcommercProductAdd(ProductService.GetProduct(offer.ProductId), offer, cartItem != null?  (float?)cartItem[0].PriceWithDiscount : null);
                }
            }

            if (productId != 0)
            {
                var product = ProductService.GetProduct(productId);
                if (product != null && product.Offers.Count > 0 && cartItem.Count > 0)
                {
                    return EcommercProductAdd(product, product.Offers[0], cartItem != null ? (float?)cartItem[0].PriceWithDiscount : null);
                }
            }

            return Json(new { artno = "", name = "" });
        }

        [HttpGet]
        public JsonResult GetProductByOfferId(int offerId)
        {
            if (offerId != 0)
            {
                var offer = OfferService.GetOffer(offerId);
                if (offer != null)
                    return Json(new { artno = offer.ArtNo, name = offer.Product.Name });
            }

            return Json(new { artno = "", name = "" });
        }

        #region Help method

        private JsonResult EcommercProductAdd(Product product, Offer offer, float? price)
        {
            int? prices = (int?)price;
            if (prices == null && offer != null)
                prices = (int)offer.RoundedPrice;

            var brands = product.Brand != null ? product.Brand.Name : null;

            var categories = product.ProductCategories;
            if (categories.Count > 5)
                categories = categories.Skip(categories.Count - 5).ToList();

            var category = String.Join("/", categories.Select(x => x.Name));

            return Json(new { artno = offer.ArtNo, name = offer.Product.Name, price = prices, brand = brands, category = category, amount = offer.Product.MinAmount ?? offer.Product.Multiplicity });

        }

        #endregion
    }
}
