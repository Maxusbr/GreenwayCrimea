using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Core.SQL;
using AdvantShop.Customers;
using AdvantShop.Mails;
using AdvantShop.Orders;
using AdvantShop.Payment;
using AdvantShop.Shipping;
using AdvantShop.Trial;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Orders;
using AdvantShop.Web.Admin.Handlers.Orders.Shippings;
using AdvantShop.Web.Admin.Models.Orders;
using AdvantShop.Web.Admin.Models.OrdersEdit;
using AdvantShop.Web.Admin.ViewModels.Orders;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;
using AdvantShop.Saas;
using AdvantShop.Core.Modules;
using AdvantShop.Core.UrlRewriter;

namespace AdvantShop.Web.Admin.Controllers.Orders
{
    [Auth(RoleAction.Orders)]
    public partial class OrdersController : BaseAdminController
    {
        #region Orders List

        public ActionResult Index(OrdersFilterModel filter)
        {
            if (!string.IsNullOrEmpty(filter.Search))
            {
                var order = OrderService.GetOrder(filter.Search.TryParseInt()) ?? OrderService.GetOrderByNumber(filter.Search);
                if (order != null)
                    return RedirectToAction("Edit", new { id = order.OrderID });
            }

            var model = new OrdersViewModel()
            {
                PreFilter = filter.FilterBy,
                EnableMangers = SettingsCheckout.EnableManagersModule &&
                    (!SaasDataService.IsSaasEnabled || (SaasDataService.IsSaasEnabled && SaasDataService.CurrentSaasData.HaveCrm))
            };

            SetMetaInformation(T("Admin.Orders.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.OrdersCtrl);

            return View("List", model);
        }

        /// <summary>
        /// Orders Paging
        /// </summary>
        public JsonResult GetOrders(OrdersFilterModel model)
        {
            return Json(new GetOrdersHandler(model).Execute());
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteOrder(int orderId)
        {
            OrderService.DeleteOrder(orderId);
            return JsonOk();
        }

        #region Commands

        private void Command(OrdersFilterModel command, Action<int, OrdersFilterModel> func)
        {
            if (command.SelectMode == SelectModeCommand.None)
            {
                foreach (var id in command.Ids)
                    func(id, command);
            }
            else
            {
                var ids = new GetOrdersHandler(command).GetItemsIds("[Order].OrderID");
                foreach (int id in ids)
                {
                    if (command.Ids == null || !command.Ids.Contains(id))
                        func(id, command);
                }
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteOrders(OrdersFilterModel command)
        {
            Command(command, (id, c) => OrderService.DeleteOrder(id));
            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult MarkPaid(OrdersFilterModel command)
        {
            Command(command, (id, c) => OrderService.PayOrder(id, true));
            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult MarkNotPaid(OrdersFilterModel command)
        {
            Command(command, (id, c) => OrderService.PayOrder(id, false));
            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult ChangeStatus(OrdersFilterModel command, int newOrderStatusId, string statusBasis)
        {
            Command(command, (id, c) => OrderStatusService.ChangeOrderStatus(id, newOrderStatusId, statusBasis));
            return JsonOk();
        }

        public JsonResult GetOrderStatuses()
        {
            var statuses = OrderStatusService.GetOrderStatuses();

            return Json(statuses.Select(x => new { label = x.StatusName, value = x.StatusID.ToString(), }));
        }

        public JsonResult GetOrderPaymentMethods()
        {
            var methods = SQLDataAccess.Query<string>("SELECT distinct Name FROM [Order].[PaymentMethod]").ToList();

            return Json(methods.Select(method => new { label = method, value = method }));
        }

        public JsonResult GetOrderShippingMethods()
        {
            var methods = OrderService.GetShippingMethodNamesFromOrder();

            return Json(methods.Select(method => new { label = method, value = method }));
        }

        public JsonResult GetManagers()
        {
            var managers = ManagerService.GetCustomerManagersList();

            return Json(managers.Select(x => new { label = string.Format("{0} {1}", x.FirstName, x.LastName), value = x.Id }));
        }

        public JsonResult GetOrderSources()
        {
            var sources = OrderSourceService.GetOrderSources();

            return Json(sources.Select(x => new { label = x.Name, value = x.Id }));
        }

        #endregion

        #endregion

        #region Add | Edit Order

        public ActionResult Add(string customerId, string phone)
        {
            var model = new GetOrder(customerId, phone).Execute();

            SetMetaInformation(T("Admin.Orders.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.OrderCtrl);

            return View("AddEdit", model);
        }

        public ActionResult Edit(int id)
        {
            var model = new GetOrder(true, id).Execute();
            if (model == null)
                return RedirectToAction("Index");

            SetMetaInformation(T(!model.Order.IsDraft ? "Admin.Orders.AddEdit.OrderTitle" : "Admin.Orders.AddEdit.OrderDraftTitle", model.Order.Number));
            SetNgController(NgControllers.NgControllersTypes.OrderCtrl);

            return View("AddEdit", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(OrderModel model)
        {
            if (ModelState.IsValid)
            {
                var result = new UpdateOrder(model).Execute();
                if (result)
                {
                    ShowMessage(NotifyType.Success, T("Admin.ChangesSuccessfullySaved"));
                    return RedirectToAction("Edit", new { id = model.OrderId });
                }
            }

            ShowErrorMessages();

            SetMetaInformation(T("Admin.Orders.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.OrderCtrl);

            return RedirectToAction("Edit", new { id = model.OrderId });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Add(OrderModel model)
        {
            if (ModelState.IsValid)
            {
                var orderId = 0;
                var order = model.OrderId != 0
                                ? OrderService.GetOrder(model.OrderId)
                                : null;
                if (order == null)
                {
                    var result = new SaveOrderDraft(model.Order).Execute();
                    if (result != null)
                        orderId = result.OrderId;
                }
                else
                {
                    model.Order.IsDraft = false;
                    var result = new UpdateOrder(model).Execute();

                    if (result)
                        orderId = order.OrderID;
                }

                if (orderId != 0)
                {
                    ShowMessage(NotifyType.Success, T("Admin.ChangesSuccessfullySaved"));
                    return RedirectToAction("Edit", new { id = orderId });
                }
            }

            ShowErrorMessages();

            SetMetaInformation(T("Admin.Orders.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.OrderCtrl);

            return View("AddEdit", model);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SaveDraft(OrderDraftModel model)
        {
            var result = new SaveOrderDraft(model).Execute();
            return Json(new { result = true, orderId = result.OrderId, customerId = result.CustomerId });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddOrderFromCart(Guid customerId)
        {
            return ProcessJsonResult(new AddOrderFromCart(customerId));
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UpdateOrderBonusCard(int orderId)
        {
            return ProcessJsonResult(new UpdateOrderTotal(orderId, null));
        }


        #region OrderItems

        [HttpGet]
        public JsonResult GetOrderItems(int orderId, string sorting, string sortingType)
        {
            var orderItems = new GetOrderItems(orderId, sorting, sortingType).Execute();
            return Json(new { DataItems = orderItems });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddOrderItems(int orderId, List<int> offerIds)
        {
            var order = OrderService.GetOrder(orderId);

            if (order == null || offerIds == null || offerIds.Count == 0 || !OrderService.CheckAccess(order))
                return Json(new { result = false });

            var saveChanges = new AddOrderItems(order, offerIds).Execute();

            var result = saveChanges && new UpdateOrderItems(order).Execute();

            return Json(new { result });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UpdateOrderItem(OrderItemModel model)
        {
            var order = OrderService.GetOrder(model.OrderId);
            if (order == null)
                return JsonError();

            var orderItem = order.OrderItems.Find(x => x.OrderItemID == model.OrderItemId);
            if (orderItem == null)
                return JsonError();

            orderItem.Price = model.Price;
            orderItem.Amount = model.Amount;

            var result = new UpdateOrderItems(order).Execute();

            return Json(new { result });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteOrderItem(int orderid, int orderItemId)
        {
            var order = OrderService.GetOrder(orderid);
            if (order == null)
                return JsonError();

            var index = order.OrderItems.FindIndex(x => x.OrderItemID == orderItemId);
            if (index == -1)
                return JsonError();

            order.OrderItems.RemoveAt(index);

            var result = new UpdateOrderItems(order).Execute();

            return Json(new { result });
        }

        public JsonResult GetOrderItemsSummary(int orderId)
        {
            return Json(new GetOrderItemsSummary(orderId).Execute());
        }


        #endregion

        #region OrderCertificates

        public JsonResult GetOrderCertificates(int orderId)
        {
            var items = GiftCertificateService.GetOrderCertificates(orderId).Select(x => new
            {
                x.CertificateId,
                x.CertificateCode,
                x.Sum,
                x.ApplyOrderNumber
            });
            return Json(new { DataItems = items });
        }

        #endregion

        #region Shippings

        [HttpGet]
        public JsonResult GetShippings(int id, string country, string city, string region, string zip)
        {
            return Json(new GetShippings(id, country, city, region, zip).Execute());
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult CalculateShipping(int id, string country, string city, string region, string zip, BaseShippingOption shipping)
        {
            var model = new GetShippings(id, country, city, region, zip, shipping, false).Execute();

            var option = model.Shippings != null ? model.Shippings.FirstOrDefault(x => x.Id == shipping.Id) : null;

            if (option != null)
                option.Update(shipping);

            if (option == null && shipping.MethodId == 0)
                option = shipping;

            return Json(new { selectShipping = option });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SaveShipping(int id, string country, string city, string region, BaseShippingOption shipping)
        {
            var order = OrderService.GetOrder(id);
            if (order == null || shipping == null)
                return JsonError();

            new SaveShipping(order, country, city, region, shipping).Execute();

            return JsonOk();
        }

        [HttpGet]
        public JsonResult GetDeliveryTime(int id)
        {
            var order = OrderService.GetOrder(id);
            if (order == null)
                return JsonError();

            return Json(new
            {
                DeliveryDate = order.DeliveryDate != null ? order.DeliveryDate.Value.ToString("dd.MM.yyyy") : "",
                DeliveryTime = order.DeliveryTime
            });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SaveDeliveryTime(int id, string deliveryDate, string deliveryTime)
        {
            var order = OrderService.GetOrder(id);
            if (order == null)
                return JsonError();

            order.DeliveryDate = !string.IsNullOrWhiteSpace(deliveryDate) ? deliveryDate.TryParseDateTime() : default(DateTime?);
            order.DeliveryTime = deliveryTime;

            var trackChanges = !order.IsDraft;

            OrderService.UpdateOrderMain(order, updateModules: false, trackChanges: trackChanges);

            return JsonOk();
        }

        #endregion

        #region Payments

        [HttpGet]
        public JsonResult GetPayments(int orderId, string country, string city, string region)
        {
            return Json(new { payments = new GetPayments(orderId, country, city, region).Execute() });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SavePayment(int orderId, string country, string city, string region, BasePaymentOption payment)
        {
            var order = OrderService.GetOrder(orderId);
            if (order == null || payment == null)
                return JsonError();

            new SavePayment(order, country, city, region, payment).Execute();

            return JsonOk();
        }

        #endregion

        #region Discount

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult ChangeDiscount(int orderId, float orderDiscount, bool isValue)
        {
            if (!isValue && (orderDiscount < 0 || orderDiscount > 100))
                return JsonError();

            var order = OrderService.GetOrder(orderId);
            if (order == null)
                return JsonError();

            if (!isValue)
            {
                order.OrderDiscount = orderDiscount;
                order.OrderDiscountValue = 0;
            }
            else
            {
                order.OrderDiscount = 0;
                order.OrderDiscountValue = orderDiscount;
            }

            new UpdateOrderTotal(order).Execute();

            return JsonOk();
        }

        #endregion

        #region Bonuses

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UseBonuses(int orderId, float bonusesAmount)
        {
            return ProcessJsonResult(new UpdateOrderTotal(orderId, bonusesAmount));
        }

        #endregion

        #region Status

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult ChangeOrderStatus(int orderId, int statusId, string basis)
        {
            var order = OrderService.GetOrder(orderId);
            var status = OrderStatusService.GetOrderStatus(statusId);

            if (order != null && status != null && OrderService.CheckAccess(order))
            {
                if(order.OrderStatusId == statusId)
                {
                    return JsonError();
                }

                OrderStatusService.ChangeOrderStatus(orderId, statusId, basis);
                TrialService.TrackEvent(TrialEvents.ChangeOrderStatus, "");

                return Json(new
                {
                    result = true,
                    color = status.Color,
                    isNotifyUserEmail = !status.Hidden && order.OrderCustomer.Email.IsNotEmpty(),
                    isNotifyUserSms = !status.Hidden && order.OrderCustomer.Phone.IsNotEmpty() &&
                             ModulesExecuter.SendNotificationsHasTemplatesOnChangeStatus(status.StatusID)
                });
            }

            return JsonError();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult NotifyStatusChanged(int orderId)
        {
            return Json(new { result = new NotifyStatusChanged(orderId).Exectute() });
        }

        #endregion

        #region Paied

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SetPaied(int orderId, bool paid)
        {
            var order = OrderService.GetOrder(orderId);
            if (order == null || !OrderService.CheckAccess(order))
                return JsonError();

            OrderService.PayOrder(orderId, paid, trackChanges: !order.IsDraft);

            return JsonOk();
        }

        #endregion

        #region Save status and admin comments, tracknumber

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SaveOrderInfo(int orderId, string statusComment, string adminOrderComment, string trackNumber)
        {
            var order = OrderService.GetOrder(orderId);
            if (order == null || !OrderService.CheckAccess(order))
                return JsonError();

            order.StatusComment = statusComment.DefaultOrEmpty();
            order.AdminOrderComment = adminOrderComment.DefaultOrEmpty();
            order.TrackNumber = trackNumber.DefaultOrEmpty();

            OrderService.UpdateOrderMain(order);

            return JsonOk();
        }

        #endregion

        #region Status History

        [HttpGet]
        public JsonResult GetOrderStatusHistory(int orderId)
        {
            var items =
                OrderStatusService.GetOrderStatusHistory(orderId).OrderByDescending(item => item.Date).Select(x => new
                {
                    Date = Localization.Culture.ConvertDate(x.Date),
                    x.PreviousStatus,
                    x.NewStatus,
                    x.CustomerName,
                    x.Basis
                });
            return Json(new { DataItems = items });
        }

        #endregion

        #region Order History

        [HttpGet]
        public JsonResult GetOrderHistory(int orderId)
        {
            var items =
                OrderHistoryService.GetList(orderId).Select(x => new
                {
                    ModificationTime = x.ModificationTime,
                    ModificationTimeFormatted = Localization.Culture.ConvertDateWithoutSeconds(x.ModificationTime),
                    x.Parameter,
                    x.ParameterDescription,
                    x.OldValue,
                    x.NewValue,
                    x.ManagerId,
                    x.ManagerName,
                    IsEmployee = x.CustomerRole == Role.Administrator || x.CustomerRole == Role.Moderator
                });
            return Json(new { DataItems = items });
        }

        #endregion

        [ChildActionOnly]
        public ActionResult ClientInfo(OrderModel orderModel)
        {
            if (!orderModel.IsEditMode)
                return new EmptyResult();

            var model = new GetClientInfo(orderModel).Execute();
            return PartialView("_ClientInfo", model);
        }

        #endregion

        #region Send Billing Link

        public JsonResult GetBillingLink(GetBillingLinkModel model)
        {
            if (ModelState.IsValid)
            {
                var order = model.Order;
                var hash = OrderService.GetBillingLinkHash(order);
                var billingLink = UrlService.GetUrl("checkout/billing?number=" + order.Number + "&hash=" + hash);

                return JsonOk(billingLink);
            }
            return JsonError();
        }

        public JsonResult GetBillingLinkMailTemplate(GetBillingLinkMailModel model)
        {
            if (ModelState.IsValid)
            {
                var order = model.Order;
                var hash = OrderService.GetBillingLinkHash(order);
                var billingLink = UrlService.GetUrl("checkout/billing?number=" + order.Number + "&hash=" + hash);

                var orderTable =
                    OrderService.GenerateOrderItemsHtml(order.OrderItems, order.OrderCurrency,
                                                        order.OrderItems.Sum(x => x.Price * x.Amount),
                                                        order.OrderDiscount, order.OrderDiscountValue, order.Coupon,
                                                        order.Certificate, order.TotalDiscount, order.ShippingCost,
                                                        order.PaymentCost, order.TaxCost, order.BonusCost, 0);

                var mailTemplate = new BillingLinkMailTemplate(order.OrderID.ToString(), order.Number,
                                                                order.OrderCustomer.FirstName, OrderService.GenerateCustomerContactsHtml(order.OrderCustomer),
                                                                hash, "", orderTable);
                mailTemplate.BuildMail();

                return Json(new { result = true, link = billingLink, subject = mailTemplate.Subject, text = mailTemplate.Body });
            }
            return JsonError();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SendBillingLink(SendBillingLinkMailModel model)
        {
            if (ModelState.IsValid)
            {
                var order = model.Order;

                SendMail.SendMailNow(order.OrderCustomer.CustomerID, order.OrderCustomer.Email, model.Subject, model.Text, true);
                SendMail.SendMailNow(Guid.Empty, SettingsMail.EmailForOrders, model.Subject, model.Text, true, order.OrderCustomer.Email);

                return Json(new { result = true, message = "Ссылка на оплату отправлена покупателю и администратору" });
            }
            return JsonError();
        }

        #endregion

        #region Shippings

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult CreateYandexDeliveryOrder(int orderId)
        {
            var model = new CreateYandexDeliveryOrder(orderId).Execute();
            return Json(new { result = model.Result, error = model.Error, message = model.Message });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult CreateCheckoutRuOrder(int orderId)
        {
            var model = new CreateCheckoutRuOrder(orderId).Execute();
            return Json(new { result = model.Result, error = model.Error, message = model.Message });
        }

        #region Sdek

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult CreateSdekOrder(int orderId)
        {
            var model = new CreateSdekOrder(orderId).Execute();
            return Json(new { result = model.Result, error = model.Error, message = model.Message });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SdekCallCustomer(int orderId)
        {
            var model = new SdekCallCustomer(orderId).Execute();
            return Json(new { result = model.Result, error = model.Error, message = model.Message });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SdekDeleteOrder(int orderId)
        {
            var model = new SdekDeleteOrder(orderId).Execute();
            return Json(new { result = model.Result, error = model.Error, message = model.Message });
        }

        public ActionResult SdekOrderPrintForm(int orderId)
        {
            var result = new SdekOrderPrintForm(orderId).Execute();

            if (result == null)
                return Content("");

            return File(result.Item1, "application/octet-stream", result.Item2);
        }


        public ActionResult SdekOrderReportStatus(int orderId)
        {
            var result = new SdekOrderReportStatus(orderId).Execute();

            if (result == null)
                return Content("");

            return File(result.Item1, "application/octet-stream", result.Item2);
        }

        public ActionResult SdekReportOrderInfo(int orderId)
        {
            var result = new SdekReportOrderInfo(orderId).Execute();

            if (result == null)
                return Content("");

            return File(result.Item1, "application/octet-stream", result.Item2);
        }

        #endregion

        #region Boxberry

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult CreateBoxberryOrder(int orderId)
        {
            var model = new BoxberryCreateOrder(orderId).Execute();
            return Json(new { result = model.Result, error = model.Error, message = model.Message });
        }


        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteBoxberryOrder(int orderId)
        {
            var model = new BoxberryDeleteOrder(orderId).Execute();
            return Json(new { result = model.Result, error = model.Error, message = model.Message });
        }

        #endregion

        #region Grastin

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult GrastinSendRequestForMark(int orderId)
        {
            if (ModelState.IsValid)
            {
                var handler = new GrastinSendRequestForMark(orderId);

                var result = handler.Execute();
                if (!string.IsNullOrEmpty(result))
                    return JsonOk(new { FileName = System.IO.Path.GetFileName(result) }, T("Admin.Orders.MarkingReceived"));
                else if (handler.Errors != null)
                    return JsonError(handler.Errors.ToArray());
            }
            return JsonError();
        }

        public ActionResult GrastinOrderPrintMark(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || !System.IO.File.Exists(FilePath.FoldersHelper.GetPathAbsolut(FilePath.FolderType.PriceTemp) + fileName))
                return Content("");

            return File(FilePath.FoldersHelper.GetPathAbsolut(FilePath.FolderType.PriceTemp) + fileName, "application/octet-stream", fileName);
        }

        #endregion
        
        #region DDelivery

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult CreateDDeliveryOrder(int orderId)
        {
            var model = new DDeliveryCreateOrder(orderId).Execute();
            return Json(new { result = model.Result, error = model.Error, message = model.Message });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult CanselDDeliveryOrder(int orderId)
        {
            var model = new DDeliveryCanselOrder(orderId).Execute();
            return Json(new { result = model.Result, error = model.Error, message = model.Message });
        }
                
        public ActionResult DDeliveryOrderInfo(int orderId)
        {
            var result = new DDeliveryOrderInfo(orderId).Execute();

            if (result == null)
                return Content("");

            return File(result.Item1, "application/octet-stream", result.Item2);
        }

        #endregion

        #endregion
    }
}
