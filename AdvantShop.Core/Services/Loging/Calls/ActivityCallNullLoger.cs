using System;
using System.Collections.Generic;

namespace AdvantShop.Core.Services.Loging.Calls
{
    partial class ActivityCallNullLoger : ICallLoger
    {
        public virtual void LogCall(Call call)
        {
        }

        public virtual List<Call> GetCalls(Guid customerId, string call)
        {
            return null;
        }
    }
}
