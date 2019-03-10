using System.Web.Mvc;
using AdvantShop.CMS;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Crm.BusinessProcesses;
using AdvantShop.Core.Services.IPTelephony;
using AdvantShop.Core.Services.Webhook;
using AdvantShop.Core.Services.Webhook.Models;
using AdvantShop.Customers;
using AdvantShop.Orders;
using AdvantShop.Web.Admin.Handlers.BizProcesses;
using AdvantShop.Web.Admin.Handlers.Tasks;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;
using AdvantShop.Saas;
using AdvantShop.Web.Admin.Handlers.AdminNotifications;

namespace AdvantShop.Web.Admin.Controllers.Settings
{
    [WebhookAuth(EWebhookType.BizProcess)]
    public class AdvantshopEventsController : BaseController
    {
        [HttpPost]
        public JsonResult OrderAdded(WebhookOrderModel model)
        {
            var order = OrderService.GetOrder(model.Id);
            if (order == null)
                return JsonError("order not found");

            var notificationsHandler = new AdminNotificationsHandler();
            notificationsHandler.NotifyAllCustomers(new OrderAddedNotification(order), RoleAction.Orders, model.CurrentCustomerId);
            notificationsHandler.UpdateOrders();

            if (SaasDataService.IsEnabledFeature(ESaasProperty.BizProcess))
            {
                var rules = BizProcessRuleService.GetBizProcessRules<BizProcessOrderCreatedRule>();
                var handler = new BizProcessOrderHandler<BizProcessOrderCreatedRule>(rules, order);
                handler.ProcessBizObject();
                handler.GenerateTask();
            }

            return JsonOk();
        }

        [HttpPost]
        public JsonResult OrderStatusChanged(WebhookOrderModel model)
        {
            var order = OrderService.GetOrder(model.Id);
            if (order == null)
                return JsonError("order not found");

            if (SaasDataService.IsEnabledFeature(ESaasProperty.BizProcess))
            {
                var rules = BizProcessRuleService.GetBizProcessRules<BizProcessOrderStatusChangedRule>(order.OrderStatusId);
                var handler = new BizProcessOrderHandler<BizProcessOrderStatusChangedRule>(rules, order);
                handler.GenerateTask();
            }

            return JsonOk();
        }

        [HttpPost]
        public JsonResult LeadAdded(WebhookLeadModel model)
        {
            var lead = LeadService.GetLead(model.Id);
            if (lead == null)
                return JsonError("lead not found");

            var notificationsHandler = new AdminNotificationsHandler();
            notificationsHandler.NotifyAllCustomers(new LeadAddedNotification(lead), RoleAction.Crm, model.CurrentCustomerId);
            notificationsHandler.UpdateLeads();

            if (SaasDataService.IsEnabledFeature(ESaasProperty.BizProcess))
            {
                var rules = BizProcessRuleService.GetBizProcessRules<BizProcessLeadCreatedRule>();
                var handler = new BizProcessLeadHandler<BizProcessLeadCreatedRule>(rules, lead);
                handler.ProcessBizObject();
                handler.GenerateTask();
            }

            return JsonOk();
        }

        [HttpPost]
        public JsonResult LeadStatusChanged(WebhookLeadModel model)
        {
            var lead = LeadService.GetLead(model.Id);
            if (lead == null)
                return JsonError("lead not found");

            if (SaasDataService.IsEnabledFeature(ESaasProperty.BizProcess))
            {
                var rules = BizProcessRuleService.GetBizProcessRules<BizProcessLeadStatusChangedRule>(lead.DealStatusId);
                var handler = new BizProcessLeadHandler<BizProcessLeadStatusChangedRule>(rules, lead);
                handler.GenerateTask();
            }

            return JsonOk();
        }

        [HttpPost]
        public JsonResult ProcessDeferredTasks()
        {
            if (SaasDataService.IsEnabledFeature(ESaasProperty.BizProcess))
            {
                var tasks = TaskService.GetDeferredTasks();
                foreach (var task in tasks)
                {
                    task.IsDeferred = false;
                    TaskService.UpdateTask(task);
                    new TaskAddedNotificationsHandler(task).Execute();
                }
            }

            return JsonOk();
        }

        [HttpPost]
        public JsonResult CallMissed(WebhookCallModel model)
        {
            var call = CallService.GetCall(model.Id);
            if (call == null)
                return JsonError("call not found");
            if (call.Type != ECallType.Missed)
                return JsonError("call not missed");

            if (SaasDataService.IsEnabledFeature(ESaasProperty.BizProcess))
            {
                var rules = BizProcessRuleService.GetBizProcessRules<BizProcessCallMissedRule>();
                var handler = new BizProcessHandler<BizProcessCallMissedRule>(rules, call);
                handler.GenerateTask();
            }

            return JsonOk();
        }

        [HttpPost]
        public JsonResult ReviewAdded(WebhookReviewModel model)
        {
            var review = ReviewService.GetReview(model.Id);
            if (review == null)
                return JsonError("review not found");

            if (SaasDataService.IsEnabledFeature(ESaasProperty.BizProcess))
            {
                var rules = BizProcessRuleService.GetBizProcessRules<BizProcessReviewAddedRule>();
                var handler = new BizProcessReviewHandler(rules, review);
                handler.GenerateTask();
            }

            return JsonOk();
        }

        [HttpPost]
        public JsonResult MessageReply(WebhookCustomerModel model)
        {
            var customer = CustomerService.GetCustomer(model.Id);
            if (customer == null)
                return JsonError("customer not found");

            if (SaasDataService.IsEnabledFeature(ESaasProperty.BizProcess))
            {
                var rules = BizProcessRuleService.GetBizProcessRules<BizProcessMessageReplyRule>();
                var handler = new BizProcessCustomerHandler<BizProcessMessageReplyRule>(rules, customer);
                handler.GenerateTask();
            }

            return JsonOk();
        }
    }
}
