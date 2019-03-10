using System;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Core.Controls;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Brands;
using AdvantShop.Web.Admin.Models.Brands;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Catalog
{
    [Auth(RoleAction.Catalog)]
    public partial class BrandsController : BaseAdminController
    {
        #region List

        public ActionResult Index()
        {
            SetMetaInformation(T("Admin.Brands.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.BrandsListCtrl);
            return View();
        }

        public JsonResult GetBrands(AdminBrandFilterModel model)
        {
            return Json(new GetBrands(model).Execute());
        }

        #region Inplace

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult InplaceBrand(AdminBrandModel model)
        {
            var brand = BrandService.GetBrandById(model.BrandId);
            brand.Name = model.BrandName;

            bool enabledChanged = brand.Enabled != model.Enabled;
            brand.Enabled = model.Enabled;

            brand.SortOrder = model.SortOrder;

            BrandService.UpdateBrand(brand);

            return Json(new { result = true });
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult DeleteBrand(int brandId)
        {
            BrandService.DeleteBrand(brandId);
            return Json(new { result = true });
        }

        #endregion

        #region Commands

        private void Command(AdminBrandFilterModel model, Action<int, AdminBrandFilterModel> func)
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
                var ids = new GetBrands(model).GetItemsIds();
                foreach (int id in ids)
                {
                    if (model.Ids == null || !model.Ids.Contains(id))
                        func(id, model);
                }
            }

            CategoryService.RecalculateProductsCountManual();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteBrands(AdminBrandFilterModel model)
        {
            Command(model, (id, c) => BrandService.DeleteBrand(id));
            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult ActivateBrands(AdminBrandFilterModel model)
        {
            Command(model, (id, c) => BrandService.SetActive(id, true));
            return JsonOk();
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DisableBrands(AdminBrandFilterModel model)
        {
            Command(model, (id, c) => BrandService.SetActive(id, false));
            return JsonOk();
        }
        #endregion

        #endregion

        #region Add | Edit Brand

        public ActionResult Add()
        {
            var model = new AdminBrandModel
            {
                BrandId = -1,
                IsEditMode = false,
                DefaultMeta = true,
                Enabled = true
            };

            SetMetaInformation(T("Admin.Brands.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.BrandCtrl);

            return View("AddEdit", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Add(AdminBrandModel model)
        {
            if (ModelState.IsValid)
            {
                var brandId = new AddUpdateBrand(model).Execute();
                if (brandId != 0)
                {
                    ShowMessage(NotifyType.Success, T("Admin.ChangesSuccessfullySaved"));
                    return RedirectToAction("Edit", new { id = brandId });
                }
            }

            ShowErrorMessages();

            SetMetaInformation(T("Admin.Brands.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.BrandCtrl);

            return View("AddEdit", model);
        }

        public ActionResult Edit(int id)
        {
            var brand = BrandService.GetBrandById(id);
            if (brand == null)
                return Error404();

            var model = new GetBrandModel(brand).Execute();

            SetMetaInformation(T("Admin.Brands.Index.Title") + " - " + brand.Name);
            SetNgController(NgControllers.NgControllersTypes.BrandCtrl);

            return View("AddEdit", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(AdminBrandModel model)
        {
            if (ModelState.IsValid)
            {
                var result = new AddUpdateBrand(model).Execute();
                if (result != 0)
                {
                    ShowMessage(NotifyType.Success, T("Admin.ChangesSuccessfullySaved"));
                    return RedirectToAction("Edit", new { id = model.BrandId });
                }
            }

            ShowErrorMessages();

            SetMetaInformation(T("Admin.Brands.Index.Title") + " - " + model.BrandName);
            SetNgController(NgControllers.NgControllersTypes.BrandCtrl);

            return View("AddEdit", model);
        }


        #region UploadPicture/delete

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UploadPicture(int? objId)
        {
            var result = new UploadBrandPicture(objId).Execute();
            return
                result.Result
                    ? JsonOk(new { picture = result.Picture, pictureId = result.PictureId })
                    : JsonError("Ошибка при загрузке изображения");
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeletePicture(int? objId)
        {
            var result = new DeleteBrandPicture(objId).Execute();
            return
                result.Result
                    ? JsonOk(new { picture = result.Picture })
                    : JsonError("Ошибка при удалении изображения");
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UploadPictureByLink(int? objId, string fileLink)
        {
            var result = new UploadBrandPictureByLink(objId, fileLink).Execute();
            return
                result.Result
                    ? JsonOk(new { picture = result.Picture, pictureId = result.PictureId })
                    : JsonError("Ошибка при загрузке изображения");
        }

        #endregion

        #endregion

        #region Brands chooser 

        public JsonResult GetBrandsChooser(AdminBrandFilterModel model)
        {
            var handler = new GetBrandsChooser(model);
            var result = handler.Execute();

            return Json(result);
        }

        #endregion
    }
}
