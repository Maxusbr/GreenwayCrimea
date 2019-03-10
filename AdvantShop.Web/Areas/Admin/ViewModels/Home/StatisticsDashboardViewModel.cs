namespace AdvantShop.Web.Admin.ViewModels.Home
{
    public class StatisticsDashboardViewModel
    {
        public bool AnyActive { get; set; }

        public int TotalProductsCount { get; set; }
        public bool ShowTotalProducts { get; set; }

        // Orders
        public int TotalOrdersTodayCount { get; set; }
        public string TotalOrdersTodayPrice { get; set; }
        public bool ShowTotalOrdersToday { get; set; }


        public int TotalOrdersYesterdayCount { get; set; }
        public string TotalOrdersYesterdayPrice { get; set; }
        public bool ShowTotalOrdersYesterday { get; set; }


        public int TotalOrdersMonthCount { get; set; }
        public string TotalOrdersMonthPrice { get; set; }
        public bool ShowTotalOrdersMonth { get; set; }


        public int TotalOrdersAllTimeCount { get; set; }
        public string TotalOrdersAllTimePrice { get; set; }
        public bool ShowTotalOrdersAllTime { get; set; }


        // Leads
        public int TotalLeadsTodayCount { get; set; }
        public string TotalLeadsTodayPrice { get; set; }
        public bool ShowTotalLeadsToday { get; set; }


        public int TotalLeadsYesterdayCount { get; set; }
        public string TotalLeadsYesterdayPrice { get; set; }
        public bool ShowTotalLeadsYesterday { get; set; }


        public int TotalLeadsMonthCount { get; set; }
        public string TotalLeadsMonthPrice { get; set; }
        public bool ShowTotalLeadsMonth { get; set; }


        // Reviews
        public int TotalReviewsTodayCount { get; set; }
        public bool ShowTotalReviewsToday { get; set; }


        public int TotalReviewsYesterdayCount { get; set; }
        public bool ShowTotalReviewsYesterday { get; set; }


        public int TotalReviewsMonthCount { get; set; }
        public bool ShowTotalReviewsMonth { get; set; }


        // Calls
        public int TotalCallsTodayCount { get; set; }
        public bool ShowTotalCallsToday { get; set; }


        public int TotalCallsYesterdayCount { get; set; }
        public bool ShowTotalCallsYesterday { get; set; }


        public int TotalCallsMonthCount { get; set; }
        public bool ShowTotalCallsMonth { get; set; }

        public bool TelephonyConfigured { get; set; }

    }
}
