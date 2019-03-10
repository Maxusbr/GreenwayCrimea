using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Modules.Interfaces;
using AdvantShop.Core.Services.Bonuses;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.MyAccount;
using AdvantShop.Customers;
using AdvantShop.Handlers.MyAccount;
using AdvantShop.Helpers;
using AdvantShop.Mails;
using AdvantShop.Models.MyAccount;
using AdvantShop.Orders;
using AdvantShop.Payment;
using AdvantShop.Repository;
using AdvantShop.Repository.Currencies;
using AdvantShop.Security;
using AdvantShop.ViewModel.MyAccount;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Diagnostics;

namespace AdvantShop.Controllers
{
    public partial class MyAccountController : BaseClientController
    {
        public ActionResult Index()
        {
            var customer = CustomerContext.CurrentCustomer;

            if (!customer.RegistredUser)
                return RedirectToRoute("Home");

            var model = new MyAccountViewModel()
            {
                DisplayBonuses = BonusSystem.IsActive,
                DisplayChangeEmail = customer.EMail.Contains("@temp"),
                Tabs = new List<MyAccountTab>()
            };

            if (model.DisplayBonuses)
            {
                var bonusCard = BonusSystemService.GetCard(customer.Id);
                if (bonusCard != null)
                    model.BonusesAmount = bonusCard.BonusesTotalAmount;
            }

            foreach (var type in AttachedModules.GetModules<IMyAccountTabs>())
            {
                var instance = (IMyAccountTabs)Activator.CreateInstance(type, null);
                if (instance != null)
                {
                    model.Tabs.AddRange(instance.GetMyAccountTabs());
                }
            }

            SetMetaInformation(T("MyAccount.Index.MyAccountTitle"));
            SetNgController(NgControllers.NgControllersTypes.MyAccountCtrl);

            ModulesExecuter.ViewMyAccount(customer);

            return View(model);
        }

        #region Customer contacts

        public JsonResult GetCustomerContacts()
        {
            if (!CustomerContext.CurrentCustomer.RegistredUser)
                return Json("");

            var customerContacts =
                from item in CustomerContext.CurrentCustomer.Contacts
                select new
                {
                    item.ContactId,
                    item.Country,
                    item.City,
                    item.Name,
                    item.CountryId,
                    item.RegionId,
                    item.Region,
                    item.Zip,

                    item.Street,
                    item.House,
                    item.Apartment,
                    item.Structure,
                    item.Entrance,
                    item.Floor,

                    IsShowFullAddress = SettingsCheckout.IsShowFullAddress,
                    AggregatedAddress = new[] { item.Name, item.Zip, item.Country, item.City, item.Region, item.Street, item.House, item.Structure, item.Entrance, item.Floor, item.Apartment }.
                                        Where(str => str.IsNotEmpty()).AggregateString(", ")

                };

            return Json(customerContacts.ToList());
        }

        public JsonResult GetFieldsForCustomerContacts()
        {
            return Json(new
            {
                SettingsCheckout.IsShowCountry,
                SettingsCheckout.IsRequiredCountry,
                SettingsCheckout.IsShowState,
                SettingsCheckout.IsRequiredState,
                SettingsCheckout.IsShowCity,
                SettingsCheckout.IsRequiredCity,
                SettingsCheckout.IsShowAddress,
                SettingsCheckout.IsRequiredAddress,
                SettingsCheckout.IsShowZip,
                SettingsCheckout.IsRequiredZip
            });
        }

        public JsonResult AddUpdateCustomerContact(CustomerAccountModel account)
        {
            var contact = new AddUpdateContactHandler().Execute(account);
            return Json(contact);
        }

        public JsonResult DeleteCustomerContact(string contactId)
        {
            if (!CustomerContext.CurrentCustomer.RegistredUser)
                return Json(false);

            var id = contactId.TryParseGuid();
            if (id != Guid.Empty && CustomerContext.CurrentCustomer.Contacts.Any(contact => contact.ContactId == id))
            {
                CustomerService.DeleteContact(id);
            }
            return Json(true);
        }

        #endregion

        #region Order history

        public JsonResult GetOrderDetails(string ordernumber)
        {
            if (!CustomerContext.CurrentCustomer.RegistredUser)
                return Json(null);

            var order = OrderService.GetOrderByNumber(ordernumber);

            if (order.OrderCustomer == null || order.OrderCustomer.CustomerID != CustomerContext.CurrentCustomer.Id)
                return Json(null);

            var orderDetails = new GetOrderDetailsHandler(order).Get(); // TODO: return class
            if (orderDetails == null)
                return Json(null);

            return Json(orderDetails);
        }

        public JsonResult GetCustomerOrderHistory()
        {
            if (!CustomerContext.CurrentCustomer.RegistredUser)
                return Json(null);

            var orders = OrderService.GetCustomerOrderHistory(CustomerContext.CurrentCustomer.Id);

            var customerOrders = from item in orders
                                 select new
                                 {
                                     item.ArchivedPaymentName,
                                     Status = OrderStatusService.GetOrderStatus(item.StatusID).Hidden ? item.PreviousStatus : item.Status,
                                     item.ShippingMethodName,
                                     OrderDate = item.OrderDate.ToString(SettingsMain.ShortDateFormat),
                                     OrderTime = item.OrderDate.ToString("HH:mm"),
                                     Sum = PriceFormatService.FormatPrice(item.Sum, item.CurrencyValue, item.CurrencySymbol, item.CurrencyCode, item.IsCodeBefore, null),
                                     item.OrderNumber,
                                     item.Payed
                                 };
            var totalPrice = orders.Where(item => item.Payed).Sum(item => item.Sum * item.CurrencyValue);
            totalPrice = totalPrice / CurrencyService.CurrentCurrency.Rate;

            return Json(new
            {
                Orders = customerOrders,
                TotalSum = PriceFormatService.FormatPrice(totalPrice, CurrencyService.CurrentCurrency)
            });
        }

        public JsonResult CancelOrder(string ordernumber)
        {
            if (!CustomerContext.CurrentCustomer.RegistredUser || ordernumber.IsNullOrEmpty())
                return Json(null);

            var order = OrderService.GetOrderByNumber(ordernumber);
            var customer = OrderService.GetOrderCustomer(ordernumber);

            if (order == null || customer == null || customer.CustomerID != CustomerContext.CurrentCustomer.Id)
                return Json(null);

            OrderService.CancelOrder(order.OrderID);

            order = OrderService.GetOrder(order.OrderID);

            var orderItemsHtml = OrderService.GenerateOrderItemsHtml(order.OrderItems, order.OrderCurrency,
                                                           order.Sum,
                                                           order.OrderDiscount, order.OrderDiscountValue,
                                                           order.Coupon, order.Certificate,
                                                           order.TotalDiscount,
                                                           order.ShippingCost, order.PaymentCost,
                                                           order.TaxCost,
                                                           order.BonusCost,
                                                           0);

            var mailTemplate = new OrderStatusMailTemplate(order.OrderStatus.StatusName,
                                                           order.StatusComment.Replace("\r\n", "<br />"),
                                                           order.Number, orderItemsHtml,!string.IsNullOrEmpty(order.TrackNumber) ? order.TrackNumber : string.Empty);
            mailTemplate.BuildMail();
            SendMail.SendMailNow(order.OrderCustomer.CustomerID, order.OrderCustomer.Email, mailTemplate.Subject, mailTemplate.Body, true);
            SendMail.SendMailNow(Guid.Empty, SettingsMail.EmailForOrders, mailTemplate.Subject, mailTemplate.Body, true);


            return Json(null);
        }

        public JsonResult ChangePaymentMethod(string paymentId, string orderNumber)
        {
            if (!CustomerContext.CurrentCustomer.RegistredUser && paymentId.IsNullOrEmpty() && orderNumber.IsNullOrEmpty())
                return Json(null);

            var order = OrderService.GetOrderByNumber(orderNumber);
            if (order == null)
                return Json(null);

            var payment = PaymentService.GetPaymentMethod(Convert.ToInt32(paymentId));
            if (payment == null)
                return Json(null);

            order.PaymentMethodId = payment.PaymentMethodId;
            order.ArchivedPaymentName = payment.Name;
            order.PaymentCost = payment.GetExtracharge(order);

            OrderService.UpdateOrderMain(order);
            OrderService.RefreshTotal(order);

            return Json(true);
        }

        public JsonResult ChangeOrderComment(string orderNumber, string customerComment)
        {
            if (orderNumber.IsNullOrEmpty())
                return Json(null);

            var order = OrderService.GetOrderByNumber(orderNumber);
            if (order == null)
                return Json(null);

            try
            {
                var changeUserCommentTemplate = new ChangeUserCommentTemplate(order.OrderID.ToString(), customerComment, order.Number);
                changeUserCommentTemplate.BuildMail();
                SendMail.SendMailNow(Guid.Empty, SettingsMail.EmailForOrders, changeUserCommentTemplate.Subject, changeUserCommentTemplate.Body, true);
            }
            catch(Exception ex)
            {
                Debug.Log.Error(ex.Message, ex);
            }

            order.CustomerComment = customerComment;

            OrderService.UpdateOrderMain(order);
            OrderService.RefreshTotal(order);

            return Json(true);
        }

        #endregion

        public JsonResult CheckEmailBusy(string email)
        {
            return Json(ValidationHelper.IsValidEmail(email) && !CustomerService.ExistsEmail(email));
        }

        [HttpPost]
        public JsonResult UpdateCustomerEmail(string email)
        {
            if (ValidationHelper.IsValidEmail(email) && !CustomerService.ExistsEmail(email) &&
                CustomerContext.CurrentCustomer.EMail.Contains("@temp"))
            {
                CustomerService.UpdateCustomerEmail(CustomerContext.CustomerId, email);
                AuthorizeService.SignIn(email, CustomerContext.CurrentCustomer.Password, true, true);

                return Json(true);
            }

            return Json(false);
        }

        [ChildActionOnly]
        public ActionResult CommonInfo()
        {
            var model = new CommonInfoHandler().Get();
            return PartialView(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult CommonInfo(CommonInfoViewModel infoModel)
        {
            var customer = CustomerContext.CurrentCustomer;
            customer.FirstName = HttpUtility.HtmlEncode(infoModel.FirstName);
            customer.LastName = HttpUtility.HtmlEncode(infoModel.LastName);
            customer.Patronymic = HttpUtility.HtmlEncode(infoModel.Patronymic);
            customer.Phone = HttpUtility.HtmlEncode(infoModel.Phone);
            customer.StandardPhone = StringHelper.ConvertToStandardPhone(infoModel.Phone);
            customer.SubscribedForNews = infoModel.SubscribedForNews;

            if (infoModel.CustomerFields != null)
            {
                foreach (var customerField in infoModel.CustomerFields)
                {
                    CustomerFieldService.AddUpdateMap(customer.Id, customerField.Id, customerField.Value ?? "");
                }
            }

            var isCustomerUpdated = CustomerService.UpdateCustomer(customer);

            if (isCustomerUpdated)
                ShowMessage(NotifyType.Notice, T("MyAccount.CommonInfo.DataSuccessSaved"));
            else
                ShowMessage(NotifyType.Error, T("MyAccount.CommonInfo.ErrorSavingData"));

            var model = new CommonInfoHandler().Get();
            if (!isCustomerUpdated)
            {
                model.FirstName = customer.FirstName;
                model.LastName = customer.LastName;
                model.Patronymic = customer.Patronymic;
                model.Phone = customer.Phone;
                model.SubscribedForNews = customer.SubscribedForNews;
            }

            return RedirectToAction("Index");
        }

        [ChildActionOnly]
        public ActionResult AddressBook()
        {
            var selectedCountryId = IpZoneContext.CurrentZone.CountryId;

            var model = new AddressBookViewModel()
            {
                Countries =
                    CountryService.GetAllCountries()
                        .Select(x => new SelectListItem() { Text = x.Name, Value = x.CountryId.ToString(), Selected = x.CountryId == selectedCountryId })
                        .ToList()
            };

            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult ChangePassword()
        {
            return PartialView(new ChangePasswordViewModel());
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (model == null)
                return RedirectToAction("Index");

            if (model.NewPassword.Length < 6)
            {
                ShowMessage(NotifyType.Error, T("User.Registration.PasswordLenght"));
                return new RedirectResult(Url.Action("Index") + "#?tab=changepassword");
            }

            if (string.IsNullOrWhiteSpace(model.NewPassword) || string.IsNullOrWhiteSpace(model.NewPasswordConfirm) || model.NewPassword != model.NewPasswordConfirm)
            {
                ShowMessage(NotifyType.Error, T("User.Registration.ErrorPasswordNotMatch"));
                return new RedirectResult(Url.Action("Index") + "#?tab=changepassword");
            }

            CustomerService.ChangePassword(CustomerContext.CurrentCustomer.Id, model.NewPassword, false);
            AuthorizeService.SignIn(CustomerContext.CurrentCustomer.EMail, model.NewPassword, false, true);

            ShowMessage(NotifyType.Notice, T("MyAccount.ChangePassword.PasswordSaved"));

            return RedirectToAction("Index");
        }

        [ChildActionOnly]
        public ActionResult BonusCard()
        {
            var model = new MyAccountBonusSystemHandler(CustomerContext.CurrentCustomer).Get();
            return PartialView("BonusCard", model);
        }
    }
}