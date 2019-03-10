//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;

namespace AdvantShop.SEO
{
    public struct GoogleAnalyticsData
    {
        public int Visitors;
        public int Visits;
        public int PageViews;
    }

    public class GaVortexStatistic
    {
        public int TotalUsersCount { get; set; }
        public Dictionary<string, int> AdvantShopEvents { get; set; }

        public List<GaOrderSourcesStatistic> Sources { get; set; } 
    }

    public class GaOrderSourcesStatistic
    {
        public string Source { get; set; }
        public string Medium { get; set; }
        public string Transactions { get; set; }
    }

    public class GaOrderSourceData
    {
        public string Source { get; set; }
        public string Medium { get; set; }
        public string Campaign { get; set; }
        public string ReferalPath { get; set; }
        public string Revenue { get; set; }
    }
}