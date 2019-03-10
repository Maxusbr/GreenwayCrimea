
using AdvantShop.Web.Infrastructure.Controllers;
using System;
using System.IO;
using System.Web.Mvc;
using AdvantShop.Module.BannerMania.Service;
using AdvantShop.Module.BannerMania.Models;
using AdvantShop.Diagnostics;
using AdvantShop.Helpers;
using AdvantShop.Core.Modules;

namespace AdvantShop.Module.BannerMania.Controllers
{
    public class BMSettingsAdminController : ModuleAdminController
    {
        [ChildActionOnly]
        public ActionResult CommonSettings()
        {
            return PartialView("~/Modules/" + BannerMania.ModuleStringId + "/Views/Admin/CommonSettings.cshtml");
        }
        
        [ChildActionOnly]
        public ActionResult Feedback()
        {
            return PartialView("~/Modules/" + BannerMania.ModuleStringId + "/Views/Admin/Feedback.cshtml");
        }

        [HttpGet]
        public JsonResult GetSettings()
        {
            var settings = new BannerCommonSettings
            {
                BannerInTopWidth = BMSettings.BannerInTopWidth,
                BannerInTopHeight = BMSettings.BannerInTopHeight,
                UnderDeliveryInfoWidth = BMSettings.UnderDeliveryInfoWidth,
                UnderDeliveryInfoHeight = BMSettings.UnderDeliveryInfoHeight,
                AboveDeliveryInfoWidth = BMSettings.AboveDeliveryInfoWidth,
                AboveDeliveryInfoHeight = BMSettings.AboveDeliveryInfoHeight,
                UnderFilterWidth = BMSettings.UnderFilterWidth,
                UnderFilterHeight = BMSettings.UnderFilterHeight,
                AboveFilterWidth = BMSettings.AboveFilterWidth,
                AboveFilterHeight = BMSettings.AboveFilterHeight,
                UnderMenuWidth = BMSettings.UnderMenuWidth,
                UnderMenuHeight = BMSettings.UnderMenuHeight,
                AboveFooterWidth = BMSettings.AboveFooterWidth,
                AboveFooterHeight = BMSettings.AboveFooterHeight
            };

            return Json(settings, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveChanges(BannerCommonSettings settings)
        {
            BMSettings.BannerInTopWidth = settings.BannerInTopWidth;
            BMSettings.BannerInTopHeight = settings.BannerInTopHeight;
            BMSettings.UnderDeliveryInfoWidth = settings.UnderDeliveryInfoWidth;
            BMSettings.UnderDeliveryInfoHeight = settings.UnderDeliveryInfoHeight;
            BMSettings.AboveDeliveryInfoWidth = settings.AboveDeliveryInfoWidth;
            BMSettings.AboveDeliveryInfoHeight = settings.AboveDeliveryInfoHeight;
            BMSettings.UnderFilterWidth = settings.UnderFilterWidth;
            BMSettings.UnderFilterHeight = settings.UnderFilterHeight;
            BMSettings.AboveFilterWidth = settings.AboveFilterWidth;
            BMSettings.AboveFilterHeight = settings.AboveFilterHeight;
            BMSettings.UnderMenuWidth = settings.UnderMenuWidth;
            BMSettings.UnderMenuHeight = settings.UnderMenuHeight;
            BMSettings.AboveFooterWidth = settings.AboveFooterWidth;
            BMSettings.AboveFooterHeight = settings.AboveFooterHeight;

            return Json(new { success = true, msg = "Сохранено" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ResizeImages()
        {
            var bannerEntities = BMService.GetBannerEntities();
            foreach (var bannerEntity in bannerEntities)
            {
                if (string.IsNullOrEmpty(bannerEntity.ImagePath)) { continue; }
                    
                var originalFilePath = BMService.GetPath(BMService.imagesPath + "original/" + bannerEntity.ImagePath);
                
                if (!System.IO.File.Exists(originalFilePath) || Path.GetExtension(originalFilePath) == ".gif")
                {
                    continue;
                }

                try
                {
                    var originalFile = new FileStream(originalFilePath, FileMode.Open);
                    using (System.Drawing.Image image = System.Drawing.Image.FromStream(originalFile))
                    {
                        var filePath = BMService.GetPath(BMService.imagesPath);

                        if (!Directory.Exists(filePath))
                        {
                            Directory.CreateDirectory(filePath);
                        }

                        var imageWidth = 0;
                        var imageHeight = 0;

                        switch(bannerEntity.Placement)
                        {
                            case PlacementType.UnderDeliveryInfo:
                                imageWidth = BMSettings.UnderDeliveryInfoWidth;
                                imageHeight = BMSettings.UnderDeliveryInfoHeight;
                                break;
                            case PlacementType.AboveDeliveryInfo:
                                imageWidth = BMSettings.AboveDeliveryInfoWidth;
                                imageHeight = BMSettings.AboveDeliveryInfoHeight;
                                break;
                            case PlacementType.UnderFilter:
                                imageWidth = BMSettings.UnderFilterWidth;
                                imageHeight = BMSettings.UnderFilterHeight;
                                break;
                            case PlacementType.AboveFilter:
                                imageWidth = BMSettings.AboveFilterWidth;
                                imageHeight = BMSettings.AboveFilterHeight;
                                break;
                            case PlacementType.UnderMenu:
                                imageWidth = BMSettings.UnderMenuWidth;
                                imageHeight = BMSettings.UnderMenuHeight;
                                break;
                            case PlacementType.AboveFooter:
                                imageWidth = BMSettings.AboveFooterWidth;
                                imageHeight = BMSettings.AboveFooterHeight;
                                break;
                        }

                        FileHelpers.SaveResizePhotoFile(filePath + bannerEntity.ImagePath, imageWidth, imageHeight, image);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log.Error(string.Format("File {0} не был обработан. Причина: {1}", originalFilePath, ex.Message));
                }
            }


            if (!string.IsNullOrEmpty(BMSettings.BannerInTopImagePath))
            {
                var bitImagePath = BMService.GetPath("userfiles/Modules/BannerMania/Content/Pictures/BannerInTop/Original/") + BMSettings.BannerInTopImagePath;

                if (System.IO.File.Exists(bitImagePath) && Path.GetExtension(bitImagePath) != ".gif")
                {
                    try
                    {
                        var bitOriginalFile = new FileStream(bitImagePath, FileMode.Open);
                        using (System.Drawing.Image image = System.Drawing.Image.FromStream(bitOriginalFile))
                        {
                            var filePath = BMService.GetPath("userfiles/Modules/BannerMania/Content/Pictures/BannerInTop/");

                            if (!Directory.Exists(filePath))
                            {
                                Directory.CreateDirectory(filePath);
                            }

                            FileHelpers.SaveResizePhotoFile(filePath + BMSettings.BannerInTopImagePath, BMSettings.BannerInTopWidth, BMSettings.BannerInTopHeight, image);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Log.Error(string.Format("File {0} не был обработан. Причина: {1}", bitImagePath, ex.Message));
                    }
                }
            }

            return Json(new { success = true, msg = "Изображения пережаты" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult FeedbackSettings()
        {
            var settings = new FeedbackSettingsModel();
            var customer = AdvantShop.Customers.CustomerContext.CurrentCustomer;

            if (customer != null)
            {
                settings.Name = string.Format("{0} {1} {2}", customer.FirstName, customer.Patronymic, customer.LastName);
                settings.Email = customer.EMail;
                settings.Phone = customer.Phone;
                settings.Message = string.Empty;
            }

            return Json(settings, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult FeedbackSend(FeedbackSettingsModel settings)
        {
            if (!FeedbackValidate(settings))
            {
                return Json(new { success = false, msg = "Заполните поле 'Сообщение'" }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                var mailBody = settings.Message + "<br /><br />";
                mailBody += string.Format("URL магазина: {0}; ФИО: {1}; Почта администратора: {2}; Телефон: {3}",
                    AdvantShop.Configuration.SettingsMain.SiteUrl,
                    settings.Name,
                    settings.Email,
                    settings.Phone);

                var mailSubject = "Обратная связь. Модуль 'Баннер-микс'.";
                ModulesService.SendModuleMail(Guid.Empty, mailSubject, mailBody, "help@promo-z.ru", true);
            }
            catch (Exception ex)
            {
                AdvantShop.Diagnostics.Debug.Log.Error("BannerMania Module, feedback error: " + ex.Message);
                return Json(new { success = false, msg = "Не удалось отправить сообщение" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, msg = "Сообщение успешно отправлено" }, JsonRequestBehavior.AllowGet);
        }

        public bool FeedbackValidate(FeedbackSettingsModel settings)
        {
            if (string.IsNullOrEmpty(settings.Message))
            {
                return false;
            }

            return true;
        }
    }
}
