using System;
using System.Collections.Generic;

namespace AdvantShop.Core.Services.Loging.Smses
{
    partial class ActivitySmsNullLoger : ISmsLoger
    {
        public virtual void LogSms(TextMessage message)
        {
        }

        public virtual List<TextMessage> GetSms(Guid customerId, long phone)
        {
            return null;
        }
    }
}