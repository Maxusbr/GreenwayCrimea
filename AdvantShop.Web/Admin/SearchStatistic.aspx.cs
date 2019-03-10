//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using AdvantShop.Configuration;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Services.Statistic;

namespace Admin
{
    public partial class SearchStatistic : AdvantShopAdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = string.Format("{0} - {1}", SettingsMain.ShopName, Resources.Resource.Admin_SearchStatistic_Header);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (Request["view"] == "frequency")
            {

                DateTime date = DateTime.Now;
                switch (Request["span"])
                {
                    case "day":
                        date = DateTime.Now;
                        lblDisplayMode.Text = Resources.Resource.Admin_SearchStatistic_StatisticPerDay;
                        break;
                    case "week":
                        date = DateTime.Now.AddDays(-7);
                        lblDisplayMode.Text = Resources.Resource.Admin_SearchStatistic_StatisticForWeek;
                        break;
                    case "mounth":
                        date = DateTime.Now.AddDays(-30);
                        lblDisplayMode.Text = Resources.Resource.Admin_SearchStatistic_StatisticForMonth;
                        break;
                }

                pnlFrequency.Visible = true;
                pnlHistory.Visible = false;
                btnFrequency.Enabled = false;
                btnHistory.Enabled = true;

                gridFrequency.DataSource = StatisticService.GetFrequencySearchStatistic(date);
                gridFrequency.DataBind();
            }
            else
            {
                int rowsNumber;
                if (!Int32.TryParse(Request["rows"], out rowsNumber))
                {
                    rowsNumber = 10;
                }
                lblDisplayMode.Text = string.Format(Resources.Resource.Admin_SearchStatistic_LastQ, rowsNumber);

                pnlHistory.Visible = true;
                pnlFrequency.Visible = false;
                btnHistory.Enabled = false;
                btnFrequency.Enabled = true;

                gridHistory.DataSource = StatisticService.GetHistorySearchStatistic(rowsNumber);
                gridHistory.DataBind();
            }
        }
    }
}