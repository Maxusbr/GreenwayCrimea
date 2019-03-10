using System.Web.Mvc;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Core.Services.SEO;
using AdvantShop.Handlers.Checkout;
using AdvantShop.Models.Checkout;
using AdvantShop.Orders;
using AdvantShop.ViewModel.Checkout;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Extensions;
using AdvantShop.Core.Services.Configuration.Settings;

namespace AdvantShop.Areas.Mobile.Controllers
{
    public class CheckoutMobileController : BaseMobileController
    {
        public ActionResult Index()
        {
            var cart = ShoppingCartService.CurrentShoppingCart;
            if (!cart.CanOrder)
                return RedirectToRoute("Cart");

            if (SettingsMobile.IsFullCheckout)
            {
                return RedirectToRoute("Checkout");
            }

            SetMetaInformation(T("Checkout.Index.CheckoutTitle"));

            return View();
        }

        public JsonResult Confirm(string name, string phone, string email, string message)
        {
            var model = new BuyOneInClickJsonModel()
            {
                Page = BuyInOneclickPage.Checkout,
                Name = name,
                Phone = phone,
                Comment = message,
                Email = email,
                OrderType = OrderType.Mobile
            };

            var result = new BuyInOneClickHandler(model).Create();

            return Json(result);
        }

        public ActionResult Success(string code)
        {
            SetNgController(NgControllers.NgControllersTypes.CheckOutSuccessCtrl);
            SetMetaInformation(T("Checkout.Index.CheckoutTitle"));

            var model = new CheckoutSuccess();

            var order = OrderService.GetOrderByCode(code);
            if (order == null)
                return View(model);

            model = new CheckoutSuccessHandler().Get(order);

            var tagManager = GoogleTagManagerContext.Current;
            tagManager.CreateTransaction(order);

            WriteLog("", Url.AbsoluteRouteUrl("CheckoutSuccess"), ePageType.purchase);

            return View(model);
        }
    }
}