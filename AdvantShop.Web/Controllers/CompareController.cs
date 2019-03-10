using System.Linq;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Modules;
using AdvantShop.Handlers.Compare;
using AdvantShop.Orders;
using AdvantShop.SEO;
using AdvantShop.ViewModel.Shared;
using AdvantShop.Web.Infrastructure.Extensions;

namespace AdvantShop.Controllers
{
    public partial class CompareController : BaseClientController
    {
        // GET: Compare products page
        public ActionResult Index()
        {
            var model = new CompareProductsHandler().Get();

            SetMetaInformation(
                new MetaInfo(string.Format("{0} - {1}", SettingsMain.ShopName, T("Compare.Index.ComparisonHeader"))),
                string.Empty);

            return View(model);
        }

        [ChildActionOnly]
        public ActionResult CompareBlock(CompareViewModel model)
        {
            if (!SettingsCatalog.EnableCompareProducts)
                return new EmptyResult();

            return PartialView("_CompareBlock", model);
        }

        [HttpPost]
        public JsonResult AddToComparison(int offerId)
        {
            var offer = OfferService.GetOffer(offerId);
            if (offer != null)
            {
                var item = new ShoppingCartItem
                {
                    OfferId = offer.OfferId,
                    Amount = 0,
                    ShoppingCartType = ShoppingCartType.Compare
                };

                ShoppingCartService.AddShoppingCartItem(item);

                ModulesExecuter.AddToCompare(item, Url.AbsoluteRouteUrl("Product", new { url = offer.Product.UrlPath }));

                return Json(new { Count = ShoppingCartService.CurrentCompare.Count(), isComplete = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Count = ShoppingCartService.CurrentCompare.Count(), isComplete = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult RemoveFromCompare(int offerId)
        {
            var item = ShoppingCartService.CurrentCompare.Find(p => p.OfferId == offerId);
            if (item != null)
            {
                ShoppingCartService.DeleteShoppingCartItem(item);
                return Json(new { isComplete = true, Count = ShoppingCartService.CurrentCompare.Count() });
            }
            return Json(new { isComplete = false, Count = ShoppingCartService.CurrentCompare.Count() });
        }

        [HttpGet]
        public JsonResult GetStatus(int offerId)
        {
            var result = false;

            if (offerId != 0)
                result = ShoppingCartService.CurrentWishlist.Any(item => item.OfferId == offerId);

            return Json(result);
        }
    }
}