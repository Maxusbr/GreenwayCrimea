using System.Text;
using AdvantShop.Core.Services.Crm.DealStatuses;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Helpers;

namespace AdvantShop.Core.Services.Crm.BusinessProcesses
{
    public abstract class BizProcessLeadRule : BizProcessRule
    {
        public override EBizProcessObjectType ObjectType
        {
            get { return EBizProcessObjectType.Lead; }
        }

        public override string[] AvailableVariables
        {
            get
            {
                return new string[] { "#LEAD_ID#", "#NAME#", "#PHONE#", "#EMAIL#" };
            }
        }

        public override string ReplaceVariables(string value, IBizObject bizObject)
        {
            var lead = (Lead)bizObject;
            var sb = new StringBuilder(value);
            sb.Replace("#LEAD_ID#", lead.Id.ToString());
            if (lead.Customer != null)
            {
                sb.Replace("#NAME#", StringHelper.AggregateStrings(" ", lead.Customer.LastName, lead.Customer.FirstName, lead.Customer.Patronymic));
                sb.Replace("#PHONE#", lead.Customer.Phone);
                sb.Replace("#EMAIL#", lead.Customer.EMail);
            }
            else
            {
                sb.Replace("#NAME#", StringHelper.AggregateStrings(" ", lead.LastName, lead.FirstName, lead.Patronymic));
                sb.Replace("#PHONE#", lead.Phone);
                sb.Replace("#EMAIL#", lead.Email);
            }

            return sb.ToString();
        }
    }

    public class BizProcessLeadCreatedRule : BizProcessLeadRule
    {
        public override EBizProcessEventType EventType
        {
            get { return EBizProcessEventType.LeadCreated; }
        }
    }

    public class BizProcessLeadStatusChangedRule : BizProcessLeadRule
    {
        public override EBizProcessEventType EventType
        {
            get { return EBizProcessEventType.LeadStatusChanged; }
        }

        private string _eventName;
        public override string EventName
        {
            get
            {
                if (_eventName != null)
                    return _eventName;

                var dealStatus = EventObjId.HasValue ? DealStatusService.Get(EventObjId.Value) : null;
                _eventName = base.EventName + (dealStatus != null ? string.Format(" {0} {1}", LocalizationService.GetResource("Core.BizProcessRule.ChangeState.On"), dealStatus.Name) : string.Empty);

                return _eventName;
            }
        }
    }
}
