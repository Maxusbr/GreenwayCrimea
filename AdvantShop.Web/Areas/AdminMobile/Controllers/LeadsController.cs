using System;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Areas.AdminMobile.Models.Leads;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Crm.DealStatuses;
using AdvantShop.Core.SQL2;
using AdvantShop.Customers;
using AdvantShop.Orders;
using AdvantShop.Saas;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Repository.Currencies;

namespace AdvantShop.Areas.AdminMobile.Controllers
{
    [SaasFeature(ESaasProperty.HaveCrm)]
    [Auth(RoleAction.Crm)]
    public class LeadsController : BaseAdminMobileController
    {
        // GET: adminmobile/leads
        public ActionResult Index()
        {
            var model = new LeadsViewModel();
            model.Statuses.Add(new SelectListItem {Text = T("AdminMobile.Leads.AllLeads"), Value = ""});
            foreach (var status in DealStatusService.GetList())
            {
                model.Statuses.Add(new SelectListItem() {Text = status.Name, Value = status.Id.ToString()});
            }
            
            SetMetaInformation(T("AdminMobile.Leads.Leads"));
            return View(model);
        }

        public JsonResult GetLeads(int page, string status)
        {
            if (page == 0)
                page = 1;

            var paging = new SqlPaging(page, 10);
            paging.Select(
                "[Lead].Id",
                //"Name",
                "[LeadCustomer].FirstName as CustomerFirstName",
                "[LeadCustomer].LastName as CustomerLastName",
                "[LeadCustomer].Patronymic as CustomerPatronymic",
                
                "[DealStatus].[Name]".AsSqlField("Status"),
                "[Lead].CreatedDate"
                );

            paging.From("[Order].[Lead]");
            paging.Left_Join("[Customers].[Customer] as LeadCustomer on [Lead].[CustomerId] = [LeadCustomer].[CustomerId]");
            paging.Left_Join("[CRM].[DealStatus] ON [DealStatus].[Id] = [Lead].[DealStatusId]");
            
            var customer = CustomerContext.CurrentCustomer;
            if (customer.IsModerator)
            {
                var manager = ManagerService.GetManager(customer.Id);
                if (manager != null && manager.Active)
                {
                    if (SettingsManager.ManagersLeadConstraint == ManagersLeadConstraint.Assigned)
                    {
                        paging.Where("[Lead].ManagerId = {0}", manager.ManagerId);
                    }
                    else if (SettingsManager.ManagersLeadConstraint == ManagersLeadConstraint.AssignedAndFree)
                    {
                        paging.Where("([Lead].ManagerId = {0} or [Lead].ManagerId is null)", manager.ManagerId);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(status))
                paging.Where("[DealStatus].Id = {0}", status.TryParseInt());

            paging.OrderByDesc("CreatedDate");

            var items = paging.PageItemsList<LeadModel>();

            return Json(items);
        }


        // GET: adminmobile/leads/{id}
        public ActionResult Lead(int id)
        {
            var lead = LeadService.GetLead(id);
            if (lead == null || LeadService.CheckAccess(lead) == false)
                return new EmptyResult();

            var model = new LeadViewModel {Lead = lead};

            if (lead.ManagerId != null)
            {
                var manager = ManagerService.GetManager((int) lead.ManagerId);
                if (manager != null)
                    model.Manager = CustomerService.GetCustomer(manager.CustomerId);
            }

            if (lead.CustomerId != null)
                model.Customer = CustomerService.GetCustomer((Guid)lead.CustomerId);

            model.CurrentCustomer = CustomerContext.CurrentCustomer;
            if (lead.LeadCurrency != null)
            {
                var currency = CurrencyService.GetCurrencyByIso3(lead.LeadCurrency.CurrencyCode);
                model.Currency = new OrderCurrency()
                {
                    CurrencyCode = lead.LeadCurrency.CurrencyCode,
                    CurrencyNumCode = lead.LeadCurrency.CurrencyNumCode,
                    CurrencySymbol = lead.LeadCurrency.CurrencySymbol,
                    CurrencyValue = lead.LeadCurrency.CurrencyValue,
                    IsCodeBefore = lead.LeadCurrency.IsCodeBefore,
                    RoundNumbers = currency != null ? currency.RoundNumbers : 0,
                    EnablePriceRounding = currency != null ? currency.EnablePriceRounding : false
                };
            }
            
            model.Statuses = DealStatusService.GetList().Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = x.Id == lead.DealStatusId
            }).ToList();

            model.Managers.Add(new SelectListItem() {Text = "-", Value = ""});

            foreach (var manager in ManagerService.GetManagersList().OrderBy(x => x.FullName))
            {
                model.Managers.Add(new SelectListItem()
                {
                    Text = manager.FirstName + " " + manager.LastName,
                    Value = manager.ManagerId.ToString(),
                    Selected = manager.ManagerId == lead.ManagerId
                });
            }

            SetMetaInformation(lead.FirstName);

            return View(model);
        }

        public JsonResult ChangeStatus(int id, int status)
        {
            var lead = LeadService.GetLead(id);
            if (lead == null || LeadService.CheckAccess(lead) == false)
                return Json(null);
            
            var statuses = DealStatusService.GetList();
            if (statuses.Find(x => x.Id == status) != null)
            {
                lead.DealStatusId = status;
                LeadService.UpdateLead(lead);
                return Json(new { Result = "success" });
            }

            return Json(null);
        }

        public JsonResult ChangeManager(int id, int managerId)
        {
            var lead = LeadService.GetLead(id);
            if (lead == null || LeadService.CheckAccess(lead) == false)
                return Json(null);

            var manager = ManagerService.GetManager(managerId);
            if (manager == null || !manager.Active)
                return Json(null);
            
            if (lead.ManagerId != manager.ManagerId)
            {
                lead.ManagerId = manager.ManagerId;
                LeadService.UpdateLead(lead);
                return Json(new { Result = "success" });
            }

            return Json(null);
        }

        [HttpPost]
        public JsonResult CreateOrder(int id)
        {
            var lead = LeadService.GetLead(id);

            if (lead == null || LeadService.CheckAccess(lead) == false)
                return Json(new { Error = "Error" });

            var order = OrderService.CreateOrder(lead);
            if (order == null)
                return Json(new { Error = "Error" });

            return Json(new { Result = "success", OrderUrl = Url.RouteUrl("AdminMobile_Order", new { orderId = order.OrderID }) }); ;
        }

        //[HttpPost]
        //public JsonResult CreateTask(int id)
        //{
        //    var lead = LeadService.GetLead(id);

        //    if (lead == null || LeadService.CheckAccess(lead) == false)
        //        return Json(new {Error = "Error"});

        //    var appointedManager = ManagerService.GetManager(CustomerContext.CurrentCustomer.Id);
        //    if (appointedManager == null)
        //        return Json(new {Error = T("AdminMobile.Leads.ErrorNotManager")});


        //    var managerId = lead.ManagerId ?? CustomerContext.CurrentCustomer.ManagerId;
        //    if (managerId == null)
        //        return Json(new { Error = T("AdminMobile.Leads.ErrorManagerIsNotSelected")});

        //    var customer = lead.CustomerId != null ? CustomerService.GetCustomer((Guid) lead.CustomerId) : null;
        //    if (customer == null)
        //    {
        //        customer = new Customer()
        //        {
        //            FirstName = lead.FirstName,
        //            Phone = lead.Phone,
        //            EMail = lead.Email,
        //            ManagerId = managerId
        //        };

        //        customer.Id = CustomerService.InsertNewCustomer(customer);
        //    }

        //    if (customer.Id == Guid.Empty)
        //        return Json(new {Error = T("AdminMobile.Leads.ErrorUserEmailExist")});

        //    try
        //    {
        //        var task = new ManagerTask()
        //        {
        //            Name = T("AdminMobile.Leads.TaskName") + lead.Id,
        //            Description = T("AdminMobile.Leads.TaskName") + lead.Id,
        //            AssignedManagerId = (int)managerId,
        //            AppointedManagerId = appointedManager.ManagerId,
        //            DueDate = DateTime.Now.AddMonths(1),
        //            LeadId = lead.Id,
        //            CustomerId = customer.Id
        //        };

        //        task.TaskId = ManagerTaskService.AddManagerTask(task);

        //        return Json(new {Result = "success", TaskUrl = Url.RouteUrl("AdminMobile_Task", new {taskId = task.TaskId})});
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.Log.Error(ex);
        //    }

        //    return Json(null);
        //}
    }
}