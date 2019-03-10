using System.Linq;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Handlers.PreOrderProducts;
using AdvantShop.Orders;
using AdvantShop.SEO;
using AdvantShop.ViewModel.PreOrder;
using AdvantShop.Helpers;
using AdvantShop.Web.Infrastructure.Controllers;
using BotDetect.Web.Mvc;

namespace AdvantShop.Controllers
{
    public partial class PreOrderController : BaseClientController
    {
        public ActionResult Index(int offerId = 0, float amount = 1, string options = null)
        {
            if (offerId == 0)
                return Error404();

            var model = new PreOrderHandler().Get(new PreOrderViewModel()
            {
                OfferId = offerId,
                Amount = amount,
                Options = options
            });
            if (model == null)
                return Error404();

            SetNgController(NgControllers.NgControllersTypes.PreorderCtrl);

            SetMetaInformation(
                new MetaInfo(string.Format("{0} - {1} - {2}", model.Offer.Product.Name, T("PreOrder.Index.OrderByRequest"), SettingsMain.ShopName)),
                string.Empty);

            return View(model);
        }

        [HttpPost]
        [CaptchaValidation("CaptchaCode", "CaptchaSource")]
        public ActionResult Index(PreOrderViewModel requestModel)
        {
            var isValid = requestModel.Name.IsNotEmpty() && requestModel.Email.IsNotEmpty() && requestModel.Phone.IsNotEmpty();
            isValid &= requestModel.Agreement == SettingsCheckout.IsShowUserAgreementText;
            isValid &= requestModel.Amount >= 1;
            isValid &= ValidationHelper.IsValidEmail(requestModel.Email);

            if (SettingsMain.EnableCaptchaInPreOrder)
            {
                if (!ModelState.IsValidField("CaptchaCode"))
                    isValid = false;

                MvcCaptcha.ResetCaptcha("CaptchaSource");
            }

            var handler = new PreOrderHandler();
            var model = handler.Get(requestModel);

           if(model == null)
                return Error404();
            SetMetaInformation(
                new MetaInfo(string.Format("{0} - {1}", SettingsMain.ShopName, T("PreOrder.Index.OrderByRequest"))),
                string.Empty);

            if (!isValid)
            {
                if (requestModel.Agreement != SettingsCheckout.IsShowUserAgreementText)
                    ShowMessage(NotifyType.Error, T("Js.Subscribe.ErrorAgreement"));
                else
                    ShowMessage(NotifyType.Error, T("PreOrder.Index.WrongData"));
                return View("Index", model);
            }

            var isSendSuccess = handler.Send(requestModel, model.Offer);

            if (isSendSuccess)
            {
                model.IsSuccess = true;
                model.SuccessText = T("PreOrder.Index.MessageSent");
            }

            return View("Success");
        }

        public ActionResult LinkByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return RedirectToRoute("Home");

            SetMetaInformation(T("PreOrder.LinkByCode.Header"));

            var model = new LinkByCodeViewModel();

            // Если код правильный, и такого же товара нет в корзине - то всё ок.
            var orderByRequest = OrderByRequestService.GetOrderByRequest(code);
            if (orderByRequest == null)
            {
                model.Error = T("PreOrder.Index.OrderProductMessage");
                return View(model);
            }

            if (ShoppingCartService.CurrentShoppingCart.Any(p => p.Offer.OfferId == orderByRequest.OfferId))
            {
                var item = ShoppingCartService.CurrentShoppingCart.First(p => p.Offer.OfferId == orderByRequest.OfferId);
                if (item.Amount != orderByRequest.Quantity)
                {
                    item.Amount = orderByRequest.Quantity;
                    ShoppingCartService.UpdateShoppingCartItem(item);
                }
                return RedirectToRoute("Cart");
            }

            var offer = OfferService.GetOffer(orderByRequest.OfferId);
            if (orderByRequest.IsValidCode && ProductService.IsExists(orderByRequest.ProductId) && offer != null && offer.BasePrice > 0)
            {
                ShoppingCartService.AddShoppingCartItem(new ShoppingCartItem
                {
                    OfferId = orderByRequest.OfferId,
                    Amount = orderByRequest.Quantity,
                    ShoppingCartType = ShoppingCartType.ShoppingCart,
                    AttributesXml = orderByRequest.Options,
                    AddedByRequest = orderByRequest.Quantity > offer.Amount
                });

                return RedirectToRoute("Cart");
            }
            else
            {
                model.Error = T("PreOrder.Index.OrderProductMessage");
            }

            return View(model);
        }
    }
}