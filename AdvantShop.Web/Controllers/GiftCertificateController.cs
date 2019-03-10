using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.Payment;
using AdvantShop.SEO;
using AdvantShop.ViewModel.GiftCertificate;
using AdvantShop.Web.Infrastructure.Controllers;
using BotDetect.Web.Mvc;

namespace AdvantShop.Controllers
{
    public class GiftCertificateController : BaseClientController
    {
        // GET: GiftCertificate
        public ActionResult Index()
        {
            if (!SettingsCheckout.EnableGiftCertificateService)
                return RedirectToRoute("Home");

            var customer = CustomerContext.CurrentCustomer;
            var payments = PaymentService.GetCertificatePaymentMethods();

            var model = new GiftCertificateViewModel()
            {
                PaymentMethods = payments,
                PaymentMethod = payments != null && payments.Count > 0 ? payments[0].PaymentMethodId : 0,
                PaymentKey = payments != null && payments.Count > 0 ? payments[0].PaymentKey.ToLower() : "",
                Sum = SettingsCheckout.MinimalPriceCertificate,
                EmailFrom = customer.RegistredUser ? customer.EMail : "",
                MinimumOrderPrice = CustomerGroupService.GetMinimumOrderPrice().FormatPrice(),
                MinimalPriceCertificate = SettingsCheckout.MinimalPriceCertificate.FormatPrice(),
                MaximalPriceCertificate = SettingsCheckout.MaximalPriceCertificate.FormatPrice()
            };

            SetMetaInformation(T("GiftCertificate.Index.Header"));
            SetNgController(NgControllers.NgControllersTypes.GiftCertificateCtrl);

            return View(model);
        }

        [HttpPost]
        [CaptchaValidation("CaptchaCode", "CaptchaSource")]
        public ActionResult Index(GiftCertificateViewModel model)
        {
            var isValid = model.NameFrom.IsNotEmpty() && model.NameTo.IsNotEmpty() &&
                                   ValidationHelper.IsValidEmail(model.EmailFrom) &&
                                   ValidationHelper.IsValidEmail(model.EmailTo);

            if (model.Sum < SettingsCheckout.MinimalPriceCertificate ||
                model.Sum > SettingsCheckout.MaximalPriceCertificate ||
                model.Sum < CustomerGroupService.GetMinimumOrderPrice())
            {
                isValid = false;
                ShowMessage(NotifyType.Error, T("GiftCertificate.Index.WrongSum"));
            }

            var payments = PaymentService.GetCertificatePaymentMethods();

            if (payments.Find(x => x.PaymentMethodId == model.PaymentMethod) == null)
            {
                isValid = false;
                ShowMessage(NotifyType.Error, T("GiftCertificate.Index.WrongPaymentMethod"));
            }

            if (SettingsMain.EnableCaptchaInGiftCerticate)
            {
                if (!ModelState.IsValidField("CaptchaCode"))
                {
                    isValid = false;
                    ShowMessage(NotifyType.Error, T("Captcha.Wrong"));
                }
                MvcCaptcha.ResetCaptcha("CaptchaSource");
            }

            if (SettingsCheckout.IsShowUserAgreementText && !model.Agreement)
            {
                isValid = false;
                ShowMessage(NotifyType.Error, T("User.Registration.ErrorAgreement"));
            }

            if (!isValid)
            {
                model.PaymentMethods = payments;
                model.PaymentMethod = payments.Count > 0 ? payments[0].PaymentMethodId : 0;
                model.PaymentKey = payments.Count > 0 ? payments[0].PaymentKey.ToLower() : "";

                SetMetaInformation(
                    new MetaInfo(string.Format("{0} - {1}", SettingsMain.ShopName, T("GiftCertificate.Index.Header"))),
                    string.Empty);

                return View("Index", model);
            }

            var giftCertificateModel = new GiftCertificateOrderModel()
            {
                GiftCertificate = new GiftCertificate
                {
                    CertificateCode = GiftCertificateService.GenerateCertificateCode(),
                    ToName = model.NameTo,
                    FromName = model.NameFrom,
                    Sum = model.Sum,
                    CertificateMessage = model.Message,
                    Enable = true,
                    ToEmail = model.EmailTo
                },
                EmailFrom = model.EmailFrom,
                PaymentId = model.PaymentMethod,
                Phone = model.Phone
            };

            var order = GiftCertificateService.CreateCertificateOrder(giftCertificateModel);
            if (order != null && order.OrderID != 0)
            {
                TempData["orderid"] = order.OrderID.ToString();
                return RedirectToRoute("CheckoutSuccess", new {code = order.Code});
            }

            return View("Index", model);
        }

        [ChildActionOnly]
        public ActionResult GiftCertificateModal(bool isModal, string code)
        {
            var model = new GiftCertificateModalViewModel()
            {
                IsModal = isModal,
                GiftCertificate =
                    !string.IsNullOrWhiteSpace(code)
                        ? GiftCertificateService.GetCertificateByCode(code)
                        : null
            };

            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult GiftCertificateBlock()
        {
            if (!SettingsDesign.GiftSertificateVisibility || !SettingsCheckout.EnableGiftCertificateService)
                return new EmptyResult();

            return PartialView();
        }

        public ActionResult Print(string code)
        {
            if (string.IsNullOrWhiteSpace(code) || GiftCertificateService.GetCertificateByCode(code) == null)
                return Error404();
            
            return View(new PrintGiftCertificateViewModel(){Code = code});
        }
    }
}