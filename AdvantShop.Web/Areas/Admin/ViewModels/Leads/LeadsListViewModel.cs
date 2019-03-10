using System.Collections.Generic;
using AdvantShop.Core.Services.Crm.DealStatuses;

namespace AdvantShop.Web.Admin.ViewModels.Leads
{
    public class LeadsListViewModel
    {
        public List<DealStatus> DealStatuses
        {
            get { return DealStatusService.GetList(); }
        }
    }
}
