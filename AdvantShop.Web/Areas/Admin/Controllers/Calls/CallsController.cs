using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.CMS;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.IPTelephony;
using AdvantShop.Customers;
using AdvantShop.Orders;
using AdvantShop.Saas;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Calls;
using AdvantShop.Web.Admin.Handlers.IPTelephony;
using AdvantShop.Web.Admin.Models.Calls;
using AdvantShop.Web.Admin.ViewModels.Calls;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Calls
{
    [Auth(RoleAction.Crm)]
    [SaasFeature(ESaasProperty.HaveTelephony)]
    public partial class CallsController : BaseAdminController
    {
        public ActionResult Index()
        {
            var model = new CallsListViewModel();

            SetMetaInformation(T("Admin.Calls.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.CallsCtrl);

            return View(model);
        }
        
        public JsonResult GetCalls(CallsFilterModel model)
        {
            var handler = new GetCallsHandler(model);
            var result = handler.Execute();

            return Json(result);
        }

        #region Command

        private void Command(CallsFilterModel model, Func<int, CallsFilterModel, bool> func)
        {
            if (model.SelectMode == SelectModeCommand.None)
            {
                foreach (var id in model.Ids)
                {
                    func(id, model);
                }
            }
            else
            {
                var handler = new GetCallsHandler(model);
                var ids = handler.GetItemsIds();

                foreach (int id in ids)
                {
                    if (model.Ids == null || !model.Ids.Contains(id))
                        func(id, model);
                }
            }
        }

        #endregion

        public JsonResult DeleteCalls(CallsFilterModel model)
        {
            Command(model, (id, c) =>
            {
                CallService.DeleteCall(id);
                return true;
            });

            return Json(true);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteCall(int callId)
        {
            CallService.DeleteCall(callId);
            return Json(new { result = true });
        }

        public JsonResult GetTypesSelectOptions()
        {
            return Json(Enum.GetValues(typeof(ECallType)).Cast<ECallType>().Select(x => new
            {
                label = x.Localize(),
                value = ((int)x).ToString(),
            }));
        }

        [HttpPost]
        public JsonResult GetRecordLink(EOperatorType type, int callId)
        {
            if (callId == 0 || type == EOperatorType.None)
                return Json(new { result = false });

            var @operator = IPTelephonyOperator.GetByType(type);
            return Json(new
            {
                result = true,
                link = @operator.GetRecordLink(callId)
            });
        }

        [HttpPost]
        public JsonResult GetNotifications()
        {
            var notifications = new List<AdminNotification>();
            var incomingCalls = CallService.GetNotCompletedIncomingCalls();
            foreach (var call in incomingCalls)
            {
                var handler = new IPTelephonyHandler(call);
                notifications.AddRange(handler.GetToasterNotifications());
            }

            return Json(new { notifications });
        }

        [HttpPost]
        public JsonResult CheckDataAccess(int? lastOrderId, int? lastLeadId, int? leadId)
        {
            var customer = CustomerContext.CurrentCustomer;
            var hasOrdersAccess = true;
            var hasCRMAccess = true;
            var hasCustomersAccess = true;
            if (customer.IsModerator)
            {
                hasOrdersAccess = customer.HasRoleAction(RoleAction.Orders);
                hasCRMAccess = customer.HasRoleAction(RoleAction.Crm);
                hasCustomersAccess = customer.HasRoleAction(RoleAction.Customers);
                Order lastOrder;
                if (lastOrderId.HasValue && ((lastOrder = OrderService.GetOrder(lastOrderId.Value)) == null || !hasOrdersAccess || !OrderService.CheckAccess(lastOrder)))
                {
                    lastOrderId = null;
                }
                Lead lastLead;
                if (lastLeadId.HasValue && ((lastLead = LeadService.GetLead(lastLeadId.Value)) == null || !hasCRMAccess || !LeadService.CheckAccess(lastLead)))
                {
                    lastLeadId = null;
                }
                Lead lead;
                if (lastLeadId.HasValue && ((lead = LeadService.GetLead(lastLeadId.Value)) == null || !hasCRMAccess || !LeadService.CheckAccess(lead)))
                {
                    leadId = null;
                }
            }

            return Json(new { lastOrderId, lastLeadId, leadId, hasOrdersAccess, hasCRMAccess, hasCustomersAccess });
        }
    }
}
