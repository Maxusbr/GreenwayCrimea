using System;
using System.Web.Mvc;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Diagnostics;
using AdvantShop.Module.SimaLand.ViewModel;
using AdvantShop.Module.SimaLand.Service;
using System.Web;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using AdvantShop.Module.SimaLand.Models;
using AdvantShop.Security;
using AdvantShop.Configuration;

namespace AdvantShop.Module.SimaLand.Controllers
{
    public class ComparisonCategoryController : ModuleController
    {
        bool allow = Secure.VerifyAccess();

        public ActionResult Load_Page(int? categoryId = null, int level = 0, bool back = false)
        {
            if (!allow)
            {
                throw new HttpException(403, "Доступ запрещён!");
            }
            try
            {
                Task.Factory.StartNew(() => SimalandCategoryService.ClearNullCategoryId());
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            try
            {                
                ComparisonCategoryViewModel model = new ComparisonCategoryViewModel();

                level = back ? level - 1 : level;
                model.PrevLevel = level;
                model.List = categoryId == null || (back && level == 0) ? SimalandCategoryService.GetCategoriesByLevel(level + 1) : SimalandCategoryService.GetCategoriesByLevelPath((int)categoryId, level + 1);
                model.PrevCategoryId = categoryId != null && categoryId != 0 ? SimalandCategoryService.GetParentCategory((int)categoryId) : 0;
                model.Domain = SettingsMain.SiteUrlPlain;
                model.TotalCountActive = SimalandCategoryService.GetCountSlCategoryNoHidden();
                model.TotalCountLink = SimalandCategoryService.GetCountSlCategoryLinks();
                return PartialView("~/Modules/SimaLand/Views/ComparisonCategory/Index.cshtml", model);
            }
            catch (Exception ex)
            {
                //Debug.Log.Error(ex);
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AdvCategory(int parent = 0)
        {
            if (!allow)
            {
                throw new HttpException(403, "Доступ запрещён!");
            }

            var categories = AdvCategoryService.GetAdvCategory(parent);
            var model = new AdvCategoryViewModel(categories);
            if (parent == 0)
            {
                model.DeleteLink = true;
            }
            else
            {
                model.DeleteLink = false;
            }
            model.Back = AdvCategoryService.GetParent(parent);
            return PartialView("~/Modules/SimaLand/Views/ComparisonCategory/AdvCategories.cshtml", model);
        }

        public ActionResult SetAdvCategory(int advcatid, int slcatid)
        {
            if (!allow)
            {
                throw new HttpException(403, "Доступ запрещён!");
            }

            SimalandCategoryService.SetAdvCategoryForSlCategory(advcatid, slcatid);
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DelAdvCategory(int slcatid)
        {
            if (!allow)
            {
                throw new HttpException(403, "Доступ запрещён!");
            }
            SimalandCategoryService.DeleteAdvCategoryFromSlCategory(slcatid);
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetParseCategory()
        {
            if (!allow)
            {
                throw new HttpException(403, "Доступ запрещён!");
            }
            SimalandCategoryToParseViewModel model = new SimalandCategoryToParseViewModel();
            model.Categories = SimalandCategoryService.GetSlCategoryForParse();
            model.TotalCountCategories = SimalandCategoryService.GetTotalCountParseCategories();
            return PartialView("~/Modules/SimaLand/Views/ComparisonCategory/ParseCategories.cshtml", model);
        }
        
        public ActionResult AddCategories()
        {
            if (!allow)
            {
                throw new HttpException(403, "Доступ запрещён!");
            }
            if (!PSLModuleSettings.AddingCategories)
            {
                Task addCategory = Task.Factory.StartNew(() => SimalandCategoryService.AddingSlCategoriesToAdv());
                
                return Json(new { status = "ok", message = "Добавлен категорий успешно запущено" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { status = "error", message = "Процесс уже был запущен" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RenderHrefArtNo(string slProductId)
        {
            if (!allow)
            {
                return Json(new { ok = false }, JsonRequestBehavior.AllowGet);
            }

            var q = AdvProductService.isSLProduct(slProductId);
            if (!q)
            {
                return Json(new { ok = false },JsonRequestBehavior.AllowGet);
            }

            var slId = slProductId.Split('-')[0];

            var query = "https://www.sima-land.ru/api/v3/item/" + slId + "/";
            var response = ApiService.Request(query);
            var sid = JsonConvert.DeserializeObject<SimalandProduct>(response).sid;
            var artno = string.Format("<a href='https://sima-land.ru/{0}' target='_blank'>{1}</a>", sid, slProductId);
            return Json(new { ok = true, artno}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult linkonvieworder()
        {
            if (!allow)
            {
                throw new HttpException(403, "Доступ запрещён!");
            }
            return Json(PSLModuleSettings.LinkInViewOrder, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadSettings()
        {
            if (!allow)
            {
                throw new HttpException(403, "Доступ запрещён!");
            }

            var obj = new ModuleSettingsModel();

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveData(ModuleSettingsModel settings)
        {
            if (!allow)
            {
                throw new HttpException(403, "Доступ запрещён!");
            }
            try
            {
                var obj = settings.SaveSettings();

                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new StatusMessage(ex.Message, StatusMessage.Status.Error), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ImportCategory(int categoryId)
        {
            if (!allow)
            {
                throw new HttpException(403, "Доступ запрещён!");
            }
            SimalandCategoryService.ClearNullCategoryId();
            var links = SimalandCategoryService.GetCountRelatedCategoriesByCatSlId(categoryId);
            if (links == 0)
            {
                Task addCategory = Task.Factory.StartNew(() => SimalandCategoryService.AddOneSlCategoryToAdv(categoryId));
                return Json(new { header = "Процесс запущен", text = "Добавление категорий успешно запущено" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { header = "Нельзя добавить категорию", text = "Категория уже имеет связи с категориями вашего магазина" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ParseProducts()
        {
            if (!allow)
            {
                throw new HttpException(403, "Доступ запрещён!");
            }

            if (SimalandImportStatistic.Process)
            {
                SimalandImportStatistic.Process = false;
                return Json(new { status = false, message = new StatusMessage("Загрузка прервана", StatusMessage.Status.Info) }, JsonRequestBehavior.AllowGet);
            }

            if (!SimalandImportStatistic.Process)
            {
                Task.Factory.StartNew(() => SimalandProductService.ParseProducts());
                return Json(new { status = true, message = new StatusMessage("Загрузка товаров успешно запущена", StatusMessage.Status.Success) });
            }
            return Json(new { status = false, message = new StatusMessage("Процесс загрузки товаров уже был запущен", StatusMessage.Status.Info) });
        }

        public ActionResult GetParsingStatus()
        {
            return SimalandImportStatistic.Process ? Json(new { status = "process", message = "Идёт загрузка товаров...\n\rОбработано товаров: " + SimalandImportStatistic.TotalProcessedProducts + "\n\rОшибок: "+SimalandImportStatistic.TotalProductError, dwnlbtn = "Остановить загрузку" }) 
                : Json(new { status = PSLModuleSettings.LastUpdateProducts != "01.01.1990" ?
                "noprocess" : "noshow", message = "Последнее обновление: " + PSLModuleSettings.LastUpdateProducts,
                    dwnlbtn = "Загрузить товары" });
        }
        
        public ActionResult GetParsingCategoryStatus()
        {
            return SimalandImportStatistic.Process ? Json(new { status = "process", message = "Активных категорий Sima-land: " + SimalandImportStatistic.PrePareCategoriesInSimaLand + " Обработано: " + SimalandImportStatistic.TotalCountCategoriesProcess})
                : Json(new { status = "noprocess", message = "Последнее обновление: " +PSLModuleSettings.LastUpdateCategory});
        }

        [HttpPost]
        public ActionResult ParseCategory()
        {
            if (!allow)
            {
                throw new HttpException(403, "Доступ запрещён!");
            }

            Thread.Sleep(500);

            if (!SimalandImportStatistic.Process)
            {
                Task.Factory.StartNew(() => SimalandCategoryService.pc());
            }

            return Json(new StatusMessage("Начата загрузка категорий", StatusMessage.Status.Success));
        }

        /*product - attributes*/
        public ActionResult LoadPAttributes()
        {
            return Json(new {PSLModuleSettings.NotUpdateName, PSLModuleSettings.NotUpdateDescription, PSLModuleSettings.NotUpdateUrl, PSLModuleSettings.NotUpdateProperty }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SavePAttributes(bool nun, bool nud, bool nuu, bool nup)
        {
            PSLModuleSettings.NotUpdateName = nun;
            PSLModuleSettings.NotUpdateDescription = nud;
            PSLModuleSettings.NotUpdateUrl = nuu;
            PSLModuleSettings.NotUpdateProperty = nup;
            return Json(new StatusMessage("Сохранено",StatusMessage.Status.Success), JsonRequestBehavior.AllowGet);
        }

    }
}