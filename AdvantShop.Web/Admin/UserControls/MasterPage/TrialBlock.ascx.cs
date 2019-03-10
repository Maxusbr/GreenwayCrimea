using System;
using System.Web.UI;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Trial;
using AdvantShop.Helpers;
using AdvantShop.Configuration;


namespace Admin.UserControls.MasterPage
{
    public partial class TrialBlock : UserControl
    {
        public bool ForAdmin { get; set; }

        protected string achievementsPopupMessage = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!TrialService.IsTrialEnabled)
            {
                this.Visible = false;
                return;
            }

            pnlInfo.Visible = ForAdmin;

            lDate.Text = TrialService.TrialPeriodTill.ToShortDateString();

            //todo временно убираем popup
            //achievementsPopup.Visible = false;
            //return;
            
            if (ForAdmin)
            {
                bool? isAchievementsPopupShow = CommonHelper.GetCookieString("AchievementsPopupShow").TryParseBool(true);

                if (isAchievementsPopupShow != true)
                {
                    achievementsPopupMessage = SettingsMain.AchievementsPopUp;

                    if (achievementsPopupMessage.IsNotEmpty())
                    {
                        CommonHelper.SetCookie("AchievementsPopupShow", true.ToString(), true);
                    }

                    achievementsPopup.Visible = achievementsPopupMessage.IsNotEmpty();
                }
                else
                {
                    achievementsPopup.Visible = false;
                }
            }
        }
    }
}