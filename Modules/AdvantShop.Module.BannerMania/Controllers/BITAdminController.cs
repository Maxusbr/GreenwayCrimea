using AdvantShop.Security;
using AdvantShop.Web.Infrastructure.Controllers;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Module.BannerMania.Service;
using AdvantShop.Module.BannerMania.Models;

namespace AdvantShop.Module.BannerMania.Controllers
{
    public class BITAdminController : ModuleAdminController
    {
        [ChildActionOnly]
        public ActionResult BannerInTop()
        {
            return PartialView("~/Modules/" + BannerMania.ModuleStringId + "/Views/Admin/BannerInTop.cshtml");
        }

        [HttpGet]
        public ActionResult GetSettings()
        {
            var imagePath = BMService.GetPath("userfiles/Modules/BannerMania/Content/Pictures/BannerInTop/") + BMSettings.BannerInTopImagePath;
            if (!System.IO.File.Exists(imagePath))
            {
                BMSettings.BannerInTopImagePath = string.Empty;
            }

            var settings = new BannerInTopSettings
            {
                URL = BMSettings.BannerInTopURL,
                OnlyOnMainPage = BMSettings.BannerInTopOnlyOnMainPage,
                TargetBlank = BMSettings.BannerInTopTargetBlank,
                ImagePath = BMSettings.BannerInTopImagePath
            };

            return Json(settings, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveChanges(BannerInTopSettings settings)
        {
            BMSettings.BannerInTopOnlyOnMainPage = settings.OnlyOnMainPage;
            BMSettings.BannerInTopTargetBlank = settings.TargetBlank;
            BMSettings.BannerInTopURL = settings.URL ?? string.Empty;
            
            return Json(new { success = true, msg = "Сохранено" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UploadImage(HttpPostedFileBase imageFile)
        {
            var fileName = imageFile.FileName;
            
            if (string.IsNullOrEmpty(fileName))
            {
                return Json(new { success = false, newImagePath = fileName, msg = "Недопустимый тип файла", JsonRequestBehavior.AllowGet });
            }

            if (!AdvantShop.Helpers.FileHelpers.CheckFileExtension(fileName, Helpers.EAdvantShopFileTypes.Image))
            {
                return Json(new { success = false, newImagePath = fileName, msg = "Недопустимый тип файла", JsonRequestBehavior.AllowGet });
            }

            var path = BMService.GetPath("userfiles/Modules/BannerMania/Content/Pictures/BannerInTop/");
            var originalPath = BMService.GetPath("userfiles/Modules/BannerMania/Content/Pictures/BannerInTop/Original/");

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            if (!System.IO.Directory.Exists(originalPath))
            {
                System.IO.Directory.CreateDirectory(originalPath);
            }

            var oldImageFileName = BMSettings.BannerInTopImagePath;
            if (System.IO.File.Exists(path + oldImageFileName))
            {
                System.IO.File.Delete(path + oldImageFileName);
            }

            if (System.IO.File.Exists(originalPath + oldImageFileName))
            {
                System.IO.File.Delete(originalPath + oldImageFileName);
            }

            imageFile.SaveAs(path + fileName);
            imageFile.SaveAs(originalPath + fileName);

            BMSettings.BannerInTopImagePath = fileName;

            return Json(new { success = true, newImagePath = fileName, msg = "Изображение успешно загружено" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteImage()
        {
            var path = BMService.GetPath("userfiles/Modules/BannerMania/Content/Pictures/BannerInTop/");
            var originalPath = BMService.GetPath("userfiles/Modules/BannerMania/Content/Pictures/BannerInTop/Original/");

            var imageFileName = BMSettings.BannerInTopImagePath;
            if (System.IO.File.Exists(path + imageFileName))
            {
                System.IO.File.Delete(path + imageFileName);
            }

            if (System.IO.File.Exists(originalPath + imageFileName))
            {
                System.IO.File.Delete(originalPath + imageFileName);
            }

            BMSettings.BannerInTopImagePath = string.Empty;

            return Json(new { success = true, newImagePath = string.Empty, msg = "Изображение успешно удалено" }, JsonRequestBehavior.AllowGet);
        }
    }
}
