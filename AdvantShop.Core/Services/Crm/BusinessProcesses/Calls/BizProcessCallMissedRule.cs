using System.Linq;
using System.Text;
using AdvantShop.Core.Services.IPTelephony;
using AdvantShop.Helpers;

namespace AdvantShop.Core.Services.Crm.BusinessProcesses
{
    public class BizProcessCallMissedRule : BizProcessRule
    {
        public override EBizProcessObjectType ObjectType
        {
            get { return EBizProcessObjectType.Call; }
        }

        public override EBizProcessEventType EventType
        {
            get { return EBizProcessEventType.CallMissed; }
        }

        public override string[] AvailableVariables
        {
            get
            {
                return new string[] { "#PHONE#", "#NAME#", "#EMAIL#" };
            }
        }

        public override string ReplaceVariables(string value, IBizObject bizObject)
        {
            var call = (Call)bizObject;
            var sb = new StringBuilder(value);
            sb.Replace("#PHONE#", call.Phone);
            var customer = call.Customers.FirstOrDefault();
            sb.Replace("#NAME#", customer != null ? StringHelper.AggregateStrings(" ", customer.LastName, customer.FirstName, customer.Patronymic) : "");
            sb.Replace("#EMAIL#", customer != null ? customer.EMail : "");

            return sb.ToString();
        }
    }
}
