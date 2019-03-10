//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using AdvantShop.Configuration;
using AdvantShop.Core.Controls;
using AdvantShop.Helpers;
using AdvantShop.CMS;
using Resources;

namespace Admin
{
    public partial class VotingHistory : AdvantShopAdminPage
    {
        private int countThemes
        {
            get
            {
                if (ViewState["countThemes"] != null)
                {
                    return SQLDataHelper.GetInt(ViewState["countThemes"]);
                }
                ViewState["countThemes"] = 1;
                return 1;
            }

            set
            {
                ViewState["countThemes"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SetMeta(string.Format("{0} - {1}", SettingsMain.ShopName, Resource.Admin_VotingHistory_VotingHistory));

            if (!IsPostBack)
            {
                countThemes = VoiceService.GetThemeIDs().Count;
            }
        }

        protected string GetPagesIndex()
        {
            string retVal = string.Empty;
            byte countRow = 10;
            try
            {
                int countPage = (countThemes / countRow) % 1;
                if (countThemes % countRow != 0)
                {
                    countPage++;
                }
                if (countPage > 1)
                {
                    int page = -1;
                    Int32.TryParse(Request["page"], out page);
                    for (int i = 1; i <= countPage; i++)
                    {
                        if ((page == -1 && i == 1) || (page == i))
                        {
                            retVal += "<b>" + i + "</b>&nbsp;";
                        }
                        else
                        {
                            retVal += "<a class=\"Link\" href=\"votinghistory.aspx?page=" + i + "\">" + i + "</a>&nbsp;";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AdvantShop.Diagnostics.Debug.Log.Error(ex);
            }
            return retVal;
        }

        protected string GetHtmlTableVoiceThemes()
        {
            List<VoiceTheme> voiceThemes = VoiceService.GetAllVoiceThemes();
            if (voiceThemes.Count > 0)
            {
                var sb = new StringBuilder();
                foreach (var voiceTheme in voiceThemes)
                {
                    sb.Append("<div style='border: 1px solid #a9a9a9; width: 99%; margin: 10px'>");
                    sb.Append("<table border='0' cellpadding='3px' cellspacing='0' style='width: 100%; height: 10px;'>");

                    sb.Append("<tr>");
                    sb.Append("<td align='center'>");
                    sb.Append(GetHtmlThemeName(voiceTheme));
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append("<tr>");
                    sb.Append("<td>");
                    sb.Append(GetHtmlThemeTable(voiceTheme));
                    sb.Append("</td>");
                    sb.Append("</tr>");

                    sb.Append("</table>");
                    sb.Append("</div>");
                }

                return sb.ToString();
            }

            return "";
        }

        protected string GetHtmlThemeName(VoiceTheme voiceTheme)
        {
            string str = voiceTheme.IsDefault
                             ? "<span class=\"ThemeNameDefault\">" + voiceTheme.Name + "</span> "
                             : "<span class=\"ThemeName\">" + voiceTheme.Name + "</span> ";

            if (voiceTheme.IsClose)
            {
                str += " " + Resources.Resource.Admin_VotingHistory_ClosedWithBrakets;
            }

            return str;
        }

        protected string GetHtmlThemeTable(VoiceTheme voiceTheme)
        {
            var sb = new StringBuilder();
            sb.Append("<table border=\"0\" cellpadding=\"3px\" cellspacing=\"0\" style=\"width: 100%; height: 100%;\">");

            foreach (var answer in voiceTheme.Answers)
            {
                sb.Append(GetHtmlAnswerRow(answer));
            }

            sb.Append("</table>");
            return sb.ToString();
        }

    
        protected string GetHtmlAnswerRow(Answer answer)
        {
            string str = "";

            if (answer.CountVoice != 0)
            {
                str += "<tr>" +
                       "<td style=\"width: 10px\">" +
                       "<span class=\"NameAnswer\">" + answer.Name + "</span>" +
                       "</td>" +

                       "<td style=\"width: 95%\">" +
                       "<div style=\"width: " + answer.Percent + "%\" class=\"barAnswer\"></div>" +
                       "</td>" +

                       "<td>" +
                       answer.Percent + "%" +
                       "</td>" +
                       "</tr>" +
                       "<tr>" +
                       "<td align=\"center\" colspan=\"3\">" +
                       Resources.Resource.Admin_VotingHistory_Votes + " " + answer.CountVoice +
                       "</td>" +
                       "</tr>";
            }
            else
            {
                str += "<tr>" +
                       "<td align=\"center\">" +
                       Resources.Resource.Admin_VotingHistory_NoAnswers +
                       "</td>" +
                       "</tr>" +
                       "<tr>" +
                       "<td align=\"center\">" +
                       Resources.Resource.Admin_VotingHistory_Votes + " 0" +
                       "</td>" +
                       "</tr>";
            }

            return str;
        }
    }
}
