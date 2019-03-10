using System.Text;
using AdvantShop.Customers;

namespace AdvantShop.Core.Services.Crm.BusinessProcesses
{
    public abstract class BizProcessCustomerRule : BizProcessRule
    {
        public override EBizProcessObjectType ObjectType
        {
            get { return EBizProcessObjectType.Customer; }
        }

        public override string[] AvailableVariables
        {
            get
            {
                return new string[] { "#NAME#", "#EMAIL#" };
            }
        }

        public override string ReplaceVariables(string value, IBizObject bizObject)
        {
            var customer = (Customer)bizObject;
            var sb = new StringBuilder(value);
            sb.Replace("#NAME#", customer.GetFullName());
            sb.Replace("#EMAIL#", customer.EMail);

            return sb.ToString();
        }
    }

    public class BizProcessMessageReplyRule : BizProcessCustomerRule
    {
        public override EBizProcessEventType EventType
        {
            get { return EBizProcessEventType.MessageReply; }
        }
    }
}
