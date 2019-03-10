using AdvantShop.Core.Services.CustomerSegments;
using Quartz;

namespace AdvantShop.Web.Admin.Handlers.CustomerSegments
{
    [DisallowConcurrentExecution]
    public class CustomerSegmentsJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var segments = CustomerSegmentService.GetList();
            if (segments.Count == 0)
                return;
            
            foreach (var segment in segments)
            {
                new RecalcCustomerSegment(segment.Id).Execute();
            }
        }
    }
}
