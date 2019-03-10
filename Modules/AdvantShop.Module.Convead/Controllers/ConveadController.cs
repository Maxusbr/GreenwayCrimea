using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Core.Modules;
using AdvantShop.Orders;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Customers;
using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Bonuses.Model;

namespace AdvantShop.Module.Convead.Controllers
{
    public class ConveadController : Controller
    {
        private const string ModuleId = "Convead";

        public ActionResult ConveadScript()
        {
            var appKey = ModuleSettingsProvider.GetSettingValue<string>("APP_KEY", ModuleId);
            var customer = CustomerContext.CurrentCustomer;

            if (string.IsNullOrEmpty(appKey))
                return Content("");

            Card card = null;
            if (customer.BonusCardNumber != null) {
                card = BonusSystemService.GetCard(customer.Id);
            }

            return Content(
                "<script>window.ConveadSettings = {/* Use only [0-9a-z-] characters for visitor uid!*/" +
                (customer.CustomerRole == Role.Guest ? "" : ("visitor_uid: '" + customer.Id + "', ")) +
                "visitor_info: {" +
                    MainInfo("first_name", customer.FirstName) +
                    MainInfo("last_name", customer.LastName) +
                    MainInfo("email", customer.EMail) +
                    MainInfo("phone", customer.Phone) +
                    MainInfo("bonus_card", card != null ? card.CardNumber.ToString() : "") +
                    MainInfo("bonus_amount", card != null ? ((float)card.BonusAmount).ToInvariantString() : "") +
                    MainInfo("bonus_percent", card != null ? ((float)card.Grade.BonusPercent).ToInvariantString() : "") +
                /*date_of_birth: '1974-07-30',
                gender: 'male',
                my_custom_numeric_property: 1234,
                my_custom_string_property: 'foo',
                my_custom_date_property: '2014-04-20',
                my_custom_boolean_property: 'true'*/
                "}, app_key: \"" + appKey + "\"};" +

            "(function(w, d, c){ w[c] = w[c] || function(){ (w[c].q = w[c].q ||[]).push(arguments)}; var ts = (+new Date() / 86400000 | 0) * 86400; var s = d.createElement('script'); s.type = 'text/javascript'; s.async = true; s.charset = 'utf-8'; s.src = 'https://tracker.convead.io/widgets/' + ts + '/widget-" +
            appKey +
            ".js'; var x = d.getElementsByTagName('script')[0]; x.parentNode.insertBefore(s, x); })(window, document,'convead');" +
            "</script>"+
            "<script type=\"text/javascript\" src=\"modules/convead/js/tracking.js\" async></script>");
        }

        public ActionResult ProductScript(Product product, Offer offer) {
			var appKey = ModuleSettingsProvider.GetSettingValue<string>("APP_KEY", ModuleId);
			if (string.IsNullOrEmpty(appKey)) return Content("");
            return Content("<div style='display:none !important;'>" + 
                "<script>  convead('event', 'view_product', {" +
                "product_id: '" + offer.OfferId + "',"+
                "product_name: '" + GetRight(product.Name) + "',"+
                "product_url: '" + SettingsGeneral.AbsoluteUrlPath + UrlService.GetLink(ParamType.Product, product.UrlPath, product.ProductId) + "'});</script></div>");
        }

        public ActionResult CheckoutFinalStep(IOrder order)
        {
            var appKey = ModuleSettingsProvider.GetSettingValue<string>("APP_KEY", ModuleId);
            var customer = Customers.CustomerContext.CurrentCustomer;
            if (string.IsNullOrEmpty(appKey))
                return new EmptyResult();

            return Content(
                string.Format("<script>  convead('event', 'purchase', {{" +
                "order_id: '{0}', revenue: {1}, items: [{2}]}}, {{" +
                "first_name: '{3}', last_name: '{4}', email: '{5}', phone: '{6}'}});</script>",
                    order.OrderID,
                    order.Sum.ToString().Replace(",", "."),
                    string.Join(", ",
                        order.OrderItems.Select(
                            orderItem => string.Format("{{ product_id: '{0}', qnt: {1}, price: {2} }}",
                                OfferService.GetOffer(orderItem.ArtNo.ToString()).OfferId,
                                orderItem.Amount.ToString().Replace(",", "."),
                                orderItem.Price.ToString().Replace(",", ".")))),
                    GetRight(customer.FirstName),
                    GetRight(customer.LastName),
                    GetRight(customer.EMail),
                    GetRight(customer.Phone).Replace("-","")));            
        }

        public JsonResult Index()
        {
            var cartProducts = ShoppingCartService.CurrentShoppingCart;
            return Json(cartProducts.Select(item => new {
                product_id = item.OfferId,
                qnt = item.Amount,
                price = item.PriceWithDiscount //Было Price
            }), JsonRequestBehavior.AllowGet);
        }

        private string GetRight(string val)
        {
            return val != null ? val.Replace("'", @"\'") : string.Empty;
        }

        private string MainInfo(string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
                return name + ": '" + GetRight(value) + "',";
            return string.Empty;
        }
    }
}
