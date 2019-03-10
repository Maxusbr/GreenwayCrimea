using System.Collections.Generic;
using AdvantShop.Core.Services.IPTelephony;

namespace AdvantShop.Core.Services.Crm.BusinessProcesses
{
    public class CallFilter : IBizObjectFilter
    {
        public CallFilter()
        {
            Comparers = new List<CallFieldComparer>();
        }

        public List<CallFieldComparer> Comparers { get; set; }

        public bool Check(IBizObject bizObject)
        {
            var call = (Call)bizObject;

            foreach (var comparer in Comparers)
            {
                if (!comparer.CheckField(call))
                    return false;
            }
            // если не заданы условия - звонок подходит в любом случае
            return true;
        }
    }
}
