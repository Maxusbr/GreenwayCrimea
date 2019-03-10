//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Web.UI;
using AdvantShop.Statistic;

namespace Tools.core
{
    public partial class AppRestartLog : Page
    {
        protected void ShowLogButton_Click(object sender, EventArgs e)
        {
            LogRepeater.DataSource = InternalServices.GetAppRestartLogData();
            LogRepeater.DataBind();

            dataBlock.Visible = true;
        }

        protected void DeleteLogButton_Click(object sender, EventArgs e)
        {
            InternalServices.DeleteAppRestartLogData();
        }
    }
}