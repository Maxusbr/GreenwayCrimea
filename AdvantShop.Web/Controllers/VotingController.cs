using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using AdvantShop.CMS;
using AdvantShop.Configuration;
using AdvantShop.Core.Modules;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;

namespace AdvantShop.Controllers
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    public partial class VotingController : BaseClientController
    {
        private readonly string _cookieCollectionNameVoting = HttpUtility.UrlEncode(SettingsMain.SiteUrl) + "_Voting";

        [ChildActionOnly]
        public ActionResult VotingBlock()
        {
            var theme = VoiceService.GetTopTheme();
            if (!SettingsDesign.VotingVisibility || theme== null || !theme.Answers.Any())
                return new EmptyResult();

            return PartialView();
        }

        [HttpPost]
        public JsonResult Vote(int answerId)
        {
            bool voteAdded = false;
            var currentTheme = VoiceService.GetTopTheme();

            if (answerId != 0 && currentTheme != null)
            {
                if (VoiceService.GetAllAnswers(currentTheme.VoiceThemeId).Any(answer => answer.AnswerId == answerId))
                {
                    if (Request.Browser.Cookies)
                    {
                        var items = CommonHelper.GetCookieCollection(_cookieCollectionNameVoting)
                                    ?? new NameValueCollection();

                        if (items["ThemesID" + currentTheme.VoiceThemeId] == null)
                        {
                            items.Add("ThemesID" + currentTheme.VoiceThemeId, answerId.ToString("G"));
                            CommonHelper.SetCookieCollection(_cookieCollectionNameVoting, items, new TimeSpan(365, 0, 0, 0));
                            VoiceService.AddVote(answerId);
                            voteAdded = true;

                            ModulesExecuter.Vote();
                        }
                    }
                }
            }
            else if (answerId == 0 && Request.Browser.Cookies)
            {
                var items = CommonHelper.GetCookieCollection(_cookieCollectionNameVoting)
                                    ?? new NameValueCollection();
                items.Add("ThemesID" + currentTheme.VoiceThemeId, answerId.ToString("G"));
                CommonHelper.SetCookieCollection(_cookieCollectionNameVoting, items, new TimeSpan(365, 0, 0, 0));
                voteAdded = true;
            }

            return Json(new { voteAdded });
        }

        [HttpPost]
        public JsonResult GetVotingData()
        {
            var theme = VoiceService.GetTopTheme();
            List<Answer> answers;
            if ((theme == null) || (!(answers = theme.Answers).Any() && !theme.IsHaveNullVoice))
            {
                return Json(null);
            }

            int userAnswerId = -1;
            try
            {
                if (Request.Browser.Cookies)
                {
                    var items = CommonHelper.GetCookieCollection(_cookieCollectionNameVoting);
                    if (items != null && items[string.Format("ThemesID{0}", theme.VoiceThemeId)] != null)
                    {
                        userAnswerId = Int32.Parse(items[string.Format("ThemesID{0}", theme.VoiceThemeId)]);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            var vote = new
            {
                Question = theme.Name,
                Answers = answers.Select(item => new { Text = item.Name, item.AnswerId }).ToList(),
                Result = new
                {
                    Rows = answers.Select(item => new { Text = item.Name, Value = item.Percent, Selected = item.AnswerId == userAnswerId }).ToList(),
                    Count = theme.CountVoice
                },
                isVoted = userAnswerId != -1 || theme.IsClose,
                theme.IsHaveNullVoice
            };
            
            return Json(vote);
        }
    }
}