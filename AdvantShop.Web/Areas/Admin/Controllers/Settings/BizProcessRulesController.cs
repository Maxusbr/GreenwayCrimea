using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Scheduler;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Crm.BusinessProcesses;
using AdvantShop.Core.Services.Crm.DealStatuses;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.BizProcessRules;
using AdvantShop.Web.Admin.Models;
using AdvantShop.Web.Admin.Models.BizProcessRules;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Settings
{
    [Auth(RoleAction.Settings)]
    [SaasFeature(Saas.ESaasProperty.BizProcess)]
    public class BizProcessRulesController : BaseAdminController
    {
        public JsonResult GetBizProcessRules(BizProcessRulesFilterModel model)
        {
            var items = new GetBizProcessRulesHandler(model).Execute();
            return Json(new { DataItems = items });
        }

        #region CRUD BizProcessRule

        public JsonResult Get(int id, EBizProcessEventType eventType)
        {
            var dbModel = new GetBizProcessRuleHandler(eventType, id).Execute();

            if (dbModel == null)
                return Json(new { result = false });

            var result = (BizProcessRuleModel)dbModel;

            return Json(result);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Add([ModelBinder(typeof(BizProcessRuleModelBinder))] BizProcessRuleModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = new List<string>();
                foreach (var modelState in ViewData.ModelState.Values)
                    foreach (var error in modelState.Errors)
                        errors.Add(error.ErrorMessage);
                return Json(new { result = false, error = errors.AggregateString(" ") });
            }
            var dbModel = new GetBizProcessRuleHandler(model.EventType).Execute();

            dbModel.EventObjId = model.EventObjId;
            dbModel.TaskDueDateInterval = model.TaskDueDateInterval;
            dbModel.TaskCreateInterval = model.TaskCreateInterval;
            dbModel.Priority = model.Priority;
            dbModel.TaskName = model.TaskName;
            dbModel.TaskDescription = model.TaskDescription;
            dbModel.TaskPriority = model.TaskPriority;
            dbModel.TaskGroupId = model.TaskGroupId;
            dbModel.ManagerFilter = model.ManagerFilter;
            dbModel.Filter = model.Filter;

            BizProcessRuleService.AddBizProcessRule(dbModel);

            return Json(new { result = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Update([ModelBinder(typeof(BizProcessRuleModelBinder))] BizProcessRuleModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = new List<string>();
                foreach (var modelState in ViewData.ModelState.Values)
                    foreach (var error in modelState.Errors)
                        errors.Add(error.ErrorMessage);
                return Json(new { result = false, error = errors.AggregateString(" ") });
            }
            var dbModel = new GetBizProcessRuleHandler(model.EventType, model.Id).Execute();

            if (dbModel == null)
                return Json(new { result = false });

            dbModel.TaskDueDateInterval = model.TaskDueDateInterval;
            dbModel.TaskCreateInterval = model.TaskCreateInterval;
            dbModel.Priority = model.Priority;
            dbModel.TaskName = model.TaskName;
            dbModel.TaskDescription = model.TaskDescription;
            dbModel.TaskPriority = model.TaskPriority;
            dbModel.TaskGroupId = model.TaskGroupId;
            dbModel.ManagerFilter = model.ManagerFilter;
            dbModel.Filter = model.Filter;

            BizProcessRuleService.UpdateBizProcessRule(dbModel);

            return Json(new { result = true, ModelState });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Delete(int id)
        {
            BizProcessRuleService.DeleteBizProcessRule(id);
            return Json(new { result = true });
        }

        #endregion

        public JsonResult GetRuleFormData(EBizProcessEventType eventType)
        {
            List<SelectItemModel> eventObjects = null;
            string filterTypeName = string.Empty;
            var useFilter = true;
            var rule = new GetBizProcessRuleHandler(eventType).Execute();
            string eventName = eventType.Localize();
            switch (eventType)
            {
                case EBizProcessEventType.OrderCreated:
                    filterTypeName = T("Admin.BizProcessRules.FilterTypeName.Order");
                    break;
                case EBizProcessEventType.OrderStatusChanged:
                    filterTypeName = T("Admin.BizProcessRules.FilterTypeName.Order");
                    eventObjects = OrderStatusService.GetOrderStatuses().Select(x => new SelectItemModel(x.StatusName, x.StatusID.ToString())).ToList();
                    break;
                case EBizProcessEventType.LeadCreated:
                    filterTypeName = T("Admin.BizProcessRules.FilterTypeName.Lead");
                    break;
                case EBizProcessEventType.LeadStatusChanged:
                    filterTypeName = T("Admin.BizProcessRules.FilterTypeName.Lead");
                    eventObjects = DealStatusService.GetList().Select(x => new SelectItemModel(x.Name, x.Id.ToString())).ToList();
                    break;
                case EBizProcessEventType.CallMissed:
                    filterTypeName = T("Admin.BizProcessRules.FilterTypeName.Call");
                    break;
                case EBizProcessEventType.ReviewAdded:
                    useFilter = false;
                    break;
                case EBizProcessEventType.MessageReply:
                    useFilter = false;
                    break;
                default:
                    throw new NotImplementedException("No implementation for event type " + eventType);
            }

            var intervalTypes = Enum.GetValues(typeof(TimeIntervalType)).Cast<TimeIntervalType>()
                .Select(x => new SelectItemModel(x.Localize(), x.ConvertIntString()));
            var taskPriorities = Enum.GetValues(typeof(TaskPriority)).Cast<TaskPriority>()
                .Select(x => new SelectItemModel(x.Localize(), x.ConvertIntString()));

            return Json(new
            {
                eventName,
                eventObjects,
                intervalTypes,
                filterTypeName,
                taskPriorities,
                taskGroups = TaskGroupService.GetAllTaskGroups().Select(x => new SelectItemModel(x.Name, x.Id.ToString())),
                availableVariables = rule.AvailableVariables,
                useFilter
            });
        }

        public JsonResult GetFilterRuleFormData(EBizProcessEventType eventType)
        {
            var fields = new GetRuleFieldsHandler(eventType).Execute();
            return Json(new { fields });
        }

        public JsonResult GetFilterRuleParamValues(EBizProcessEventType eventType, int fieldType, int? fieldObjId)
        {
            var values = new GetRuleParamValuesHandler(eventType, fieldType, fieldObjId).Execute();
            return Json(new { values });
        }

        public JsonResult GetManagerFilterRuleFormData(EBizProcessEventType eventType)
        {
            var managerFilterTypes = Enum.GetValues(typeof(EManagerFilterType)).Cast<EManagerFilterType>()
                .Where(x => x != EManagerFilterType.Specific && x != EManagerFilterType.FromBizObject)
                .Select(x => new
                {
                    label = x.Localize(),
                    value = x.ConvertIntString(),
                    type = x.ConvertIntString()
                }).ToList();

            string labelFromBizObject = null;
            switch (eventType)
            {
                case EBizProcessEventType.OrderCreated:
                case EBizProcessEventType.OrderStatusChanged:
                    labelFromBizObject = LocalizationService.GetResource("Core.Crm.EManagerFilterType.FromOrder");
                    break;
                case EBizProcessEventType.LeadCreated:
                case EBizProcessEventType.LeadStatusChanged:
                    labelFromBizObject = LocalizationService.GetResource("Core.Crm.EManagerFilterType.FromLead");
                    break;
                case EBizProcessEventType.MessageReply:
                    labelFromBizObject = LocalizationService.GetResource("Core.Crm.EManagerFilterType.FromCustomer");
                    break;
            }
            if (labelFromBizObject.IsNotEmpty())
            {
                managerFilterTypes.Add(new
                {
                    label = labelFromBizObject,
                    value = EManagerFilterType.FromBizObject.ConvertIntString(),
                    type = EManagerFilterType.FromBizObject.ConvertIntString()
                });
            }

            var customers = CustomerService.GetCustomersByRoles(Role.Administrator, Role.Moderator).Where(x => x.Enabled);
            managerFilterTypes.AddRange(
                customers.Select(x => new
                {
                    label = StringHelper.AggregateStrings(" ", x.FirstName, x.LastName),
                    value = x.Id.ToString(),
                    type = EManagerFilterType.Specific.ConvertIntString()
                }).OrderBy(x => x.label));

            return Json(new
            {
                managerFilterTypes,
                managerRoles = ManagerRoleService.GetManagerRoles().Select(x => new
                {
                    label = x.Name,
                    value = x.Id
                })
            });
        }
    }
}
