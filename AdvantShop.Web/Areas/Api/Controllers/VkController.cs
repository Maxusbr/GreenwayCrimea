using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AdvantShop.Areas.Api.Attributes;
using AdvantShop.Areas.Api.Models.Vk;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.Api;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.IPTelephony;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Core.Services.Vk;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.Repository.Currencies;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Areas.Api.Controllers
{
    public class VkController : BaseApiController
    {
        private readonly VkApiService _apiService = new VkApiService();
        
        [LogRequest, AuthApi, CheckOrigin]
        public JsonResult GetInfo()
        {
            return Json(new VkResponse()
            {
                Data = new {StoreName = SettingsMain.ShopName}
            });
        }

        [LogRequest, AuthApi, CheckOrigin]
        public JsonResult GetCustomerInfo(long? id, string stringId)
        {
            try
            {
                var vkUser = GetVkUser(id, stringId);
                if (vkUser == null)
                {
                    Debug.Log.Error("Api vk. id " + id + " is wrong. vkUser is null");
                    return ErrorVk("");
                }

                var customer = vkUser.CustomerId != Guid.Empty ? CustomerService.GetCustomer(vkUser.CustomerId) : null;

                var model = new GetCustomerInfoModel()
                {
                    Fio = vkUser.LastName + " " + vkUser.FirstName,
                };

                if (customer != null)
                {
                    model.CustomerId = customer.Id.ToString();

                    model.CustomerLink = UrlService.GetUrl("adminv2/customers/edit/" + model.CustomerId);

                    if (customer.Manager != null)
                        model.Manager = customer.Manager.FullName;

                    var orders = OrderService.GetCustomerOrderHistory(customer.Id);
                    if (orders != null && orders.Count > 0)
                    {
                        var lastOrder = orders[0];

                        model.LastOrder = string.Format("№{0} - {1} {2} - {3}", 
                                            lastOrder.OrderNumber, lastOrder.Sum, lastOrder.CurrencySymbol, 
                                            lastOrder.Status);

                        model.LastOrderLink = UrlService.GetUrl("adminv2/orders/edit/" + lastOrder.OrderID);

                        model.TotalOrdersSum = orders.Where(x => x.Payed).Sum(x => x.Sum*x.CurrencyValue).FormatPriceInvariant();
                    }

                    var leads = LeadService.GetLeadsByCustomer(customer.Id);
                    if (leads != null && leads.Count > 0)
                    {
                        var lastLead = leads.OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                        model.LastLead = string.Format("№{0} - {1} {2} - {3}",
                                            lastLead.Id, lastLead.Sum, lastLead.LeadCurrency.CurrencySymbol,
                                            lastLead.DealStatus.Name);

                        model.LastLeadLink = UrlService.GetUrl("adminv2/leads/edit/" + lastLead.Id);
                    }

                    var tasks =
                        TaskService.GetTaskByCustomerId(customer.Id)
                            .Where(x => x.Status == TaskStatus.Open || x.Status == TaskStatus.InProgress)
                            .ToList();
                    if (tasks.Count > 0)
                    {
                        var task = tasks.LastOrDefault();
                        if (task != null)
                        {
                            model.LastOpenTask = task.Name;
                            model.LastOpenTaskLink = UrlService.GetUrl("adminv2/tasks?#?modal=" + task.Id);
                        }
                    }

                    if (customer.StandardPhone != null && customer.StandardPhone != 0)
                    {
                        var call = CallService.GetCalls(customer.StandardPhone.Value).FirstOrDefault();
                        if (call != null)
                            model.LastCall = string.Format("{0} {1}", call.Phone, call.CallDate.ToString("dd-MM-yy hh:mm"));
                    }
                }

                model.NewTaskLink = UrlService.GetUrl("adminv2/tasks#?modalNewTask=true");

                return Json(new VkResponse() {Data = model});
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                ModelState.AddModelError("", ex.Message);
            }

            var errors = new List<string>();

            foreach (var modelState in ViewData.ModelState.Values)
                foreach (var error in modelState.Errors)
                    errors.Add(error.ErrorMessage);
            
            return ErrorVk(String.Join("; ", errors));
        }

        [LogRequest, AuthApi, CheckOrigin]
        public JsonResult CreateCustomer(long? id, string stringId)
        {
            var vkUser = GetVkUser(id, stringId);
            if (vkUser == null)
                return ErrorVk("");

            var customer = GetOrCreateCustomer(vkUser);
            return Json(new VkResponse() { Data = UrlService.GetUrl("adminv2/customers/edit/" + customer.Id) });
        }

        [LogRequest, AuthApi, CheckOrigin]
        public JsonResult CreateOrder(long? id, string stringId)
        {
            var vkUser = GetVkUser(id, stringId);
            if (vkUser == null)
                return ErrorVk("");

            try
            {
                var customer = GetOrCreateCustomer(vkUser);
                var currency = CurrencyService.Currency(SettingsCatalog.DefaultCurrencyIso3);
                var orderSource = OrderSourceService.GetOrderSource(OrderType.Vk);

                var order = new Order
                {
                    OrderDate = DateTime.Now,
                    OrderCustomer = (OrderCustomer) customer,
                    OrderCurrency = (OrderCurrency) currency,
                    OrderStatusId = OrderStatusService.DefaultOrderStatus,
                    OrderSourceId = orderSource.Id,
                    IsDraft = true,
                    IsFromAdminArea = true,
                };
                var orderId = OrderService.AddOrder(order, new OrderChangedBy("Расширение для ВКонтакте"), false);

                return Json(new VkResponse() {Data = UrlService.GetUrl("adminv2/orders/edit/" + orderId)});
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return ErrorVk("");
        }

        [LogRequest, AuthApi, CheckOrigin]
        public JsonResult CreateLead(long? id, string stringId)
        {
            var vkUser = GetVkUser(id, stringId);
            if (vkUser == null)
                return ErrorVk("");

            try
            {
                var customer = GetOrCreateCustomer(vkUser);
                var currency = CurrencyService.Currency(SettingsCatalog.DefaultCurrencyIso3);
                var orderSource = OrderSourceService.GetOrderSource(OrderType.Vk);

                var lead = new Lead()
                {
                    CustomerId = customer.Id,
                    FirstName = customer.FirstName ?? "",
                    LastName = customer.LastName ?? "",
                    Patronymic = customer.Patronymic ?? "",
                    LeadCurrency = currency,
                    OrderSourceId = orderSource.Id,
                    IsFromAdminArea = true,
                    LeadItems = new List<LeadItem>()
                };
                LeadService.AddLead(lead, true);

                return Json(new VkResponse() { Data = UrlService.GetUrl("adminv2/leads/edit/" + lead.Id) });
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return ErrorVk("");
        }


        private VkUser GetVkUser(long? id, string stringId)
        {
            if (id == null && string.IsNullOrWhiteSpace(stringId))
                return null;

            VkUser vkUser = null;

            if (id != null && id != 0)
            {
                vkUser = VkService.GetUser(id.Value) ?? _apiService.GetUsersInfo(new[] {id.Value}).FirstOrDefault();
            }
            else
            {
                var user = _apiService.GetUsersInfo(new[] { stringId }).FirstOrDefault();
                if (user != null)
                    vkUser = VkService.GetUser(user.Id) ?? user;
            }

            return vkUser;
        }


        private Customer GetOrCreateCustomer(VkUser vkUser)
        {
            var customer = vkUser.CustomerId != Guid.Empty ? CustomerService.GetCustomer(vkUser.CustomerId) : null;
            if (customer != null)
                return customer;

            var phone = vkUser.MobilePhone ?? vkUser.HomePhone;

            customer = new Customer(CustomerGroupService.DefaultCustomerGroup)
            {
                FirstName = vkUser.FirstName,
                LastName = vkUser.LastName,
                Phone = phone,
                StandardPhone = !string.IsNullOrEmpty(phone) ? StringHelper.ConvertToStandardPhone(phone) : null,
            };
            CustomerService.InsertNewCustomer(customer);

            // add vk user
            vkUser.CustomerId = customer.Id;
            if (VkService.GetUser(vkUser.Id) == null)
                VkService.AddUser(vkUser);

            return customer;
        }

        private JsonResult ErrorVk(string text)
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            Response.TrySkipIisCustomErrors = true;
            return Json(new ApiResponse("error", text));
        } 
    }
}