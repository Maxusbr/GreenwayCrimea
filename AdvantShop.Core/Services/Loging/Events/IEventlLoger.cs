using System;
using System.Collections.Generic;

namespace AdvantShop.Core.Services.Loging.Events
{
    public interface IEventLoger : IAdvantShopLoger
    {
        void LogEvent(Event email);

        List<Event> GetEvents(Guid customerId);
    }
}