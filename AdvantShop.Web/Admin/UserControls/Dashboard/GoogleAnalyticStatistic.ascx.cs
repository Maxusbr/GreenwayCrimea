//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Linq;
using AdvantShop;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.SEO;

namespace Admin.UserControls.Dashboard
{
    public partial class Admin_UserControls_GoogleAnalyticStatistic : System.Web.UI.UserControl
    {
        protected static string chartData = "[{data:[[1,0],[3,4]]}, {data:[[1,1],[0,5]]}]";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!SettingsSEO.GoogleAnalyticsApiEnabled)
            {
                return;
            }
            
            var data = GoogleAnalyticsService.GetData();
            if (data == null)
            {
                return;
            }

            chartData =
                string.Format(
                    "[{{label: '{0}', data:[{1}]}}, {{label: '{2}', data:[{3}]}}, {{label: '{4}', data:[{5}]}}]",
                    Resources.Resource.Admin_Statistics_PageViews, data.Select((pair, valuePair) => string.Format("[{0}, {1}]", GetTimestamp(pair.Key), pair.Value.PageViews)).AggregateString(','),
                    Resources.Resource.Admin_Statistics_Visits, data.Select((pair, valuePair) => string.Format("[{0}, {1}]", GetTimestamp(pair.Key), pair.Value.Visits)).AggregateString(','),
                    Resources.Resource.Admin_Statistics_Visitors, data.Select((pair, valuePair) => string.Format("[{0}, {1}]", GetTimestamp(pair.Key), pair.Value.Visitors)).AggregateString(',')
                   );

            if (data.ContainsKey(DateTime.Now.Date))
            {
                lblViewPagesToday.Text = data[DateTime.Now.Date].PageViews.ToString();
                lblVisitsToday.Text = data[DateTime.Now.Date].Visits.ToString();
                lblVisitorsToday.Text = data[DateTime.Now.Date].Visitors.ToString();
            }
            if (data.ContainsKey(DateTime.Now.AddDays(-1).Date))
            {
                lblViewPagesYesterday.Text = data[DateTime.Now.AddDays(-1).Date].PageViews.ToString();
                lblVisitsYesterday.Text = data[DateTime.Now.AddDays(-1).Date].Visits.ToString();
                lblVisitorsYesterday.Text = data[DateTime.Now.AddDays(-1).Date].Visitors.ToString();
            }
            if (GoogleAnalyticsService.GetLastModifiedDate() != DateTime.MinValue)
            {
                lDate.Text = string.Format(Resources.Resource.Admin_Statistics_Date, GoogleAnalyticsService.GetLastModifiedDate().ToString(SettingsMain.AdminDateFormat));
            }

        }
        public static long GetTimestamp(DateTime date)
        {
            TimeSpan span = (date - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            return (long)(span.TotalSeconds * 1000);
        }
    }
}