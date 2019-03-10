using System;
using System.Web.Mvc;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Repository;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Settings;
using AdvantShop.Web.Admin.Handlers.Settings.SearchSettings;
using AdvantShop.Web.Admin.Models.Settings;
using AdvantShop.Web.Infrastructure.ActionResults;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Settings
{
    [Auth(RoleAction.Settings)]
    public partial class SettingsSearchController : BaseAdminController
    {
        public ActionResult Index()
        {
            SetMetaInformation("Поиск настроек");
            SetNgController(NgControllers.NgControllersTypes.SettingsSearchCtrl);

            return View();
        }

        #region Add/Edit/Get/Delete

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddSettingsSearch(SettingsSearch model)
        {
            if (string.IsNullOrEmpty(model.Title) || string.IsNullOrEmpty(model.Link))
                return Json(new CommandResult() { Result = false, Error = "Обязательные поля не заполнены" });

            try
            {
                var settingsSearch = new SettingsSearch()
                {
                    Title = model.Title,
                    Link = model.Link,
                    KeyWords = model.KeyWords,
                    SortOrder = model.SortOrder
                };

                SettingsSearchService.Add(settingsSearch);
            }
            catch (Exception ex)
            {
                Debug.Log.Error("", ex);
                return Json(new CommandResult() { Result = false, Error = ex.Message });
            }

            return Json(new CommandResult() { Result = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult EditSettingsSearch(SettingsSearch model)
        {
            if (string.IsNullOrEmpty(model.Title) || string.IsNullOrEmpty(model.Link))
                return Json(new CommandResult() { Result = false, Error = "Обязательные поля не заполнены" });

            try
            {
                var settingsSearch = new SettingsSearch()
                {
                    Id = model.Id,
                    Title = model.Title,
                    Link = model.Link,
                    KeyWords = model.KeyWords,
                    SortOrder = model.SortOrder
                };

                SettingsSearchService.Update(settingsSearch);
            }
            catch (Exception ex)
            {
                Debug.Log.Error("", ex);
                return Json(new CommandResult() { Result = false, Error = ex.Message });
            }

            return Json(new CommandResult() { Result = true});
        }

        public JsonResult GetSettingsSearchItem(int id)
        {
            var settingsSearch = SettingsSearchService.GetSettingsSearch(id);

            return Json(settingsSearch);
        }


        public JsonResult GetsettingsSearch(SettingsSearchModel model)
        {
            var hendler = new GetSettingsSearch(model);
            var result = hendler.Execute();
            
            return Json(result);
        }


        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteSettingsSearch(int id)
        {
            SettingsSearchService.Delete(id);
            return Json(true);
        }

        #endregion


        #region Inplace

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult InplaceSettingsSearch(SettingsSearchModel model)
        {
            var settingsSearch = SettingsSearchService.GetSettingsSearch(model.Id);

           
            settingsSearch.Title = model.Title;
            settingsSearch.Link = model.Link;
            settingsSearch.KeyWords = model.KeyWords;
            settingsSearch.SortOrder = model.SortOrder;

            SettingsSearchService.Update(settingsSearch);

            return Json(new { result = true });
        }

        #endregion

        #region Commands
        private void Command(SettingsSearchModel model, Func<int, SettingsSearchModel, bool> func)
        {
            if (model.SelectMode == SelectModeCommand.None)
            {
                foreach (var id in model.Ids)
                {
                    func(id, model);
                }
            }
            else
            {
                var handler = new GetSettingsSearch(model);
                var ids = handler.GetItemsIds();

                foreach (int id in ids)
                {
                    if (model.Ids == null || !model.Ids.Contains(id))
                        func(id, model);
                }
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteSettingsSearches(SettingsSearchModel model)
        {
            Command(model, (id, c) =>
            {
                SettingsSearchService.Delete(id);
                return true;
            });

            return Json(true);
        }

        #endregion

    }
}
