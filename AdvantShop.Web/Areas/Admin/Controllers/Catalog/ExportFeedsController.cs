using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;
using AdvantShop.ExportImport;
using AdvantShop.Statistic;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.ExportFeeds;
using AdvantShop.Web.Admin.Models.Catalog;
using AdvantShop.Web.Admin.Models.ExportFeeds;
using AdvantShop.Web.Infrastructure.ActionResults;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Catalog
{
    [Auth(RoleAction.Catalog)]
    public partial class ExportFeedsController : BaseAdminController
    {
        public ActionResult Index(int? id)
        {
            var model = new GetExportFeedsHandler(id, null).Execute();

            SetMetaInformation(T("Экспорт товаров"));
            SetNgController(NgControllers.NgControllersTypes.ExportFeedsCtrl);

            return View(model);
        }

        [SaasFeature(Saas.ESaasProperty.HaveExportFeeds)]
        public ActionResult IndexYandex(int? id)
        {
            SetMetaInformation(T("Выгрузка в Яндекс.Маркет"));
            SetNgController(NgControllers.NgControllersTypes.ExportFeedsCtrl);

            return View("index", new GetExportFeedsHandler(id, EExportFeedType.YandexMarket).Execute());
        }

        [SaasFeature(Saas.ESaasProperty.HaveExportFeeds)]
        public ActionResult IndexGoogle(int? id)
        {
            SetMetaInformation(T("Выгрузка в Google Merchant Center"));
            SetNgController(NgControllers.NgControllersTypes.ExportFeedsCtrl);

            return View("index", new GetExportFeedsHandler(id, EExportFeedType.GoogleMerchentCenter).Execute());
        }


        public ActionResult IndexReseller(int? id)
        {
            SetNgController(NgControllers.NgControllersTypes.ExportFeedsCtrl);

            return View("index", new GetExportFeedsHandler(id, EExportFeedType.Reseller).Execute());
        }

        public ActionResult Export(int id)
        {
            SetNgController(NgControllers.NgControllersTypes.ExportFeedsCtrl);

            var exportFeed = ExportFeedService.GetExportFeed(id);
            if (exportFeed == null)
            {
                return View("Index");
            }

            var model = new GetExportFeedHandler(id).Execute();

            if (CommonStatistic.IsRun)
            {
                return View("ExportFeedProgress", model);
            };

            CommonStatistic.Init();
            CommonStatistic.IsRun = true;
            CommonStatistic.CurrentProcess = "exportfeeds/export/" + id;
            CommonStatistic.CurrentProcessName = exportFeed.Name;

            CommonStatistic.RowPosition = 0;

            var filePath = string.Empty;
            CommonStatistic.StartNew(() =>
                {
                    filePath = new StartingExportHandler(id).Execute();
                    CommonStatistic.FileName = "../" + filePath;
                });

            SetMetaInformation(exportFeed.Name);

            return View("ExportFeedProgress", model);
        }

        public JsonResult SaveExportFeedSettings(int exportFeedId, string exportFeedName, string exportFeedDescription, ExportFeedSettingsModel commonSettings, string advancedSettings)
        {
            return Json(new SaveExportFeedSettingsHandler(exportFeedId, exportFeedName, exportFeedDescription, commonSettings, advancedSettings).Execute());
        }

        public JsonResult CategoriesTree(CategoriesTree model, int exportFeedId)
        {
            return Json(new GetCategoriesTree(model, exportFeedId).Execute());
        }

        public JsonResult SaveExportFeedFields(int exportFeedId, List<string> exportFeedFields)
        {
            var result = new SaveExportFeedFields(exportFeedId, exportFeedFields).Execute();
            return Json(new { result = result });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Add(string name, string description, EExportFeedType type)
        {
            var handler = new AddExportFeed(name, description, type);
            var result = handler.Execute();
            return Json(new { id = result });
        }

        [ChildActionOnly]
        public ActionResult GetAdvansedSettings(int exportFeedId, EExportFeedType exportFeedType, string advancedSettings)
        {
            var handler = new GetExportFeedAdvancedSettings(exportFeedId, exportFeedType, advancedSettings);
            var objectAdvancedSettings = handler.Execute();
            return PartialView("ExportFeedSettings" + exportFeedType.ToString(), objectAdvancedSettings);
        }

        [ChildActionOnly]
        public ActionResult GetExportFeedFields(int exportFeedId, EExportFeedType exportFeedType, string advancedSettings)
        {
            var handler = new GetExportFeedFields(exportFeedId, exportFeedType, advancedSettings);
            var objectAdvancedSettings = handler.Execute();
            return PartialView("СhoiceOfFields", objectAdvancedSettings);
        }


        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddCategoriesToExport(int exportFeedId, List<int> categories)
        {
            ExportFeedService.InsertCategories(exportFeedId, categories);
            return Json(new { result = true, reloadCatalogTree = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteExport(int exportFeedId)
        {
            ExportFeedService.DeleteExportFeed(exportFeedId);
            return Json(new CommandResult { Result = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult GetAvalableTypes(EExportFeedType? param)
        {
            var typesList = new Dictionary<string, string>();
            if (param.HasValue)
            {
                if (Saas.SaasDataService.IsSaasEnabled && !Saas.SaasDataService.CurrentSaasData.HaveExportFeeds
                     && (param == EExportFeedType.YandexMarket || param == EExportFeedType.GoogleMerchentCenter))
                {
                    return Json(new CommandResult { Result = false, Error = "access denied" });
                }
                typesList.Add(param.ToString(), param.Localize());

            }
            else
            {
                foreach (EExportFeedType exportFeedType in Enum.GetValues(typeof(EExportFeedType)))
                {
                    if (Saas.SaasDataService.IsSaasEnabled && !Saas.SaasDataService.CurrentSaasData.HaveExportFeeds
                        && (exportFeedType == EExportFeedType.YandexMarket || exportFeedType == EExportFeedType.GoogleMerchentCenter) || exportFeedType == EExportFeedType.None)
                    {
                        continue;
                    }
                    typesList.Add(exportFeedType.ToString(), exportFeedType.Localize());
                }
            }
            return Json(new CommandResult { Result = true, Obj = typesList });
        }


        public JsonResult AddGlobalDeliveryCosts (ExportFeedYandexDeliveryCostOption model)
        {
            return JsonOk();
        }
    }
}
