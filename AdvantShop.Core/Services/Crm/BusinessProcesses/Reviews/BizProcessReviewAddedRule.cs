using System.Text;
using AdvantShop.CMS;

namespace AdvantShop.Core.Services.Crm.BusinessProcesses
{
    public class BizProcessReviewAddedRule : BizProcessRule
    {
        public override EBizProcessObjectType ObjectType
        {
            get { return EBizProcessObjectType.Review; }
        }

        public override EBizProcessEventType EventType
        {
            get { return EBizProcessEventType.ReviewAdded; }
        }

        public override string[] AvailableVariables
        {
            get
            {
                return new string[] { "#NAME#", "#EMAIL#", "#TEXT#" };
            }
        }

        public override string ReplaceVariables(string value, IBizObject bizObject)
        {
            var review = (Review)bizObject;
            var sb = new StringBuilder(value);
            sb.Replace("#NAME#", review.Name);
            sb.Replace("#EMAIL#", review.Email);
            sb.Replace("#TEXT#", review.Text);

            return sb.ToString();
        }
    }
}
