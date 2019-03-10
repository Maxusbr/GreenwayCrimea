using AdvantShop.Security;
using AdvantShop.Web.Infrastructure.Controllers;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Module.BannerMania.Service;
using AdvantShop.Module.BannerMania.Handlers;
using AdvantShop.Module.BannerMania.Models;
using System.Collections.Generic;
using System;
using AdvantShop.Web.Infrastructure.Filters;
using AdvantShop.Web.Infrastructure.Admin;
using System.Web.UI.WebControls;

namespace AdvantShop.Module.BannerMania.Controllers
{
    public class BFPAdminController : ModuleAdminController
    {
        [ChildActionOnly]
        public ActionResult BannerForProducts()
        {
            return PartialView("~/Modules/" + BannerMania.ModuleStringId + "/Views/Admin/BannerForProducts.cshtml");
        }

        [HttpPost]
        public JsonResult AddBannerEntity(int entityId, int? entityType, string entityName, int? placement, string url, bool newWindow, bool enabled, bool overwrite = false)
        {
            if(entityId <= 0 || !placement.HasValue || placement.Value < 0 || !entityType.HasValue || entityType.Value < 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            var bannerEntity = BMService.GetBannerEntity(entityId, (EntityType)entityType, (PlacementType)placement);
            if(bannerEntity != null)
            {
                if(!overwrite) return Json(null, JsonRequestBehavior.AllowGet);

                bannerEntity.EntityId = entityId;
                bannerEntity.EntityName = entityName;
                bannerEntity.EntityType = (EntityType)entityType;
                bannerEntity.NewWindow = newWindow;
                bannerEntity.URL = url;
                bannerEntity.Placement = (PlacementType)placement;

                BMService.UpdateBannerEntity(bannerEntity);

                return Json(true, JsonRequestBehavior.AllowGet);
            }

            bannerEntity = new BannerEntity()
            {
                EntityId = entityId,
                EntityName = entityName,
                EntityType = (EntityType)entityType,
                NewWindow = newWindow,
                Enabled = enabled,
                URL = url,
                Placement = (PlacementType)placement
            };

            BMService.AddBannerEntity(bannerEntity);

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPlacementTypes(int entityType)
        {
            if(entityType < 0)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            var index = 0;
            var placementList = new List<object>();
            foreach (var placement in Enum.GetNames(typeof(PlacementType)))
            {
                index++;
                switch ((EntityType)entityType)
                {
                    case EntityType.Products:
                    case EntityType.ProductsByCategories:
                        if (placement == "AboveFilter" || placement == "UnderFilter")
                        {
                            continue;
                        }
                    break;
                    case EntityType.Categories:
                        if (placement == "UnderDeliveryInfo" || placement == "AboveDeliveryInfo")
                        {
                            continue;
                        }
                    break;
                }

                placementList.Add(new { Id = index, Value = BMService.GetPlacementTypeName(placement) });
            }

            return Json(placementList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetEntityTypes()
        {
            var index = 0;
            var entityList = new List<object>();
            foreach (var entity in Enum.GetNames(typeof(EntityType)))
            {
                entityList.Add(new { Id = index, Value = BMService.GetEntityTypeName(entity) });
                index++;
            }

            return Json(entityList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCategories()
        {
            var allCategories = BMService.GetAllCategories();
            var categories = new List<ShortCategory>() { new ShortCategory(-1, "Не выбрана") };

            BMService.LoadAllCategories(allCategories, categories, 0, "");

            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UploadImage(HttpPostedFileBase imageFile, int entityId, int entityType, int placement)
        {
            var fileName = imageFile.FileName;
            var path = BMService.GetPath(BMService.imagesPath);
            var originalPath = BMService.GetPath(BMService.imagesPath + "original/");

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            if (!System.IO.Directory.Exists(originalPath))
            {
                System.IO.Directory.CreateDirectory(originalPath);
            }

            var totalFileName = fileName;
            while(System.IO.File.Exists(path + totalFileName))
            {
                var lastIndexDot = totalFileName.LastIndexOf('.');
                totalFileName = totalFileName.Insert(lastIndexDot, "n");
            }

            var bannerEntity = BMService.GetBannerEntity(entityId, (EntityType)entityType, (PlacementType)placement);
            if (bannerEntity != null)
            {
                if (System.IO.File.Exists(path + bannerEntity.ImagePath))
                {
                    System.IO.File.Delete(path + bannerEntity.ImagePath);
                }

                if (System.IO.File.Exists(originalPath + bannerEntity.ImagePath))
                {
                    System.IO.File.Delete(originalPath + bannerEntity.ImagePath);
                }
            }

            imageFile.SaveAs(path + totalFileName);
            imageFile.SaveAs(originalPath + totalFileName);

            BMService.UpdateImagePath(entityId, (EntityType)entityType, (PlacementType)placement, totalFileName);

            return Json(new { success = true, msg = "Изображение успешно загружено" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProduct(string artNo)
        {
            if (string.IsNullOrEmpty(artNo))
            {
                return Json(new { EntityId = -1, EntityName = "" }, JsonRequestBehavior.AllowGet);
            }

            var productId = AdvantShop.Catalog.ProductService.GetProductIDByOfferArtNo(artNo);
            if (productId <= 0)
            {
                return Json(new { EntityId = -1, EntityName = "" }, JsonRequestBehavior.AllowGet);
            }

            var entityName = BMService.GetEntityNameByEntityId(productId, EntityType.Products);
            return Json(new { EntityId = productId, EntityName = entityName }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidateImageFileExtenstion(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            if (!AdvantShop.Helpers.FileHelpers.CheckFileExtension(fileName, Helpers.EAdvantShopFileTypes.Image))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult Inplace(BannerEntityModel model)
        {
            var dbModel = BMService.GetBannerEntity(model.BannerId);
            if (dbModel == null)
                return Json(new { result = false });
            dbModel.NewWindow = model.NewWindow;
            dbModel.Enabled = model.Enabled;
            dbModel.URL = model.URL;
            BMService.UpdateBannerEntityById(dbModel);

            return Json(new { result = true });
        }

        public JsonResult GetBannerEntities(BannerEntityFilterModel model)
        {
            return Json(new GetBannerEntities(model).Execute());
        }

        #region Commands

        private void Command(BannerEntityFilterModel command, Func<int, BannerEntityFilterModel, bool> func)
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
                var handler = new GetBannerEntities(command);
                var ids = handler.GetItemsIds();

                foreach (int id in ids)
                {
                    if (command.Ids == null || !command.Ids.Contains(id))
                        func(id, command);
                }
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteBannerEntities(BannerEntityFilterModel command)
        {
            Command(command, (id, c) =>
            {
                BMService.DeleteBannerEntity(id);
                return true;
            });
            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteBannerEntity(int bannerId)
        {
            var bannerEntity = BMService.GetBannerEntity(bannerId);
            if(bannerEntity == null)
            {
                return JsonOk();
            }

            var imagePath = bannerEntity.ImagePath;

            var path = BMService.GetPath(BMService.imagesPath);
            var originalPath = BMService.GetPath(BMService.imagesPath + "original/");

            if (System.IO.File.Exists(path + imagePath))
            {
                System.IO.File.Delete(path + imagePath);
            }

            if (System.IO.File.Exists(originalPath + imagePath))
            {
                System.IO.File.Delete(originalPath + imagePath);
            }

            BMService.DeleteBannerEntity(bannerId);
            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SetNewWindow(BannerEntityFilterModel command)
        {
            Command(command, (id, c) =>
            {
                BMService.SetBannerEntityNewWindow(id, true);
                return true;
            });
            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SetNotNewWindow(BannerEntityFilterModel command)
        {
            Command(command, (id, c) =>
            {
                BMService.SetBannerEntityNewWindow(id, false);
                return true;
            });
            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SetEnabled(BannerEntityFilterModel command)
        {
            Command(command, (id, c) =>
            {
                BMService.SetBannerEntityEnabled(id, true);
                return true;
            });
            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SetDisabled(BannerEntityFilterModel command)
        {
            Command(command, (id, c) =>
            {
                BMService.SetBannerEntityEnabled(id, false);
                return true;
            });
            return JsonOk();
        }

        #endregion
    }
}
