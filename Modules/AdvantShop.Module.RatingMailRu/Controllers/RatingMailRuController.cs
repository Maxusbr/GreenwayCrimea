using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Orders;

namespace AdvantShop.Module.RatingMailRu.Controllers
{
    public class RatingMailRuController : Controller
    {

        public ActionResult AfterBodyStart()
        {
            var counter = ModuleSettingsProvider.GetSettingValue<string>("COUNTER", Modules.RatingMailRu.ModuleID);

            if (string.IsNullOrEmpty(counter))
                return Content(string.Empty);

            return Content("<div style='display:none !important;'>" + counter + "</div>");
        }

        public ActionResult BeforeBodyEnd(string url, int? color, int? size)
        {
            var res = "";
            var listid = ModuleSettingsProvider.GetSettingValue<string>("PriceListID", Modules.RatingMailRu.ModuleID);

            if (listid.IsNullOrEmpty())
                listid = "1";

            string controller = ControllerContext.ParentActionViewContext.RouteData.Values["controller"].ToString().ToLower();
            if (controller == "home")
            {
                res =
                    string.Format(
                        "<script type='text/javascript'>var _tmr = _tmr || [];_tmr.push({{type: 'itemView', productid: '', pagetype: 'home',totalvalue: '', list: '{0}' }});</script>",
                        listid);
            }
            else if (controller == "catalog")
            {
                res =
                    string.Format(
                        "<script type='text/javascript'>var _tmr = _tmr || [];_tmr.push({{type: 'itemView', productid: '', pagetype: 'category', totalvalue:'', list: '{0}' }});</script>",
                        listid);
            }
            else if (controller == "product")
            {
                Offer mainOffer;
                var product = ProductService.GetProductByUrl(url);
                if (product != null &&
                    (mainOffer = OfferService.GetMainOffer(product.Offers, product.AllowPreOrder, color, size)) != null)
                {
                    res =
                        string.Format(
                            "<script type='text/javascript'>var _tmr = _tmr || [];_tmr.push({{type: 'itemView',productid: '{0}',pagetype: 'product', totalvalue:'{1}',list: '{2}' }});</script>",
                            mainOffer.OfferId, mainOffer.RoundedPrice.ToString("F2").Replace(",", "."), listid);
                }
                else
                {
                    res = string.Empty;
                }
            }
            else if (controller == "cart")
            {
                var cart = ShoppingCartService.CurrentShoppingCart;

                res =
                    string.Format(
                        "<script type='text/javascript'>var _tmr = _tmr || [];_tmr.push({{type: 'itemView',productid: [{0}], pagetype: 'cart', totalvalue:'{1}', list: '{2}' }});</script>",
                        cart.Select(o => "'" + o.OfferId + "'").AggregateString(","),
                        cart.TotalPrice.ToString("F2").Replace(",", "."), listid);
            }
            else
            {
                res =
                    string.Format(
                        "<script type='text/javascript'>var _tmr = _tmr || [];_tmr.push({{type: 'itemView', productid: '', pagetype: 'other', totalvalue: '', list: '{0}' }});</script>",
                        listid);
            }

            return Content(res);
        }

        public ActionResult CheckoutFinalStep(IOrder order)
        {
            var listid = ModuleSettingsProvider.GetSettingValue<string>("PriceListID", Modules.RatingMailRu.ModuleID);
            var offers = order.OrderItems.Select(item => OfferService.GetOffer(item.ArtNo));
            return
                Content(
                    string.Format(
                        "<script type='text/javascript'>var _tmr = _tmr || [];_tmr.push({{type: 'itemView', productid: [{0}], pagetype: 'purchase', totalvalue: '{1}', list: '{2}' }});</script>",
                        offers.Select(o => "'" + o.OfferId + "'").AggregateString(","),
                        order.OrderItems.Sum(item => item.Price*item.Amount), 
                        listid));
        }
    }
}
