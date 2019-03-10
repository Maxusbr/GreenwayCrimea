using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.CMS;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.Services.Localization;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.FilePath;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Categories;
using AdvantShop.Web.Admin.Models.Categories;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Catalog
{
    [Auth(RoleAction.Catalog)]
    public partial class CategoryController : BaseAdminController
    {
        public ActionResult Add(int? parentId)
        {
            var model = new AdminCategoryModel()
            {
                IsEditMode = false,
                ParentCategoryId = parentId != null ? parentId.Value : 0,
                DisplayStyle = ECategoryDisplayStyle.Tile,
                Sorting = ESortOrder.NoSorting,
                DefaultMeta = true,
                Enabled = true
            };

            var parentCategory = CategoryService.GetCategory(model.ParentCategoryId);
            if (parentCategory != null)
                model.ParentCategoryName = parentCategory.Name;

            if (parentId.HasValue)
            {
                model.BreadCrumbs =
                    CategoryService.GetParentCategories((int)parentId)
                        .Select(x => new BreadCrumbs(x.Name, new UrlHelper(Request.RequestContext).Action("Index", "Catalog", new { categoryId = x.CategoryId })))
                        .Reverse()
                        .ToList();
                model.BreadCrumbs.Insert(0, new BreadCrumbs(LocalizationService.GetResource("Admin.Catalog.Index.CatalogTitle"), "catalog"));
            }

            SetMetaInformation(T("Admin.Catalog.Index.CategoryTitle"));
            SetNgController(NgControllers.NgControllersTypes.CategoryCtrl);

            return View("Index", model);
        }

        public ActionResult Edit(int id)
        {
            var category = CategoryService.GetCategory(id);
            if (category == null)
                return Error404();

            var model = new GetCategoryModel(category).Execute();

            SetMetaInformation(T("Admin.Catalog.Index.CategoryTitle") + " - " + category.Name);
            SetNgController(NgControllers.NgControllersTypes.CategoryCtrl);

            return View("Index", model);
        }


        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Add(AdminCategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var handler = new AddUpdateCategoryHandler(model);
                var categoryId = handler.Execute();

                if (categoryId != 0)
                {
                    ShowMessage(NotifyType.Success, T("Admin.ChangesSuccessfullySaved"));
                    return RedirectToAction("Index", "Catalog", new { categoryId });
                }
            }

            ShowErrorMessages();

            SetMetaInformation(T("Admin.Catalog.Index.CategoryTitle"));
            SetNgController(NgControllers.NgControllersTypes.CategoryCtrl);

            return View("Index", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(AdminCategoryModel model)
        {
            if (ModelState.IsValid)
            {
                var handler = new AddUpdateCategoryHandler(model);
                var result = handler.Execute();

                if (result != 0)
                {
                    ShowMessage(NotifyType.Success, T("Admin.ChangesSuccessfullySaved"));
                    return RedirectToAction("Edit", new { id = model.CategoryId });
                }
            }

            ShowErrorMessages();

            SetMetaInformation(T("Admin.Catalog.Index.CategoryTitle") + " - " + model.Name);
            SetNgController(NgControllers.NgControllersTypes.CategoryCtrl);

            return View("Index", model);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult Delete(int id)
        {
            var needRedirect = false;
            var deleteCategory = CategoryService.GetCategory(id);
            try
            {

                if (id == -1)
                {
                    return new JsonResult { Data = new { result = false, needRedirect = false, id = id } };
                }
                CategoryService.DeleteCategoryAndPhotos(id);
                CategoryService.DeleteCategoryLink(id);
                CategoryService.RecalculateProductsCountManual();
                needRedirect = true;

            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
            }

            return new JsonResult { Data = new { result = true, needRedirect = needRedirect, id = deleteCategory.ParentCategoryId } };
        }

        #region Picture

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UploadPicture(HttpPostedFileBase file, PhotoType type, int? objId)
        {
            var result = new UploadCategoryPictures(file, type, objId).Execute();
            return result.Result
                ? JsonOk(new {picture = result.Picture, pictureId = result.PictureId})
                : JsonError("Ошибка при загрузке изображения");
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeletePicture(int pictureId)
        {
            var result = new DeleteCategoryPictureHandler(pictureId).Execute();
            return result.Result
                ? JsonOk(new {picture = result.Picture})
                : JsonError("Ошибка при удалении изображения");
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UploadPictureByLink(PhotoType type, int? objId, string fileLink)
        {
            var result = new UploadCategoryPicturesByLink(type, objId, fileLink).Execute();
            return result.Result
                ? JsonOk(new {picture = result.Picture, pictureId = result.PictureId})
                : JsonError("Ошибка при загрузке изображения");
        }

        #endregion

        public JsonResult GetTags(int categoryId)
        {
            return Json(new
            {
                tags = TagService.GetAutocompleteTags().Select(x => new { value = x.Name }),
                selectedTags = TagService.Gets(categoryId, ETagType.Category, true).Select(x => new { value = x.Name })
            });
        }

        #region Property groups

        [HttpGet]
        public JsonResult GetAllPropertyGroups()
        {
            return Json(PropertyGroupService.GetList().Select(x => new
            {
                label = x.Name,
                value = x.PropertyGroupId
            }));
        }

        [HttpGet]
        public JsonResult GetPropertyGroups(int categoryId)
        {
            return Json(new
            {
                DataItems = PropertyGroupService.GetListByCategory(categoryId).Select(x => new
                {
                    CategoryId = categoryId,
                    x.PropertyGroupId,
                    x.Name
                }).ToList()
            });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddGroupToCategory(int groupId, int categoryId)
        {
            if (groupId == 0 || PropertyGroupService.Get(groupId) == null)
                return Json(false);
            try
            {
                PropertyGroupService.AddGroupToCategory(groupId, categoryId);
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return Json(false);
            }
            return Json(true);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteGroupFromCategory(int groupId, int categoryId)
        {
            PropertyGroupService.DeleteGroupFromCategory(groupId, categoryId);
            return Json(new { result = true });
        }

        #endregion

        #region Related / Alternative products

        // category
        public JsonResult GetRecomCategories(int categoryId, RelatedType type)
        {
            return Json(CategoryService.GetRelatedCategories(categoryId, type).Select(x => new { x.CategoryId, x.Name, Type = type }));
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult DeleteRecomCategory(int categoryId, int relCategoryId, RelatedType type)
        {
            CategoryService.DeleteRelatedCategory(categoryId, relCategoryId, type);
            return Json(new { result = true });
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult AddRecomCategory(int categoryId, int relCategoryId, RelatedType type)
        {
            var categoryIds = CategoryService.GetRelatedCategoryIds(categoryId, type);

            if (categoryIds == null || !categoryIds.Contains(relCategoryId))
            {
                CategoryService.AddRelatedCategory(categoryId, relCategoryId, type);
            }
            return Json(new { result = true });
        }


        // properties
        public JsonResult GetRecomProperties(int categoryId, RelatedType type)
        {
            var props = CategoryService.GetRelatedProperties(categoryId, type);

            return Json(props.Select(x => new { Id = x.PropertyId, Name = x.Name, Type = type }));
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult DeleteRecomProperty(int categoryId, int propertyId, RelatedType type)
        {
            CategoryService.DeleteRelatedProperty(categoryId, propertyId, type);

            return Json(new { result = true });
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult AddRecomProperty(int categoryId, int propertyId, RelatedType type)
        {
            var props = CategoryService.GetRelatedPropertyIds(categoryId, type);

            if (propertyId > 0 && (props == null || props.All(x => x != propertyId)))
                CategoryService.AddRelatedProperty(categoryId, propertyId, type);

            return Json(new { result = true });
        }


        // properties with values
        public JsonResult GetRecomPropertiesWithValues(int categoryId, RelatedType type)
        {
            var propVals = CategoryService.GetRelatedPropertyValues(categoryId, type);

            return Json(propVals.Select(x => new { Id = x.PropertyValueId, Name = x.Property.Name, Value = x.Value, Type = type }));
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult DeleteRecomPropertyWithValue(int categoryId, int propertyValueId, RelatedType type)
        {
            CategoryService.DeleteRelatedPropertyValue(categoryId, propertyValueId, type);

            return Json(new { result = true });
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult AddRecomPropertyWithValue(int categoryId, int propertyValueId, RelatedType type)
        {
            var props = CategoryService.GetRelatedPropertyValuesIds(categoryId, type);

            if (propertyValueId > 0 && (props == null || props.All(x => x != propertyValueId)))
                CategoryService.AddRelatedPropertyValue(categoryId, propertyValueId, type);

            return Json(new { result = true });
        }

        //properties and values
        public JsonResult GetProperties()
        {
            var props = PropertyService.GetAllProperties();

            return Json(props.Select(x => new { x.PropertyId, x.Name }));
        }

        public JsonResult GetPropertyValues(int propertyId)
        {
            var values = PropertyService.GetValuesByPropertyId(propertyId);

            return Json(values.Select(x => new { x.PropertyValueId, x.Value }));
        }

        #endregion

        #region AddCategoryList

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddCategoryList(int categoryId, List<string> categories)
        {
            var handler = new AddCategoryList(categoryId, categories);
            handler.Execute();

            return Json(new { result = true });
        }

        #endregion
    }
}
