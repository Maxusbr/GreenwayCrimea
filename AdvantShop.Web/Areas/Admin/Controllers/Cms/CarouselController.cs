using System;
using System.IO;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Services.CMS;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Carousel;
using AdvantShop.Web.Admin.Models.Carousel;
using AdvantShop.Web.Admin.ViewModels.Carousel;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Cms
{
    [Auth(RoleAction.Cms)]
    public partial class CarouselController : BaseAdminController
    {
        public ActionResult Index()
        {
            var model = new CarouselViewModel();
            SetMetaInformation(T("Admin.Carousel.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.CarouselCtrl);

            return View(model);
        }

        #region Add/Edit/Get/Delete

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddCarousel(CarouselFilterModel model)
        {
            try
            {
                int sort = 0;
                Int32.TryParse(model.SortOrder, out sort);
                var carousel = new Carousel()
                {
                    Url = model.CaruselUrl,
                    SortOrder = sort,
                    Enabled = Convert.ToBoolean(model.Enabled),
                    DisplayInOneColumn = Convert.ToBoolean(model.DisplayInOneColumn),
                    DisplayInTwoColumns = Convert.ToBoolean(model.DisplayInTwoColumns),
                    DisplayInMobile = Convert.ToBoolean(model.DisplayInMobile),
                    Blank = Convert.ToBoolean(model.Blank),
                };
                var carouselId = CarouselService.AddCarousel(carousel);
                var fullfilename = FoldersHelper.GetPathAbsolut(FolderType.ImageTemp, model.ImageSrc);
                var tempName = PhotoService.AddPhoto(new AdvantShop.Catalog.Photo(0, carouselId, PhotoType.Carousel)
                {
                    Description = model.Description,
                    OriginName = Path.GetFileName(fullfilename),
                    PhotoSortOrder = 0,
                    ColorID = null
                });

                if (!string.IsNullOrEmpty(tempName))
                {
                    using (System.Drawing.Image image = System.Drawing.Image.FromFile(fullfilename))
                    {
                        FileHelpers.SaveResizePhotoFile(FoldersHelper.GetPathAbsolut(FolderType.Carousel, tempName), SettingsPictureSize.CarouselBigWidth, SettingsPictureSize.CarouselBigHeight, image);
                    }
                    FileHelpers.DeleteFilesFromImageTemp();
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error("", ex);
                return Json(new { result = false });
            }

            return Json(new { result = true });
        }
        
        public JsonResult GetCarousel(CarouselFilterModel model)
        {
            var hendler = new GetCarousel(model);
            var result = hendler.Execute();

            for(var i = 0; i < result.DataItems.Count; i++)
            {
                result.DataItems[i].ImageSrc = result.DataItems[i].Picture.ImageSrc();
            }

            return Json(result);
        }

        public JsonResult DeleteCarousel(CarouselFilterModel model)
        {
            Command(model, (id, c) =>
            {
                CarouselService.DeleteCarousel(id);
                return true;
            });

            return Json(true);
        }

        #endregion

        #region Upload

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Upload()
        {            
            var result = new UploadPicture().Execute();
            if (result.Result)
            {
                return JsonOk(new { picture = result.Picture, pictureName = result.FileName });
            }
            else
            {
                return JsonError("Ошибка при загрузке изображения");
            }
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult UploadByLink(string fileLink)
        {            
            var result = new UploadPictureByLink(fileLink).Execute();
            if (result.Result)
            {
                return JsonOk(new { picture = result.Picture, pictureName = result.FileName });
            }
            else
            {
                return JsonError("Ошибка при загрузке изображения");
            }
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult DeleteFile()
        {
            FileHelpers.DeleteFilesFromImageTemp();
            return JsonOk(new { picture = "../images/nophoto_small.jpg" });            
        }

        #endregion

        #region Inplace

        public JsonResult InplaceCarousel(CarouselFilterModel model)
        {
            if (model.CarouselId == 0)
            {
                return Json(new { result = false });
            }
            var carousel = CarouselService.GetCarousel(model.CarouselId);

            carousel.Url = model.CaruselUrl;
            carousel.SortOrder = Convert.ToInt32(model.SortOrder);
            carousel.Enabled = (bool)model.Enabled;
            carousel.DisplayInOneColumn = (bool)model.DisplayInOneColumn;
            carousel.DisplayInTwoColumns = (bool)model.DisplayInTwoColumns;
            carousel.DisplayInMobile = (bool)model.DisplayInMobile;
            carousel.Blank = (bool)model.Blank;
            CarouselService.UpdateCarousel(carousel);
            if(carousel.Picture != null && carousel.Picture.Description != model.Description  && !string.IsNullOrEmpty(model.Description))
            {
                var photo = PhotoService.GetPhoto(carousel.Picture.PhotoId);
                if(photo != null)
                {
                    photo.Description = model.Description;
                    PhotoService.UpdatePhoto(photo);
                }
            }

            return Json(new { result = true });
        }

        #endregion

        #region Command

        private void Command(CarouselFilterModel model, Func<int, CarouselFilterModel, bool> func)
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
                var handler = new GetCarousel(model);
                var ids = handler.GetItemsIds();

                foreach (int id in ids)
                {
                    if (model.Ids == null || !model.Ids.Contains(id))
                        func(id, model);
                }
            }
        }


        #endregion
    }
}
