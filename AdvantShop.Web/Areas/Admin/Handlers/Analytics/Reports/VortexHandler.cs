using System;
using System.Collections.Generic;
using System.Globalization;
using AdvantShop.Orders;
using AdvantShop.SEO;

namespace AdvantShop.Web.Admin.Handlers.Analytics.Statistics
{
    public class VortexHandler
    {
        #region private

        private readonly DateTime _dateFrom;
        private readonly DateTime _dateTo;

        public class VortexData
        {
            public float TotalUsersCount;
            public string TotalUsersCountPercent;

            public float AddtoCartEvents;
            public string AddtoCartEventsPercent;

            public float TotalOrdersCount;
            public string TotalOrdersCountPercent;

            public float PaiedOrdersCount;
            public string PaiedOrdersCountPercent;

            public float ComletedOrdersCount;
            public string ComletedOrdersCountPercent;

            public List<GaOrderSourcesStatistic> Sources;
        }

        #endregion

        public VortexHandler(DateTime dateFrom, DateTime dateTo)
        {
            _dateFrom = dateFrom;
            _dateTo = dateTo;
        }

        public VortexData Execute()
        {
            var data = GoogleAnalyticsService.GetVortexStatistics(_dateFrom, _dateTo);
            if (data == null)
                return new VortexData()
                {
                    TotalUsersCountPercent = "0",
                    AddtoCartEventsPercent = "0",
                    TotalOrdersCountPercent = "0",
                    PaiedOrdersCountPercent = "0",
                    ComletedOrdersCountPercent = "0"
                };

            var vortex = new VortexData()
            {
                TotalUsersCount = data.TotalUsersCount,
                AddtoCartEvents = data.AdvantShopEvents != null && data.AdvantShopEvents.ContainsKey("addToCart")
                    ? data.AdvantShopEvents["addToCart"]
                    : 0,

                TotalOrdersCount = OrderStatisticsService.GetOrdersCount(_dateFrom, _dateTo),
                PaiedOrdersCount = OrderStatisticsService.GetOrdersCount(_dateFrom, _dateTo, true),
                ComletedOrdersCount = OrderStatisticsService.GetOrdersCount(_dateFrom, _dateTo, true, true),
                Sources = data.Sources
            };

            float addtoCartEventsPercent = 0, totalOrdersCountPercent = 0, paiedOrdersCountPercent = 0, comletedOrdersCountPercent = 0;

            if (vortex.TotalUsersCount != 0)
            {
                addtoCartEventsPercent = vortex.AddtoCartEvents*100/vortex.TotalUsersCount;
                totalOrdersCountPercent = vortex.TotalOrdersCount*100/vortex.TotalUsersCount;
                paiedOrdersCountPercent = vortex.PaiedOrdersCount*100/vortex.TotalUsersCount;
                comletedOrdersCountPercent = vortex.ComletedOrdersCount*100/vortex.TotalUsersCount;
            }

            vortex.TotalUsersCountPercent = "100";
            vortex.AddtoCartEventsPercent = addtoCartEventsPercent.ToString("0.###", CultureInfo.InvariantCulture);
            vortex.TotalOrdersCountPercent = totalOrdersCountPercent.ToString("0.###", CultureInfo.InvariantCulture);
            vortex.PaiedOrdersCountPercent = paiedOrdersCountPercent.ToString("0.###", CultureInfo.InvariantCulture);
            vortex.ComletedOrdersCountPercent = comletedOrdersCountPercent.ToString("0.###", CultureInfo.InvariantCulture);
            
            return vortex;
        }
    }
}