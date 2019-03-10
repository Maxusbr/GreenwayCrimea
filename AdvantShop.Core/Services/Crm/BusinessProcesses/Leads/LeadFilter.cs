using System.Collections.Generic;

namespace AdvantShop.Core.Services.Crm.BusinessProcesses
{
    public class LeadFilter : IBizObjectFilter
    {
        public LeadFilter()
        {
            Comparers = new List<LeadFieldComparer>();
        }

        public List<LeadFieldComparer> Comparers { get; set; }

        public bool Check(IBizObject bizObject)
        {
            var lead = (Lead)bizObject;

            foreach (var comparer in Comparers)
            {
                if (!comparer.CheckField(lead))
                    return false;
            }
            // если не заданы условия - лид подходит в любом случае
            return true;
        }
    }
}
