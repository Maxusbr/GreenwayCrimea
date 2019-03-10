using System;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Controls;
using AdvantShop.Customers;
using AdvantShop.Repository.Currencies;
using AdvantShop.Saas;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Settings.Catalog;
using AdvantShop.Web.Admin.Models.Settings;
using AdvantShop.Web.Admin.Models.Settings.CatalogSettings;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;
using AdvantShop.FullSearch;

namespace AdvantShop.Web.Admin.Controllers.Settings
{
    [Auth(RoleAction.Settings)]
    public partial class SettingsCatalogController : BaseAdminController
    {
        public ActionResult Index()
        {
            var model = new GetCatalogSettings().Execute();

            SetMetaInformation(T("Admin.Settings.Catalog.CatalogTitle"));
            SetNgController(NgControllers.NgControllersTypes.SettingsCatalogCtrl);

            return View("Index", model);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(CatalogSettingsModel model)
        {
            if (ModelState.IsValid)
            {
                new SaveCatalogSettingsHandler(model).Execute();
                ShowMessage(NotifyType.Success, T("Admin.Settings.SaveSuccess"));
            }
            
            ShowErrorMessages();

            return RedirectToAction("Index");
        }


        #region Currencies 

        public JsonResult GetCurrenciesPaging(CurrencyFilterModel model)
        {
            return Json(new GetCurrenciesHandler(model).Execute());
        }

        [HttpPost]
        public JsonResult UpdateCb()
        {
            CurrencyService.UpdateCurrenciesFromCentralBank();
            return JsonOk(true);
        }

        [HttpPost]
        public JsonResult ReindexLucene()
        {
            LuceneSearch.CreateAllIndex();
            return JsonOk(true);
        }


        #region Inplace

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult CurrencyInplace(Currency model)
        {
            var dbModel = CurrencyService.GetCurrency(model.CurrencyId);
            if (dbModel == null)
                return Json(new { result = false });

            dbModel.Name = model.Name;
            dbModel.Rate = model.Rate;
            dbModel.EnablePriceRounding = model.RoundNumbers != -1;
            dbModel.IsCodeBefore = model.IsCodeBefore;
            dbModel.Iso3 = model.Iso3;
            dbModel.NumIso3 = model.NumIso3;
            dbModel.RoundNumbers = model.RoundNumbers;
            dbModel.Symbol = model.Symbol;

            CurrencyService.UpdateCurrency(dbModel);

            return JsonOk();
        }

        #endregion

        #region Commands

        private void Command(CurrencyFilterModel command, Func<int, CurrencyFilterModel, bool> func)
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
                var ids = new GetCurrenciesHandler(command).GetItemsIds("CurrencyId");

                foreach (int id in ids)
                {
                    if (command.Ids == null || !command.Ids.Contains(id))
                        func(id, command);
                }
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteItems(CurrencyFilterModel command)
        {
            Command(command, (id, c) =>
            {
                CurrencyService.DeleteCurrency(id);
                return true;
            });
            return JsonOk();
        }

        #endregion

        #region CRUD 

        public JsonResult GetCurrency(int id)
        {
            var dbModel = CurrencyService.GetCurrency(id);
            return Json(dbModel);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddCurrency(Currency model)
        {
            if (model.Name.IsNullOrEmpty())
                return Json(new { result = false });

            model.EnablePriceRounding = model.RoundNumbers != -1;

            CurrencyService.InsertCurrency(model);

            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UpdateCurrency(Currency model)
        {
            if (model.Name.IsNullOrEmpty())
                return JsonError();

            var dbModel = CurrencyService.GetCurrency(model.CurrencyId);
            if (dbModel == null)
                return JsonError();

            dbModel.Name = model.Name;
            dbModel.Rate = model.Rate;
            dbModel.EnablePriceRounding = model.RoundNumbers != -1;
            dbModel.IsCodeBefore = model.IsCodeBefore;
            dbModel.Iso3 = model.Iso3;
            dbModel.NumIso3 = model.NumIso3;
            dbModel.RoundNumbers = model.RoundNumbers;
            dbModel.Symbol = model.Symbol;

            CurrencyService.UpdateCurrency(dbModel);

            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteCurrency(int id)
        {
            CurrencyService.DeleteCurrency(id);
            return JsonOk();
        }

        #endregion

        #endregion
    }
}
