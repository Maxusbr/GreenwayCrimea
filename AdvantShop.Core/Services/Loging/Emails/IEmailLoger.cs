using System;
using System.Collections.Generic;

namespace AdvantShop.Core.Services.Loging.Emails
{
    public interface IEmailLoger : IAdvantShopLoger
    {
        void LogEmail(Email email);

        List<Email> GetEmails(Guid customerId, string email);
    }
}
