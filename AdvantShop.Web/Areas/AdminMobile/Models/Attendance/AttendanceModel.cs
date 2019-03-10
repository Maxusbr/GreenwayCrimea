namespace AdvantShop.Areas.AdminMobile.Models.Attendance
{
    public class AttendanceModel
    {
        public int PagesToday { get; set; }

        public int VisitorsToday { get; set; }

        public int VisitsToday { get; set; }

        public int PagesYesterday { get; set; }

        public int VisitorsYesterday { get; set; }

        public int VisitsYesterday { get; set; }

        public string ChartData { get; set; }

        public string Min { get; set; }
        public string Max { get; set; }
    }
}