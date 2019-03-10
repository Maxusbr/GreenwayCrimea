using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Services.Crm.BusinessProcesses;
using AdvantShop.Web.Admin.Models.BizProcessRules;

namespace AdvantShop.Web.Admin.Handlers.BizProcessRules
{
    public class GetBizProcessRulesHandler
    {
        private readonly BizProcessRulesFilterModel _filterModel;

        public GetBizProcessRulesHandler(BizProcessRulesFilterModel filterModel)
        {
            _filterModel = filterModel;
        }

        public List<BizProcessRuleModel> Execute()
        {
            var rules = new List<BizProcessRule>();
            switch (_filterModel.EventType)
            {
                case EBizProcessEventType.OrderCreated:
                    rules = new List<BizProcessRule>(BizProcessRuleService.GetBizProcessRules<BizProcessOrderCreatedRule>());
                    break;
                case EBizProcessEventType.OrderStatusChanged:
                    rules = new List<BizProcessRule>(BizProcessRuleService.GetBizProcessRules<BizProcessOrderStatusChangedRule>().OrderBy(x => x.EventObjId));
                    break;
                case EBizProcessEventType.LeadCreated:
                    rules = new List<BizProcessRule>(BizProcessRuleService.GetBizProcessRules<BizProcessLeadCreatedRule>());
                    break;
                case EBizProcessEventType.LeadStatusChanged:
                    rules = new List<BizProcessRule>(BizProcessRuleService.GetBizProcessRules<BizProcessLeadStatusChangedRule>().OrderBy(x => x.EventObjId));
                    break;
                case EBizProcessEventType.CallMissed:
                    rules = new List<BizProcessRule>(BizProcessRuleService.GetBizProcessRules<BizProcessCallMissedRule>());
                    break;
                case EBizProcessEventType.ReviewAdded:
                    rules = new List<BizProcessRule>(BizProcessRuleService.GetBizProcessRules<BizProcessReviewAddedRule>());
                    break;
                case EBizProcessEventType.MessageReply:
                    rules = new List<BizProcessRule>(BizProcessRuleService.GetBizProcessRules<BizProcessMessageReplyRule>());
                    break;
                default:
                    throw new NotImplementedException("No implementation for event type " + _filterModel.EventType);
            }
            return rules.Select(x => (BizProcessRuleModel)x).ToList();
        }
    }
}