using System;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Areas.AdminMobile.Models.Attendance;
using AdvantShop.Configuration;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.SEO;
using Resources;

namespace AdvantShop.Areas.AdminMobile.Controllers
{
    public class AttendanceController : BaseAdminMobileController
    {
        // GET: AdminMobile/Attendance
        public ActionResult Index()
        {
            var model = new AttendanceModel();

            if (!SettingsSEO.GoogleAnalyticsApiEnabled)
                return View(model);
            
            var data = GoogleAnalyticsService.GetData();
            if (data == null)
                return View(model);

            model.ChartData =
                string.Format(
                    "[{{label: '{0}', data:[{1}]}}, {{label: '{2}', data:[{3}]}}, {{label: '{4}', data:[{5}]}}]",
                    Resource.Admin_Statistics_PageViews, data.Select((pair, valuePair) => string.Format("[{0}, {1}]", GetTimestamp(pair.Key), pair.Value.PageViews)).AggregateString(','),
                    Resource.Admin_Statistics_Visits, data.Select((pair, valuePair) => string.Format("[{0}, {1}]", GetTimestamp(pair.Key), pair.Value.Visits)).AggregateString(','),
                    Resource.Admin_Statistics_Visitors, data.Select((pair, valuePair) => string.Format("[{0}, {1}]", GetTimestamp(pair.Key), pair.Value.Visitors)).AggregateString(',')
                   );

            if (data.ContainsKey(DateTime.Now.Date))
            {
                model.PagesToday = data[DateTime.Now.Date].PageViews;
                model.VisitsToday = data[DateTime.Now.Date].Visits;
                model.VisitorsToday = data[DateTime.Now.Date].Visitors;
            }
            if (data.ContainsKey(DateTime.Now.AddDays(-1).Date))
            {
                model.PagesYesterday = data[DateTime.Now.AddDays(-1).Date].PageViews;
                model.VisitsYesterday = data[DateTime.Now.AddDays(-1).Date].Visits;
                model.VisitorsYesterday = data[DateTime.Now.AddDays(-1).Date].Visitors;
            }

            model.Min = GetTimestamp(DateTime.Now.AddDays(-7)).ToString();
            model.Max = GetTimestamp(DateTime.Now).ToString();

            SetMetaInformation(Resource.AdminMobile_Attendance);

            return View(model);
        }

        private static long GetTimestamp(DateTime date)
        {
            TimeSpan span = (date - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
            return (long)(span.TotalSeconds * 1000);
        }

    }
}