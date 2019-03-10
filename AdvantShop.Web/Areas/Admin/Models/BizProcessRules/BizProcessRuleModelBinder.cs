using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Scheduler;
using AdvantShop.Core.Services.Crm.BusinessProcesses;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Customers;
using Newtonsoft.Json;

namespace AdvantShop.Web.Admin.Models.BizProcessRules
{
    public class BizProcessRuleModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var model = base.BindModel(controllerContext, bindingContext) as BizProcessRuleModel;
            model.TaskDueDateInterval = model.TaskDueDateIntervalSerialized != null
                ? JsonConvert.DeserializeObject<TimeInterval>(model.TaskDueDateIntervalSerialized)
                : null;
            model.TaskCreateInterval = model.TaskCreateIntervalSerialized != null
                ? JsonConvert.DeserializeObject<TimeInterval>(model.TaskCreateIntervalSerialized)
                : null;
            model.ManagerFilter = model.ManagerFilterSerialized != null
                ? JsonConvert.DeserializeObject<ManagerFilter>(model.ManagerFilterSerialized)
                : null;

            if (model.TaskDueDateInterval != null && model.TaskDueDateInterval.Interval <= 0)
                model.TaskDueDateInterval = null;
            if (model.TaskCreateInterval != null && model.TaskCreateInterval.Interval <= 0)
                model.TaskCreateInterval = null;

            if (model.TaskName.IsNullOrEmpty())
                bindingContext.ModelState.AddModelError("TaskName", LocalizationService.GetResource("Admin.BizProcessRules.Validate.NoTaskName"));
            if (model.TaskDescription.IsNullOrEmpty())
                bindingContext.ModelState.AddModelError("TaskDescription", LocalizationService.GetResource("Admin.BizProcessRules.Validate.NoTaskDescription"));

            if (model.ManagerFilter == null || !model.ManagerFilter.Comparers.Any())
                bindingContext.ModelState.AddModelError("ManagerFilter", LocalizationService.GetResource("Admin.BizProcessRules.Validate.NoManagerFilter"));
            else
            {
                var managerErrors = new List<string>();
                foreach (var comparer in model.ManagerFilter.Comparers)
                {
                    Customer customer;
                    if (comparer.FilterType == EManagerFilterType.Specific && (!comparer.CustomerId.HasValue || (customer = CustomerService.GetCustomer(comparer.CustomerId.Value)) == null))
                        managerErrors.Add(LocalizationService.GetResource("Admin.BizProcessRules.Validate.ManagerNotSet"));
                }
                if (managerErrors.Any())
                    bindingContext.ModelState.AddModelError("ManagerFilter", LocalizationService.GetResourceFormat("Admin.BizProcessRules.Validate.ManagerFilterErrors", managerErrors.AggregateString(", ")));
            }

            if (model.TaskCreateInterval != null && model.TaskCreateInterval.IntervalType == TimeIntervalType.Minutes && model.TaskCreateInterval.Interval < 5)
                bindingContext.ModelState.AddModelError("TaskCreateInterval", LocalizationService.GetResource("Admin.BizProcessRules.Validate.WrongTaskCreateInterval"));

            switch (model.EventType)
            {
                case EBizProcessEventType.OrderCreated:
                    model.Filter = BizProcessRuleService.GetBizObjectFilterFromJson<BizProcessOrderCreatedRule>(model.FilterSerialized);
                    ValidateOrderFilter(bindingContext, model.Filter as OrderFilter);
                    break;
                case EBizProcessEventType.OrderStatusChanged:
                    model.Filter = BizProcessRuleService.GetBizObjectFilterFromJson<BizProcessOrderStatusChangedRule>(model.FilterSerialized);
                    ValidateOrderFilter(bindingContext, (OrderFilter)model.Filter);
                    if (!model.EventObjId.HasValue)
                        bindingContext.ModelState.AddModelError("EventObjId", LocalizationService.GetResource("Admin.BizProcessRules.Validate.EventObjectNotSet"));
                    break;
                case EBizProcessEventType.LeadCreated:
                    model.Filter = BizProcessRuleService.GetBizObjectFilterFromJson<BizProcessLeadCreatedRule>(model.FilterSerialized);
                    ValidateLeadFilter(bindingContext, (LeadFilter)model.Filter);
                    break;
                case EBizProcessEventType.LeadStatusChanged:
                    if (!model.EventObjId.HasValue)
                        bindingContext.ModelState.AddModelError("EventObjId", LocalizationService.GetResource("Admin.BizProcessRules.Validate.EventObjectNotSet"));
                    model.Filter = BizProcessRuleService.GetBizObjectFilterFromJson<BizProcessLeadStatusChangedRule>(model.FilterSerialized);
                    ValidateLeadFilter(bindingContext, (LeadFilter)model.Filter);
                    break;
                case EBizProcessEventType.CallMissed:
                    model.Filter = BizProcessRuleService.GetBizObjectFilterFromJson<BizProcessCallMissedRule>(model.FilterSerialized);
                    ValidateCallFilter(bindingContext, (CallFilter)model.Filter);
                    break;
                case EBizProcessEventType.ReviewAdded:
                    model.Filter = BizProcessRuleService.GetBizObjectFilterFromJson<BizProcessReviewAddedRule>(model.FilterSerialized);
                    break;
                case EBizProcessEventType.MessageReply:
                    model.Filter = BizProcessRuleService.GetBizObjectFilterFromJson<BizProcessMessageReplyRule>(model.FilterSerialized);
                    break;
                default:
                    throw new NotImplementedException("No implementation for event type " + model.EventType);
            }
            return model;
        }

        private bool ValidateOrderFilter(ModelBindingContext bindingContext, OrderFilter filter)
        {
            var errors = new List<string>();
            foreach (var comparer in filter.Comparers)
            {
                string error;
                if (!ValidateFieldComparer(comparer.FieldComparer, comparer.FieldType, comparer.FieldType == EOrderFieldType.CustomerField, comparer.FieldName, out error))
                    errors.Add(error);
            }
            if (errors.Any())
                bindingContext.ModelState.AddModelError("Filter", LocalizationService.GetResourceFormat("Admin.BizProcessRules.Validate.OrderFilterErrors", errors.AggregateString(", ")));

            return !errors.Any();
        }

        private bool ValidateLeadFilter(ModelBindingContext bindingContext, LeadFilter filter)
        {
            var errors = new List<string>();
            foreach (var comparer in filter.Comparers)
            {
                string error;
                if (!ValidateFieldComparer(comparer.FieldComparer, comparer.FieldType, comparer.FieldType == ELeadFieldType.CustomerField, comparer.FieldName, out error))
                    errors.Add(error);
            }
            if (errors.Any())
                bindingContext.ModelState.AddModelError("Filter", LocalizationService.GetResourceFormat("Admin.BizProcessRules.Validate.LeadFilterErrors", errors.AggregateString(", ")));

            return !errors.Any();
        }

        private bool ValidateCallFilter(ModelBindingContext bindingContext, CallFilter filter)
        {
            var errors = new List<string>();
            foreach (var comparer in filter.Comparers)
            {
                string error;
                if (!ValidateFieldComparer(comparer.FieldComparer, comparer.FieldType, comparer.FieldType == ECallFieldType.CustomerField, comparer.FieldName, out error))
                    errors.Add(error);
            }
            if (errors.Any())
                bindingContext.ModelState.AddModelError("Filter", LocalizationService.GetResourceFormat("Admin.BizProcessRules.Validate.CallFilterErrors", errors.AggregateString(", ")));

            return !errors.Any();
        }

        private bool ValidateFieldComparer(FieldComparer fieldComparer, Enum bizObjectFieldType, bool isCustomerField, string fieldName, out string error)
        {
            var fieldType = BizProcessRuleService.GetFieldType(bizObjectFieldType, isCustomerField, fieldComparer.FieldObjId);
            error = null;
            switch (fieldComparer.Type)
            {
                case EFieldComparerType.Equal:
                    var equalityComparer = fieldComparer as FieldEqualityComparer;
                    if (equalityComparer.Value.IsNullOrEmpty())
                        error = LocalizationService.GetResourceFormat("Admin.BizProcessRules.Validate.FieldComparer.NoValue", fieldName);
                    else if (fieldType == EFieldType.Date && !equalityComparer.Value.TryParseDateTime(true).HasValue)
                        error = LocalizationService.GetResourceFormat("Admin.BizProcessRules.Validate.FieldComparer.WrongDate", fieldName);
                    else if (fieldType == EFieldType.Number && !equalityComparer.Value.TryParseFloat(true).HasValue)
                        error = LocalizationService.GetResourceFormat("Admin.BizProcessRules.Validate.FieldComparer.WrongNumber", fieldName);
                    break;
                case EFieldComparerType.Range:
                    var rangeComparer = fieldComparer as FieldRangeComparer;
                    if (fieldType == EFieldType.Date && !rangeComparer.DateFrom.HasValue && !rangeComparer.DateTo.HasValue)
                        error = LocalizationService.GetResourceFormat("Admin.BizProcessRules.Validate.FieldComparer.NoDateRange", fieldName);
                    else if (fieldType != EFieldType.Date && !rangeComparer.From.HasValue && !rangeComparer.To.HasValue)
                        error = LocalizationService.GetResourceFormat("Admin.BizProcessRules.Validate.FieldComparer.NoRange", fieldName);
                    break;
                case EFieldComparerType.Flag:
                    break;
                case EFieldComparerType.Contains:
                    var containsComparer = fieldComparer as FieldContainsComparer;
                    if (containsComparer.Value.IsNullOrEmpty())
                        error = LocalizationService.GetResourceFormat("Admin.BizProcessRules.Validate.FieldComparer.NoValue", fieldName);
                    break;
                default:
                    throw new NotImplementedException("No implementation for FieldComparerType " + fieldComparer.Type);
            }
            return error.IsNullOrEmpty();
        }
    }
}
