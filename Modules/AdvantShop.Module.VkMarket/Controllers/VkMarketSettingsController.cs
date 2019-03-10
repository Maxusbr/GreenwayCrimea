using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Modules;
using AdvantShop.Core.Services.Vk;
using AdvantShop.Module.VkMarket.Domain;
using AdvantShop.Module.VkMarket.Handlers.Settings;
using AdvantShop.Module.VkMarket.Models.Settings;
using AdvantShop.Module.VkMarket.Services;
using AdvantShop.Repository.Currencies;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Module.VkMarket.Controllers
{
    public class VkMarketSettingsController : ModuleAdminController
    {
        [ChildActionOnly]
        public ActionResult ExportSettings()
        {
            var model = new SettingsModel();

            var module = ModulesService.GetModules().Items.FirstOrDefault(x => x.StringId == VkMarket.ModuleId);
            if (module != null)
                model.Version = module.Version;

            if (model.Version.TryParseFloat() == 0)
                model.Version = "rnd" + new Random().Next(1000);

            return PartialView("~/Modules/VkMarket/Views/Admin/_ExportSettings.cshtml", model);
        }

        
        #region Auth

        [HttpGet]
        public JsonResult GetAuthSettings()
        {
            var vkMarketApiService = new VkMarketApiService();
            var isActive = vkMarketApiService.IsActive();
            if (!isActive)
                InstallUpdateModuleService.Install();

            return Json(new
            {
                IsActive = isActive,
                ApplicationId = VkMarketSettings.ApplicationId,
            });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SaveAuth(string clientId, string accessToken, string userId)
        {
            if (string.IsNullOrWhiteSpace(clientId) ||
                string.IsNullOrWhiteSpace(accessToken) ||
                string.IsNullOrWhiteSpace(userId))
            {
                return JsonError();
            }

            VkMarketSettings.UserId = userId.TryParseLong();
            VkMarketSettings.ApplicationId = clientId;
            VkMarketSettings.AuthToken = accessToken;

            return JsonOk();
        }

        #endregion

        #region Group

        [HttpGet]
        public JsonResult GetGroups()
        {
            var groups = new VkMarketApiService().GetGroups();
            return Json(groups);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SaveGroup(VkGroup group)
        {
            if (group == null || group.Id == 0)
                return JsonError();
            
            VkMarketSettings.Group = group;

            //var settings = new VkMarketApiService().GetGroupSettings(group.Id);
            //return JsonOk(settings);

            return JsonOk(null);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteGroup()
        {
            VkMarketSettings.Group = null;
            return JsonOk();
        }

        #endregion

        #region Categories

        [HttpGet]
        public JsonResult GetCategories()
        {
            return Json(new GetCategories().Execute());
        }

        [HttpGet]
        public JsonResult GetCategory(int id)
        {
            var category = new VkCategoryService().Get(id);
            if (category == null)
                return Json(null);

            var marketCategories = new VkMarketApiService().GetMarketCategories();

            return Json(new CategoryModel(category, marketCategories));
        }

        [HttpGet]
        public JsonResult GetMarketCategories()
        {
            return Json(new VkMarketApiService().GetMarketCategories());
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddCategory(CategoryModel category)
        {
            return ProcessJsonResult(new AddUpdateCategory(category));
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UpdateCategory(CategoryModel category)
        {
            return ProcessJsonResult(new AddUpdateCategory(category));
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Inplace(CategoryModel category)
        {
            return ProcessJsonResult(new AddUpdateCategory(category));
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteCategory(int id, long vkId)
        {
            return ProcessJsonResult(() => new VkCategoryService().Delete(id, vkId));
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteCategories(BaseFilterModel model)
        {
            var categoryService = new VkCategoryService();

            Command(model, (id, c) =>
            {
                var cat = categoryService.Get(id);
                if (cat != null)
                    categoryService.Delete(cat.Id, cat.VkId);
            });
            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteAllProducts()
        {
            new VkProductService().DeleteAllProducts();
            return JsonOk();
        }

        #region Command
        private void Command(BaseFilterModel model, Action<int, BaseFilterModel> func)
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
                var ids = new GetCategories().Execute().DataItems.Select(x => x.Id);
                foreach (int id in ids)
                {
                    if (model.Ids == null || !model.Ids.Contains(id))
                        func(id, model);
                }
            }
        }
        #endregion

        #endregion

        #region Settings

        [HttpGet]
        public JsonResult GetExportSettings()
        {
            var dir = new DirectoryInfo(HostingEnvironment.MapPath(VkMarketExportState.FileLogPath));

            var reports = dir.GetFiles("*.txt").OrderByDescending(x => x.CreationTime).ToList();
            if (reports.Count > 10)
            {
                Task.Run(() =>
                {
                    foreach (var rep in reports.Skip(10))
                        System.IO.File.Delete(rep.FullName);
                });
            }

            return Json(new MarketExportSettingsModel()
            {
                ExportUnavailableProducts = VkMarketExportSettings.ExportUnavailableProducts,
                AddSizeAndColorInDescription = VkMarketExportSettings.AddSizeAndColorInDescription,
                AddSizeAndColorInName = VkMarketExportSettings.AddSizeAndColorInName,
                ShowDescription = (int)VkMarketExportSettings.ShowDescription,
                AddLinkToSite = (int)VkMarketExportSettings.AddLinkToSite,
                TextBeforeLinkToSite = VkMarketExportSettings.TextBeforeLinkToSite ?? "Подробное описание на сайте: ",
                Group = VkMarketSettings.Group,
                CurrencyIso3 = VkMarketSettings.CurrencyIso3 ?? CurrencyService.CurrentCurrency.Iso3,
                ExportOnShedule = VkMarketExportSettings.ExportOnShedule,
                ShowProperties = VkMarketExportSettings.ShowProperties,
                IsExportRun = VkMarketExportState.IsRun,
                Reports = reports.Take(10).Select(x => x.Name).ToList()
            });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SaveExportSettings(MarketExportSettingsModel model)
        {
            VkMarketExportSettings.ExportUnavailableProducts = model.ExportUnavailableProducts;
            VkMarketExportSettings.AddSizeAndColorInDescription = model.AddSizeAndColorInDescription;
            VkMarketExportSettings.AddSizeAndColorInName = model.AddSizeAndColorInName;
            VkMarketExportSettings.ShowDescription = (ShowDescriptionMode)model.ShowDescription;
            VkMarketExportSettings.ShowProperties = model.ShowProperties;
            VkMarketExportSettings.AddLinkToSite = (AddLinkToSiteMode)model.AddLinkToSite;
            VkMarketExportSettings.TextBeforeLinkToSite = model.TextBeforeLinkToSite.DefaultOrEmpty();
            VkMarketSettings.CurrencyIso3 = model.CurrencyIso3;
            
            VkMarketExportSettings.ExportOnShedule = model.ExportOnShedule;
            
            return JsonOk();
        }

        [HttpGet]
        public JsonResult GetReports()
        {
            var dir = new DirectoryInfo(HostingEnvironment.MapPath(VkMarketExportState.FileLogPath));

            var reports = dir.GetFiles("*.txt").OrderByDescending(x => x.CreationTime).ToList();
            if (reports.Count > 10)
            {
                Task.Run(() =>
                {
                    foreach (var rep in reports.Skip(10))
                        System.IO.File.Delete(rep.FullName);
                });
            }

            return Json(new {reports = reports.Take(10).Select(x => x.Name).ToList()});
        }

        #endregion

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Export()
        {
            if (VkMarketExportState.IsRun)
                return JsonError("Перенос товаров уже запущен");

            Task.Run(() => new VkMarketExportService().StartExport());

            return JsonOk();
        }

        public JsonResult GetExportProgress()
        {
            var res = ExportProgress.State();
            return Json(new { Total = res.Item1, Current = res.Item2 });
        }


        #region Import

        [ChildActionOnly]
        public ActionResult ImportSettings()
        {
            var module = ModulesService.GetModules().Items.FirstOrDefault(x => x.StringId == VkMarket.ModuleId);
            var model = new ImportSettingsModel()
            {
                Version = module != null ? module.Version : ""
            };

            return PartialView("~/Modules/VkMarket/Views/Admin/_ImportSettings.cshtml", model);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Import(ImportSettingsModel model)
        {
            return ProcessJsonResult(new ImportProducts(model));
        }

        public JsonResult GetImportProgress()
        {
            var res = ImportProgress.State();
            return Json(new {Total = res.Item1, Current = res.Item2});
        }

        #endregion
    }
}
