using System.Collections.Generic;
using AdvantShop.CMS;
using AdvantShop.Core.Services.Crm.BusinessProcesses;
using AdvantShop.Web.Admin.Models.Tasks;

namespace AdvantShop.Web.Admin.Handlers.BizProcesses
{
    public class BizProcessReviewHandler : BizProcessHandler<BizProcessReviewAddedRule>
    {
        private readonly Review _review;

        public BizProcessReviewHandler(List<BizProcessReviewAddedRule> rules, Review review) : base(rules, review)
        {
            _review = review;
        }

        public override TaskModel GenerateTask()
        {
            TaskModel = new TaskModel
            {
                ReviewId = _review.ReviewId
            };

            return base.GenerateTask();
        }
    }


}
