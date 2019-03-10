using System;
using System.Collections.Generic;

namespace AdvantShop.Core.Services.Loging.TrafficSource
{
    public interface ITrafficSourceLoger : IAdvantShopLoger
    {
        void LogTrafficSource();

        void LogOrderTafficSource(int objId, TrafficSourceType type, bool isFromAdminArea);

        List<TrafficSource> GetTrafficSources(Guid customerId);
    }
}