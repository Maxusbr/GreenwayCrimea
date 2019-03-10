using System.Collections.Generic;
using System.Web.Mvc;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Customers;
using AdvantShop.Statistic;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.ExportCategories;
using AdvantShop.Web.Admin.Models.ExportCategories;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Catalog
{
    [Auth(RoleAction.Catalog)]
    public partial class ExportCategoriesController : BaseAdminController
    {
        public ActionResult Index()
        {
            SetNgController(NgControllers.NgControllersTypes.ExportCategoriesCtrl);

            ExportCategoriesModel model = new GetExportCategoriesHandler().Execute();

            return View(model);
        }

        public ActionResult Export()
        {
            SetNgController(NgControllers.NgControllersTypes.ExportCategoriesCtrl);

            if (CommonStatistic.IsRun) 
            {
                return View("ExportCategoriesProgress");
            };

            CommonStatistic.Init();
            CommonStatistic.CurrentProcess = "exportcategories/export";
            CommonStatistic.CurrentProcessName = LocalizationService.GetResource("Admin.ExportCategories.CategoriesExport");
            CommonStatistic.RowPosition = 0;

            var filePath = string.Empty;
            CommonStatistic.StartNew(() =>
            {
                filePath = new StartingExportCategoriesHandler().Execute(); ;
                CommonStatistic.FileName = "../" + filePath;
            });
            return View("ExportCategoriesProgress");
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SaveExportCategoriesSettings(string separator, string encoding, List<string> exportCategoriesFields)
        {
            var result = new SaveExportCategoriesFieldsHandler(separator, encoding, exportCategoriesFields).Execute();
            if(result)
            {
                return JsonOk();
            }
            else
            {
                return JsonError("Ошибка сохранения настроек");
            }
            
        }

    }
}
