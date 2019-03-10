//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Web.UI.WebControls;
using AdvantShop;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Saas;
using AdvantShop.Trial;
using System.Linq;
using System.Text;

namespace Admin
{
    partial class Achievements_Page : AdvantShopAdminPage
    {

        protected bool isLevelUnlock = false;
        protected int LevelId;
        protected int Percent;
        protected int AchievementLastId;
        protected void Page_Load(object sender, EventArgs e)
        {

            Master.AchievementsHelpVisible = false;

            if (!TrialService.IsTrialEnabled && !SaasDataService.IsSaasEnabled)
            {
                Response.Redirect("~/admin/default.aspx");
            }

            SetMeta(string.Format("{0} - {1}", AdvantShop.Configuration.SettingsMain.ShopName, Resources.Resource.Admin_Achievements_Header));

            var achievements = TrialService.GetAchievements();
            if (achievements == null)
                return;

            var lastLevel = achievements.LastOrDefault();

            if (lastLevel != null)
            {
                var lastAchievement = lastLevel.Achievements.LastOrDefault();

                if (lastAchievement != null)
                {
                    AchievementLastId = lastAchievement.Id;
                }
            }

            var headers = achievements.Select(x => new
            {
                x.Id,
                x.Title,
                x.SortOrder,
                x.Unlocked,
                x.Complete
            }).OrderBy(x => x.SortOrder);

            lvLevels.DataSource = headers;
            lvLevels.DataBind();

            lvContentsContainer.DataSource = achievements;
            lvContentsContainer.DataBind();

            Percent = achievements.Sum(level => level.Achievements.Count(ach => ach.Complete)) * 100 / achievements.Sum(level => level.Achievements.Count());
        }

        protected void lvContentsContainer_OnItemDataBound(object sender, ListViewItemEventArgs e)
        {
            var lv = (ListView)e.Item.FindControl("lvAchievements");
            var level = (AchievementLevel)e.Item.DataItem;

            isLevelUnlock = level.Unlocked;
            LevelId = level.Id;

            lv.DataSource = level.Achievements;
            lv.DataBind();

        }


        protected string GetAdditionalLinks(int levelId, int achievementId, string instruction, string recomendation)
        {
            if (instruction.IsNullOrEmpty() && recomendation.IsNullOrEmpty())
            {
                return "";
            }


            var result = new StringBuilder();

            result.Append("<div  class=\"achievements-list-links\">");

            if (instruction.IsNotEmpty())
            {
                result.AppendFormat("<a href=\"javascript:void(0);\" data-achievement-help-id=\"{1}\" data-achievement-level-id=\"{2}\" data-achievement-help-type=\"instruction\" class=\"achievements-list-link js-achievements-help-call\">{0}</a>",
                    Resources.Resource.Admin_Achievements_Instruction,
                    achievementId,
                    levelId);
            }

            //if (recomendation.IsNotEmpty())
            //{
            //    result.AppendFormat("<a href=\"javascript:void(0);\" data-achievement-help-id=\"{1}\" data-achievement-level-id=\"{2}\" data-achievement-help-type=\"recomendation\" class=\"achievements-list-link js-achievements-help-call\">{0}</a>",
            //        Resources.Resource.Admin_Achievements_Recommendation,
            //        achievementId,
            //        levelId);
            //}

            result.Append("</div>");

            return result.ToString();
        }
    }

}