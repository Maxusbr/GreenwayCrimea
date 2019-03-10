using System;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.CMS;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Voting;
using AdvantShop.Web.Admin.Models.Voting;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Marketing
{
    public partial class VotingController : BaseAdminController
    {
        [Auth(RoleAction.Marketing)]
        public ActionResult Index()
        {
            SetMetaInformation(T("Admin.Voting.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.VotingCtrl);

            return View();
        }

        #region Get/GetItem
        
        public JsonResult GetVoting(VotingThemeFilterModel model)
        {
            var themes = VoiceService.GetAllVoiceThemes().Where(x => x.Name == "Default").ToList();
            foreach(var theme in themes)
            {
                VoiceService.DeleteTheme(theme.VoiceThemeId);
                foreach(var answer in VoiceService.GetAllAnswers(theme.VoiceThemeId))
                {
                    VoiceService.DeleteAnswer(answer.AnswerId);
                }
            }

            var result = new GetVotingHandler(model).Execute();
            
            return Json(result);
        }

        public JsonResult GetVotingItem(int ID)
        {
            var voting = VoiceService.GetAllVoiceThemes();
            if (voting == null)
            {
                return Json( new { result = false });
            }
            voting = voting.Where(x => x.VoiceThemeId == ID).ToList();
            if(voting.Count == 0 || voting.Count > 1)
            {
                return Json(new { result = false });
            }
            return Json(voting[0]);
        }

        public JsonResult GetAnswers(VotingAnswerFilterModel model)
        {
            var handler = new GetAnswersHandler(model);
            var result = handler.Execute();

            return Json(result);
        }

        public JsonResult GetAnswersItem(int ThemeId, int id)
        {
            if(ThemeId == 0)
            {
                return Json( new { result = false });
            }
            var theme = VoiceService.GetVotingName(ThemeId);
            if(id == 0)
            {
                return Json(new { theme });
            }
            var answers = VoiceService.GetAllAnswers(ThemeId);
            if (answers == null)
            {
                return Json(new { result = false });
            }
            answers = answers.Where(x => x.AnswerId == id).ToList();
            if (answers.Count == 0 || answers.Count > 1)
            {
                return Json(new { result = false });
            }
            return Json( new { answers = answers[0], theme = theme });
        }

        public JsonResult GetVotingList(int ThemeId)
        {
            var themes = VoiceService.GetAllVoiceThemes().Where(x => x.Name == "Default").ToList();
            var theme = themes.Count == 1 ? themes.First() : null;
            if(ThemeId == 0)
            {
                ThemeId = theme != null ? theme.VoiceThemeId : 0;
            }
            var answers = VoiceService.GetAllAnswers(ThemeId);
            return Json(answers);
        }

        #endregion

        #region Add/Edit

        public JsonResult AddVoting(VotingThemeFilterModel model)
        {
            try
            {
                var voting = new VoiceTheme()
                {
                    Name = model.Name,
                    IsHaveNullVoice = model.IsHaveNullVoice != null ? model.IsHaveNullVoice.Value : false,
                    IsClose = model.IsClose != null ? model.IsClose.Value : false,
                    IsDefault = model.IsDefault != null ? model.IsDefault.Value : false
                };
                var themes = VoiceService.GetAllVoiceThemes().Where(x => x.Name == "Default").ToList();
                var theme = themes.Count == 1 ? themes.First() : null;
                if (theme == null)
                {
                    VoiceService.AddTheme(voting);
                }
                else
                {
                    voting.VoiceThemeId = theme.VoiceThemeId;
                    VoiceService.UpdateTheme(voting);
                }
            }
            catch
            {
                return Json( new { result = false });
            }
            return Json( new { result = true });
        }

        public JsonResult EditVoting(VotingThemeFilterModel model)
        {
            if (model.ID == null)
            {
                return Json(new { result = false });
            }

            var voting = VoiceService.GetAllVoiceThemes();
            if (voting == null)
            {
                return Json(new { result = false });
            }
            voting = voting.Where(x => x.VoiceThemeId == (int)model.ID).ToList();
            if (voting.Count == 0 || voting.Count > 1)
            {
                return Json(new { result = false });
            }
            try
            {
                var theme = voting[0];
                theme.VoiceThemeId = model.ID.Value;
                theme.Name = model.Name;
                if (model.PsyID != null) { theme.PsyId = model.PsyID.Value; }
                theme.IsDefault = model.IsDefault != null ? model.IsDefault.Value : false;
                theme.IsClose = model.IsClose != null ? model.IsClose.Value : false;
                theme.IsHaveNullVoice = model.IsHaveNullVoice != null ? model.IsHaveNullVoice.Value : false;
                VoiceService.UpdateTheme(theme);
            }
            catch
            {
                return Json( new { result = false});
            }
            return Json( new { result = true } );
        }

        public JsonResult AddAnswers(VotingAnswerFilterModel model)
        {
            if (model.ID == null || model.ThemeId == null)
            {
                return Json(new { result = false });
            }
            try
            {
                var answer = new Answer()
                {
                    AnswerId = model.ID.Value,
                    FkidTheme = model.ThemeId.Value,
                    Name = model.Name,
                    Sort = model.SortOrder,
                    IsVisible = model.IsVisible != null ? model.IsVisible.Value : false
                };
                VoiceService.InsertAnswer(answer);
            }
            catch
            {
                return Json(new { result = false });
            }
            return Json( new { result = true });
        }

        public JsonResult EditAnswers(VotingAnswerFilterModel model)
        {
            if (model.ID == null || model.ThemeId == null)
            {
                return Json(new { result = false });
            }
            var answers = VoiceService.GetAllAnswers(model.ThemeId.Value);
            if (answers == null || answers.Count == 0)
            {
                return Json(new { result = false });
            }
            answers = answers.Where(x => x.AnswerId == model.ID).ToList();
            if (answers.Count == 0 || answers.Count > 1)
            {
                return Json(new { result = false });
            }
            try
            {
                var answer = answers[0];
                answer.IsVisible = model.IsVisible != null ? model.IsVisible.Value : false;
                answer.Name = model.Name;
                answer.Sort = model.SortOrder;
                VoiceService.UpdateAnswer(answer);
            }
            catch
            {
                
            }
            return Json(new { result = true });
        }

        public JsonResult SetAnswer(string text, int? themeId)
        {
            if(themeId == null)
                return Json( new { result = false });
            
            try
            {
                if(themeId == 0)
                {
                    var themes = VoiceService.GetAllVoiceThemes().Where(x => x.Name == "Default").ToList();
                    if (themes.Count == 1)
                    {
                        themeId = themes[0].VoiceThemeId;
                    }
                    else
                    {
                        VoiceService.AddTheme(new VoiceTheme(){Name = "Default"});
                        themeId = themes.Count > 0 ? themes[0].VoiceThemeId : 0;
                    }
                }
                var answer = new Answer()
                {
                    FkidTheme = themeId.Value,
                    Name = text,
                    Sort = 0,
                    IsVisible = true
                };
                VoiceService.InsertAnswer(answer);
            }
            catch
            {
                return Json( new { result = false });
            }

            return Json( new { result = true });
        }


        #endregion

        #region Delete

        public JsonResult DeleteAnswer(int Id, int ThemeId)
        {
            var answer = VoiceService.GetAllAnswers(ThemeId).Where(x => x.AnswerId == Id).First();
            if (answer == null)
                return Json( new { result = false });
            VoiceService.DeleteAnswer(answer.AnswerId);
            return Json( new { result = true });
        }


        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteVoting(VotingThemeFilterModel model)
        {
            CommandTheme(model, (id, c) =>
            {
                VoiceService.DeleteTheme(id);
                return true;
            });

            return Json(true);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteAnswers(VotingAnswerFilterModel model)
        {
            CommandAnswer(model, (id, c) =>
            {
                VoiceService.DeleteAnswer(id);
                return true;
            });

            return Json(true);
        }

        #endregion

        #region Inplace

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult InplaceVoting(VotingThemeFilterModel model)
        {
            var theme = VoiceService.GetAllVoiceThemes().Where(x => x.VoiceThemeId == model.ID).First();
            if(theme == null)
            {
                return Json( new { result = false });
            }
            if (model.IsClose != null)
                theme.IsClose = model.IsClose.Value;
            if (model.IsDefault != null)
                theme.IsDefault = model.IsDefault.Value;
            if (model.IsHaveNullVoice != null)
                theme.IsHaveNullVoice = model.IsHaveNullVoice.Value;

            VoiceService.UpdateTheme(theme);

            return Json(new { result = true });
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult InplaceAnswers(VotingAnswerFilterModel model)
        {
            if(model.ThemeId == null)
            {
                return Json(new { result = false });
            }
            var answer = VoiceService.GetAllAnswers(model.ThemeId.Value).Where(x => x.AnswerId == model.ID).First();
            if (answer == null)
            {
                return Json(new { result = false });
            }
            if (model.IsVisible != null)
                answer.IsVisible = model.IsVisible.Value;

            VoiceService.UpdateAnswer(answer);

            return Json(new { result = true });
        }


        #endregion

        #region Help methods

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult ChangeSorting(int id, int? ThemeId, int? prevId, int? nextId)
        {
            if(ThemeId == null)
            {
                return Json( new { result = false });
            }
            var handler = new ChangeVotingSorting(id, ThemeId.Value, prevId, nextId);
            var result = handler.Execute();

            return Json(new { result = result });
        }

        public JsonResult GetDiagrem(int ThemeId)
        {
            var theme = VoiceService.GetAllVoiceThemes().Where(x => x.VoiceThemeId == ThemeId).First();
            var result = theme.Answers.Select(x => new {
                Name = x.Name,
                Percent = x.Percent,
                Text = x.CountVoice.ToString() + " голосов(" + x.Percent.ToString() + "%)"
            }).ToList();
            return Json(result);
        }
        
        #endregion

        #region Commands

        private void CommandTheme(VotingThemeFilterModel command, Func<int, VotingThemeFilterModel, bool> func)
        {
            if (command.SelectMode == SelectModeCommand.None)
            {
                foreach (var id in command.Ids)
                {
                    func(id, command);
                }
            }
            else
            {
                var handler = new GetVotingHandler(command);
                var ids = handler.GetItemsIds();

                foreach (int id in ids)
                {
                    if (command.Ids == null || !command.Ids.Contains(id))
                        func(id, command);
                }
            }
        }

        private void CommandAnswer(VotingAnswerFilterModel command, Func<int, VotingAnswerFilterModel, bool> func)
        {
            if (command.SelectMode == SelectModeCommand.None)
            {
                foreach (var id in command.Ids)
                {
                    func(id, command);
                }
            }
            else
            {
                var handler = new GetAnswersHandler(command);
                var ids = handler.GetItemsIds();

                foreach (int id in ids)
                {
                    if (command.Ids == null || !command.Ids.Contains(id))
                        func(id, command);
                }
            }
        }

        #endregion
    }
}
