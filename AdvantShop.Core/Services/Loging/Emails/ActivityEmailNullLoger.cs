using System;
using System.Collections.Generic;

namespace AdvantShop.Core.Services.Loging.Emails
{
    partial class ActivityEmailNullLoger : IEmailLoger
    {
        public virtual void LogEmail(Email email)
        {
        }

        public virtual List<Email> GetEmails(Guid customerId, string email)
        {
            return null;
        }
    }
}
