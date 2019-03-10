using System;
using System.Collections.Generic;

namespace AdvantShop.Core.Services.Loging.Smses
{
    public interface ISmsLoger : IAdvantShopLoger
    {
        void LogSms(TextMessage message);

        List<TextMessage> GetSms(Guid customerId, long phone);
    }
}
