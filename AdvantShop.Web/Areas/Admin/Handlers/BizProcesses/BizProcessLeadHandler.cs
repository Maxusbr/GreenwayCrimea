using System.Collections.Generic;
using AdvantShop.Core.Services.Crm;
using AdvantShop.Core.Services.Crm.BusinessProcesses;
using AdvantShop.Web.Admin.Models.Tasks;

namespace AdvantShop.Web.Admin.Handlers.BizProcesses
{
    public class BizProcessLeadHandler<TRule> : BizProcessHandler<TRule> where TRule : BizProcessLeadRule
    {
        private readonly Lead _lead;

        public BizProcessLeadHandler(List<TRule> rules, Lead lead) : base(rules, lead)
        {
            _lead = lead;
        }

        public override TaskModel GenerateTask()
        {
            TaskModel = new TaskModel
            {
                LeadId = _lead.Id
            };

            return base.GenerateTask();
        }

        public override void ProcessBizObject()
        {
            if (!_lead.ManagerId.HasValue && Employee != null)
            {
                _lead.ManagerId = Employee.AssociatedManagerId;
                LeadService.UpdateLeadManager(_lead.Id, Employee.AssociatedManagerId);
            }
        }
    }


}
