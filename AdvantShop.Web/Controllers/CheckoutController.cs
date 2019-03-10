using AdvantShop.Configuration;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Core.Services.SEO;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Extensions;
using AdvantShop.Handlers.Checkout;
using AdvantShop.Helpers;
using AdvantShop.Models.Checkout;
using AdvantShop.Orders;
using AdvantShop.Payment;
using AdvantShop.Repository.Currencies;
using AdvantShop.Shipping;
using AdvantShop.Trial;
using AdvantShop.ViewModel.Checkout;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using BotDetect.Web.Mvc;
using AdvantShop.Core.Services.Configuration.Settings;

namespace AdvantShop.Controllers
{
    public class CheckoutController : BaseClientController
    {
        #region Checkout

        // GET: /checkout
        public ActionResult Index()
        {
            var cart = ShoppingCartService.CurrentShoppingCart;
            if (!cart.CanOrder)
                return RedirectToRoute("Cart");

            var showConfirmButtons =
                AttachedModules.GetModules<IShoppingCart>()
                    .Select(moduleType => (IShoppingCart)Activator.CreateInstance(moduleType))
                    .Aggregate(true, (current1, module) => current1 & module.ShowConfirmButtons);

            if (!showConfirmButtons)
                return RedirectToRoute("Cart");

            if (MobileHelper.IsMobileEnabled() && !SettingsMobile.IsFullCheckout)
            {
                return Redirect("mobile/checkoutmobile/index");
            }

            var customer = CustomerContext.CurrentCustomer;
            var current = MyCheckout.Factory(customer.Id);

            if (customer.RegistredUser)
            {
                current.Data.User.Id = customer.Id;
                current.Data.User.Email = customer.EMail;
                current.Data.User.FirstName = customer.FirstName;
                current.Data.User.LastName = customer.LastName;
                current.Data.User.Patronymic = customer.Patronymic;
                current.Data.User.Phone = customer.Phone;

                var card = BonusSystemService.GetCard(customer.Id);
                current.Data.User.BonusCardId = card != null ? customer.Id : default(Guid?);

                current.Data.User.ManagerId = customer.ManagerId;
                current.Data.User.CustomerFields = CustomerFieldService.GetCustomerFieldsWithValue(customer.Id).Where(x => x.ShowInClient).ToList();
                current.Update();
            }

            var tagManager = GoogleTagManagerContext.Current;
            if (tagManager.Enabled)
            {
                tagManager.PageType = ePageType.order;
                tagManager.ProdIds = cart.Select(item => item.Offer.ArtNo).ToList();
                tagManager.Products = cart.Select(x => new TransactionProduct()
                {
                    SKU = x.Offer.ArtNo,
                    Category = x.Offer.Product.MainCategory != null ? x.Offer.Product.MainCategory.Name : string.Empty,
                    Name = x.Offer.Product.Name,
                    Price = x.Price,
                    Quantity = x.Amount
                }).ToList();
                tagManager.TotalValue = cart.TotalPrice;
            }

            SetNgController(NgControllers.NgControllersTypes.CheckOutCtrl);
            SetMetaInformation(T("Checkout.Index.CheckoutTitle"));

            WriteLog("", Url.AbsoluteRouteUrl("Checkout"), ePageType.order);
            return View();
        }

        // POST: Confirm order
        [HttpPost, ValidateAntiForgeryToken]
        [CaptchaValidation("CaptchaCode", "CaptchaSource")]
        public ActionResult IndexPost(CheckoutModel checkoutModel)
        {
            var cart = ShoppingCartService.CurrentShoppingCart;
            if (!cart.CanOrder)
                return RedirectToRoute("Cart");

            if (SettingsMain.EnableCaptchaInCheckout)
            {
                if (!ModelState.IsValidField("CaptchaCode"))
                {
                    ShowMessage(NotifyType.Error, T("Captcha.Wrong"));
                    return RedirectToAction("Index");
                }
                MvcCaptcha.ResetCaptcha("CaptchaSource");
            }

            var orderCode = "";
            var current = MyCheckout.Factory(CustomerContext.CustomerId);

            if (ShoppingCartService.CurrentShoppingCart.GetHashCode() != current.Data.ShopCartHash)
                return RedirectToAction("Index");

            if (current.Data.User.WantRegist && !ValidationHelper.IsValidEmail(current.Data.User.Email))
            {
                ShowMessage(NotifyType.Error, T("User.Registration.ErrorCustomerExist"));
                return RedirectToAction("Index");
            }

            var valid = current.Data.SelectShipping.Validate();
            if (!valid.IsValid)
            {
                ShowMessage(NotifyType.Error, valid.ErrorMessage);
                return RedirectToAction("Index");
            }

            try
            {
                var order = current.ProcessOrder(checkoutModel.CustomData, checkoutModel.OrderType);
                orderCode = order.Code.ToString();
                TempData["orderid"] = order.OrderID.ToString();
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                ShowMessage(NotifyType.Error, "Error");
                return RedirectToAction("Index");
            }

            this.ToTempData();
            
            return RedirectToRoute("CheckoutSuccess", new { code = orderCode });
        }

        // GET: /checkout/success
        public ActionResult Success(string code)
        {
            this.ToContext();
            if (string.IsNullOrWhiteSpace(code))
                return Error404();

            var tempOrderId = TempData["orderid"] != null ? TempData["orderid"].ToString() : string.Empty;

            var order = OrderService.GetOrderByCode(code);
            if (order == null || order.OrderID.ToString() != tempOrderId)
                return View("OrderComplete");

            var model = new CheckoutSuccessHandler().Get(order);

            var tagManager = GoogleTagManagerContext.Current;
            tagManager.CreateTransaction(order);

            TrialService.TrackEvent(TrialEvents.CheckoutOrder, order.OrderID.ToString());
            TrialService.TrackEvent(ETrackEvent.Trial_AddOrderFromClientSide);

            SetNgController(NgControllers.NgControllersTypes.CheckOutSuccessCtrl);
            SetMetaInformation(T("Checkout.Index.CheckoutTitle"));

            WriteLog("", Url.AbsoluteRouteUrl("CheckoutSuccess"), ePageType.purchase);
            return View(model);
        }

        public JsonResult GetPaymentButton(int orderid)
        {
            var order = OrderService.GetOrder(orderid);
            var shouldBeConfirmedByManager = SettingsCheckout.ManagerConfirmed && !order.ManagerConfirmed;

            var script = shouldBeConfirmedByManager && !(order.PaymentMethod is ICreditPaymentMethod)
                ? string.Empty
                : OrderService.ProcessOrder(order, PageWithPaymentButton.orderconfirmation);

            return Json(new { script, proceedToPayment = SettingsCheckout.ProceedToPayment });
        }

        [ChildActionOnly]
        public ActionResult CheckoutUser()
        {
            return PartialView(new CheckoutUserHandler().Execute());
        }

        [HttpPost]
        public JsonResult CheckoutUserPost(CheckoutUser customer)
        {
            var current = MyCheckout.Factory(CustomerContext.CustomerId);
            current.Data.User.Email = customer.Email;
            current.Data.User.FirstName = customer.FirstName;
            current.Data.User.LastName = customer.LastName;
            current.Data.User.Patronymic = customer.Patronymic;
            current.Data.User.Phone = customer.Phone;
            current.Data.User.WantRegist = customer.WantRegist;
            current.Data.User.Password = customer.Password;
            current.Data.User.CustomerFields = customer.CustomerFields;
            current.Update();

            return Json(true);
        }

        [HttpPost]
        public JsonResult SaveWantBonusCard(bool wantBonusCard)
        {
            var current = MyCheckout.Factory(CustomerContext.CustomerId);
            current.Data.User.WantBonusCard = wantBonusCard;
            current.Update();

            return Json(true);
        }

        [HttpPost]
        public JsonResult CheckoutContactPost(CheckoutAddress address)
        {
            var current = MyCheckout.Factory(CustomerContext.CustomerId);

            if (current.Data.Contact == address)
                return Json(true);

            current.Data.Contact = address;

            var options = current.AvailableShippingOptions();

            if (current.Data.SelectShipping == null || !options.Any(x => x.Id == current.Data.SelectShipping.Id))
            {
                current.Data.SelectShipping = null;
            }

            current.Update();

            return Json(true);
        }

        [ChildActionOnly]
        public ActionResult CheckoutShipping()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult CheckoutShippingJson(List<PreOrderItem> preorderList = null)
        {
            var current = MyCheckout.Factory(CustomerContext.CustomerId);
            var options = current.AvailableShippingOptions(preorderList);
            current.UpdateSelectShipping(preorderList, current.Data.SelectShipping, options);

            return Json(new { selectShipping = current.Data.SelectShipping, option = options });
        }

        [HttpPost]
        public JsonResult CheckoutShippingPost(BaseShippingOption shipping, List<PreOrderItem> preorderList = null)
        {
            var current = MyCheckout.Factory(CustomerContext.CustomerId);
            current.UpdateSelectShipping(preorderList, shipping);

            return Json(new { selectShipping = current.Data.SelectShipping });
        }

        [ChildActionOnly]
        public ActionResult CheckoutPayment()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult CheckoutPaymentJson(List<PreOrderItem> preorderList = null)
        {
            var current = MyCheckout.Factory(CustomerContext.CustomerId);
            var options = current.AvailablePaymentOptions(preorderList);

            var cookiePayment = CommonHelper.GetCookie("payment");
            if (cookiePayment != null && !string.IsNullOrEmpty(cookiePayment.Value))
            {
                var paymentId = cookiePayment.Value.TryParseInt();
                current.Data.SelectPayment = options.FirstOrDefault(x => x.Id == paymentId);
                CommonHelper.DeleteCookie("payment");
            }

            current.UpdateSelectPayment(preorderList, current.Data.SelectPayment, options);
            return Json(new { selectPayment = current.Data.SelectPayment, option = options });
        }

        [HttpPost]
        public JsonResult CheckoutPaymentPost(BasePaymentOption payment, List<PreOrderItem> preorderList = null)
        {
            var current = MyCheckout.Factory(CustomerContext.CustomerId);
            var temp = current.UpdateSelectPayment(preorderList, payment);
            return Json(temp);
        }

        [ChildActionOnly]
        public ActionResult CheckoutShippingAddress()
        {
            var hasAddresses = CustomerContext.CurrentCustomer.Contacts.Any();
            var hasCustomShippingFields = SettingsCheckout.IsShowCustomShippingField1 ||
                                          SettingsCheckout.IsShowCustomShippingField2 ||
                                          SettingsCheckout.IsShowCustomShippingField3;

            if (hasAddresses && !hasCustomShippingFields)
                return new EmptyResult();

            var current = MyCheckout.Factory(CustomerContext.CustomerId);
            var model = new CheckoutShippingAddressViewModel()
            {
                AddressContact = current.Data.Contact,
                HasAddresses = hasAddresses,
                HasCustomShippingFields = hasCustomShippingFields
            };

            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult CheckoutBonus()
        {
            var model = new CheckoutBonusHandler().Execute();
            if (model == null)
                return new EmptyResult();

            if (!model.HasCard && !CustomerContext.CurrentCustomer.RegistredUser)
                return new EmptyResult();

            return PartialView(model);
        }

        public JsonResult CheckoutBonusAutorizePost(long cardNumber)
        {
            if (!BonusSystem.IsActive)
                return Json(false);

            var card = BonusSystemService.GetCard(cardNumber);
            if (card != null)
            {
                var current = MyCheckout.Factory(CustomerContext.CustomerId);
                current.Data.User.BonusCardId = card.CardId;
                current.Update();
            }
            return Json(true);
        }

        public JsonResult CheckoutBonusApplyPost(bool isApply)
        {
            if (!BonusSystem.IsActive)
                return Json(false);
            if (isApply && BonusSystem.ForbidOnCoupon && ShoppingCartService.CurrentShoppingCart.Coupon != null)
            {
                return Json(new { result = false, msg = "Нельзя применить бонусы при использовании купона" });
            }

            var current = MyCheckout.Factory(CustomerContext.CustomerId);
            current.Data.Bonus = current.Data.Bonus ?? new CheckoutBonus();
            current.Data.Bonus.UseIt = isApply;
            current.Update();
            return Json(true);
        }

        [ChildActionOnly]
        public ActionResult CheckoutCoupon()
        {
            bool isRenderView = CustomerContext.CurrentCustomer.CustomerGroupId == CustomerGroupService.DefaultCustomerGroup
                && SettingsCheckout.DisplayPromoTextbox;

            return PartialView(isRenderView);
        }

        [HttpPost]
        public ActionResult CheckoutCouponApplied()
        {
            var current = MyCheckout.Factory(CustomerContext.CustomerId);
            current.Data.ShopCartHash = ShoppingCartService.CurrentShoppingCart.GetHashCode();
            current.Update();

            return Json(true);
        }


        [ChildActionOnly]
        public ActionResult CheckoutSummary()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult CheckoutComment()
        {
            var current = MyCheckout.Factory(CustomerContext.CustomerId);
            return PartialView("CheckoutComment", current.Data);
        }

        [ChildActionOnly]
        public ActionResult CheckoutCart()
        {
            var current = MyCheckout.Factory(CustomerContext.CustomerId);
            current.Data.ShopCartHash = ShoppingCartService.CurrentShoppingCart.GetHashCode();
            current.Update();

            return PartialView();
        }

        public JsonResult CheckoutCartJson()
        {
            var current = MyCheckout.Factory(CustomerContext.CustomerId);

            var shippingPrice = current.Data.SelectShipping != null ? current.Data.SelectShipping.Rate : 0;
            var paymentCost = current.Data.SelectPayment != null ? current.Data.SelectPayment.Rate : 0;
            var currency = CurrencyService.CurrentCurrency;

            var model = new CheckoutCartHandler().Get(current.Data, ShoppingCartService.CurrentShoppingCart, shippingPrice, paymentCost, currency);

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CommentPost(string message)
        {
            var current = MyCheckout.Factory(CustomerContext.CustomerId);
            current.Data.CustomerComment = message;
            current.Update();
            return Json(true);
        }

        #endregion

        #region Buy in one click

        [HttpPost]
        public JsonResult CheckoutBuyInOneClick(BuyOneInClickJsonModel model)
        {
            var returnModel = new BuyInOneClickHandler(model).Create();
            TempData["orderid"] = returnModel.orderId;
            return Json(returnModel);
        }

        public JsonResult CheckoutBuyInOneClickFields()
        {
            var obj = new
            {
                SettingsCheckout.IsShowBuyInOneClickName,
                SettingsCheckout.IsShowBuyInOneClickEmail,
                SettingsCheckout.IsShowBuyInOneClickPhone,
                SettingsCheckout.IsShowBuyInOneClickComment,
                SettingsCheckout.BuyInOneClickName,
                SettingsCheckout.BuyInOneClickEmail,
                SettingsCheckout.BuyInOneClickPhone,
                SettingsCheckout.BuyInOneClickComment,
                SettingsCheckout.IsRequiredBuyInOneClickName,
                SettingsCheckout.IsRequiredBuyInOneClickEmail,
                SettingsCheckout.IsRequiredBuyInOneClickPhone,
                SettingsCheckout.IsRequiredBuyInOneClickComment,
                BuyInOneClickFinalText = T("Checkout.BuyInOneClickFinalText"),
                SettingsCheckout.BuyInOneClickFirstText,
                SettingsCheckout.BuyInOneClickButtonText,
                SettingsCheckout.BuyInOneClickLinkText,
                SettingsCheckout.IsShowUserAgreementText,
                SettingsCheckout.UserAgreementText
            };

            return Json(obj);
        }

        public JsonResult CheckoutBuyInOneClickCustomer()
        {
            var obj = new
            {
                name = "",
                email = "",
                phone = ""
            };

            if (CustomerContext.CurrentCustomer != null)
            {
                obj = new
                {
                    name = CustomerContext.CurrentCustomer.FirstName + " " + CustomerContext.CurrentCustomer.LastName,
                    email = CustomerContext.CurrentCustomer.EMail,
                    phone = CustomerContext.CurrentCustomer.Phone
                };

            }
            return Json(obj);
        }


        #endregion

        #region Print Order

        public ActionResult PrintOrder(string code, bool showMap = false)
        {
            if (string.IsNullOrWhiteSpace(code))
                return Error404();

            var order = OrderService.GetOrderByCode(code);
            if (order == null)
                return Error404();

            if (!CustomerContext.CurrentCustomer.IsAdmin &&
                (CustomerContext.CurrentCustomer.CustomerRole != Role.Moderator ||
                 !CustomerContext.CurrentCustomer.HasRoleAction(RoleAction.Orders)))
            {
                if (order.OrderDate.AddDays(3) < DateTime.Now &&
                    (order.OrderCustomer == null || order.OrderCustomer.CustomerID != CustomerContext.CustomerId))
                    return Error404();
            }

            var model = new PrintOrderHandler(order, showMap).Execute();

            SettingsDesign.IsMobileTemplate = false;

            return View(model);
        }

        #endregion

        #region Billing page

        public ActionResult Billing(string number, string hash)
        {
            if (String.IsNullOrWhiteSpace(number) || String.IsNullOrWhiteSpace(hash))
                return Error404();

            var order = OrderService.GetOrderByNumber(number);
            if (order == null || hash != OrderService.GetBillingLinkHash(order))
            {
                return Error404();
            }

            var model = new BillingViewModel() { Order = order, IsMobile = MobileHelper.IsMobileForced() };

            model.Header = T("Checkout.Billing.BillingTitle") + " " + order.OrderID + (order.Payed ? " - " + T("Core.Orders.Order.OrderPaied")
                : order.OrderStatus.IsCanceled ? T("Core.Crm.LeadStatus.NotClosedDeal") : "");

            SetNgController(NgControllers.NgControllersTypes.BillingCtrl);
            SetMetaInformation(model.Header);
            SettingsDesign.IsMobileTemplate = false;
            return View(model);
        }

        [HttpPost]
        public JsonResult BillingPaymentJson(int orderId)
        {
            var order = OrderService.GetOrder(orderId);
            if (order == null)
                return Json("error");

            var shippingMethod = ShippingMethodService.GetShippingMethod(order.ShippingMethodId);

            BaseShippingOption shipping = null;

            if (shippingMethod != null)
            {
                shipping = new BaseShippingOption(shippingMethod);
            }
            else if (order.ArchivedShippingName == LocalizationService.GetResource("Core.Orders.GiftCertificate.DeliveryByEmail"))
            {
                shipping = new BaseShippingOption(){Name = LocalizationService.GetResource("Core.Orders.GiftCertificate.DeliveryByEmail")};
            }

            if (shipping == null && !string.IsNullOrWhiteSpace(order.ArchivedShippingName))
                shipping = new BaseShippingOption() {Name = order.ArchivedShippingName, Rate = order.ShippingCost};

            if (shipping == null)
                return Json("Доставка не выбрана");
            
            var preOrder = new PreOrder()
            {
                Items = order.OrderItems.Select(x => new PreOrderItem(x)).ToList(),
                CountryDest = order.OrderCustomer.Country,
                CityDest = order.OrderCustomer.City,
                ZipDest = order.OrderCustomer.Zip,
                ShippingOption = shipping,
                Currency = order.OrderCurrency
            };

            var manager = new PaymentManager(preOrder, null);
            var options = manager.GetOptions();

            BasePaymentOption selectedPayment = null;
            if (options != null)
                selectedPayment = options.FirstOrDefault(x => x.Id == order.PaymentMethodId) ?? options.FirstOrDefault();

            return Json(new { selectPayment = selectedPayment, option = options });
        }

        [HttpPost]
        public JsonResult BillingPaymentPost(BasePaymentOption payment, int orderId)
        {
            var order = OrderService.GetOrder(orderId);

            if (order == null || order.Payed || order.OrderStatus.IsCanceled)
                return Json(null);

            var paymentMethod = PaymentService.GetPaymentMethod(payment.Id);
            if (paymentMethod == null)
                return Json(null);

            order.PaymentMethodId = paymentMethod.PaymentMethodId;
            order.ArchivedPaymentName = paymentMethod.Name;
            order.PaymentCost = paymentMethod.GetExtracharge(order);

            order.PaymentDetails = payment.GetDetails();

            OrderService.UpdatePaymentDetails(order.OrderID, order.PaymentDetails);
            OrderService.UpdateOrderMain(order);

            OrderService.RefreshTotal(order);
            order = OrderService.GetOrder(order.OrderID);

            var script = SettingsCheckout.ManagerConfirmed && !order.ManagerConfirmed
                ? string.Empty
                : OrderService.ProcessOrder(order, PageWithPaymentButton.orderconfirmation);

            return Json(new { script, proceedToPayment = SettingsCheckout.ProceedToPayment });
        }

        public JsonResult BillingCartJson(int orderId)
        {
            var order = OrderService.GetOrder(orderId);
            if (order == null)
                return Json("error");

            var model = new BillingCartHandler().Get(order);

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Check order

        [ChildActionOnly]
        public ActionResult CheckOrderBlock()
        {
            if (!SettingsDesign.CheckOrderVisibility)
                return new EmptyResult();

            return PartialView();
        }

        [HttpGet]
        public JsonResult CheckOrder(string ordernumber)
        {
            if (string.IsNullOrEmpty(ordernumber))
                return Json(null);

            var statusInfo = OrderService.GetStatusInfo(ordernumber);
            return Json(statusInfo ?? new StatusInfo { StatusComment = "", StatusName = T("Checkout.CheckOrder.StatusCommentNotFound") });
        }

        #endregion
    }
}