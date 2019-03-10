using System;
using AdvantShop.Core.Services.Crm.BusinessProcesses;

namespace AdvantShop.Web.Admin.Handlers.BizProcessRules
{
    public class GetBizProcessRuleHandler
    {
        private readonly int? _id;
        private readonly EBizProcessEventType _eventType;

        public GetBizProcessRuleHandler(EBizProcessEventType eventType, int? id = null)
        {
            _id = id;
            _eventType = eventType;
        }

        public BizProcessRule Execute()
        {
            switch (_eventType)
            {
                case EBizProcessEventType.OrderCreated:
                    return _id.HasValue ? BizProcessRuleService.GetBizProcessRule<BizProcessOrderCreatedRule>(_id.Value) : new BizProcessOrderCreatedRule();
                case EBizProcessEventType.OrderStatusChanged:
                    return _id.HasValue ? BizProcessRuleService.GetBizProcessRule<BizProcessOrderStatusChangedRule>(_id.Value) : new BizProcessOrderStatusChangedRule();
                case EBizProcessEventType.LeadCreated:
                    return _id.HasValue ? BizProcessRuleService.GetBizProcessRule<BizProcessLeadCreatedRule>(_id.Value) : new BizProcessLeadCreatedRule();
                case EBizProcessEventType.LeadStatusChanged:
                    return _id.HasValue ? BizProcessRuleService.GetBizProcessRule<BizProcessLeadStatusChangedRule>(_id.Value) : new BizProcessLeadStatusChangedRule();
                case EBizProcessEventType.CallMissed:
                    return _id.HasValue ? BizProcessRuleService.GetBizProcessRule<BizProcessCallMissedRule>(_id.Value) : new BizProcessCallMissedRule();
                case EBizProcessEventType.ReviewAdded:
                    return _id.HasValue ? BizProcessRuleService.GetBizProcessRule<BizProcessReviewAddedRule>(_id.Value) : new BizProcessReviewAddedRule();
                case EBizProcessEventType.MessageReply:
                    return _id.HasValue ? BizProcessRuleService.GetBizProcessRule<BizProcessMessageReplyRule>(_id.Value) : new BizProcessMessageReplyRule();
                default:
                    throw new NotImplementedException("No implementation for event type " + _eventType);
            }
        }
    }
}
