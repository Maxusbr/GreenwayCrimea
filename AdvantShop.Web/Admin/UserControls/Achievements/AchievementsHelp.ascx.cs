using AdvantShop.Customers;
using AdvantShop.Saas;
using AdvantShop.Trial;
using System;

namespace Admin.UserControls
{
    public partial class AchievementsHelp : System.Web.UI.UserControl
    {
        public enum eAchievementsHelpType
        {
            Client = 0,
            Admin = 1
        };

        public eAchievementsHelpType AchievementsHelpType { get; set; }
        public bool isVisible { get; set; }

        public AchievementsHelp()
        {
            AchievementsHelpType = eAchievementsHelpType.Client;
            isVisible = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Visible = (TrialService.IsTrialEnabled || (SaasDataService.IsSaasEnabled && CustomerContext.CurrentCustomer.IsAdmin)) && isVisible;
        }
    }
}