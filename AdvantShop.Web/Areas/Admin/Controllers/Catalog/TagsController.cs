using System;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Core.Controls;
using AdvantShop.Core.Services.Catalog;
using AdvantShop.Core.UrlRewriter;
using AdvantShop.Customers;
using AdvantShop.SEO;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Tags;
using AdvantShop.Web.Admin.Models.Tags;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Catalog
{
    [Auth(RoleAction.Catalog)]
    [SaasFeature(Saas.ESaasProperty.HaveTags)]
    public partial class TagsController : BaseAdminController
    {
        #region Autocomlete

        public JsonResult GetAutocomleteTags()
        {
            var name = Request["data[q]"];

            var temp = TagService.Gets(name);
            if (temp.Count == 0)
            {
                if (UrlService.IsAvailableUrl(ParamType.Tag, name))
                    temp.Add(new Tag() { Id = 0, Name = name });
            }

            return Json(new { q = name, results = temp.Select(x => new { id = x.Name, text = x.Name }).ToList() });
        }

        #endregion

        #region List of tags

        public ActionResult Index()
        {
            SetMetaInformation(T("Admin.Tags.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.TagsCtrl);

            return View();
        }

        public JsonResult GetTags(TagsFilterModel model)
        {
            return Json(new GetTagsHandler(model).Execute());
        }

        #region Commands

        private void Command(TagsFilterModel model, Action<int, TagsFilterModel> func)
        {
            if (model.SelectMode == SelectModeCommand.None)
            {
                foreach (var id in model.Ids)
                    func(id, model);
            }
            else
            {
                var ids = new GetTagsHandler(model).GetItemsIds("Id");
                foreach (int id in ids)
                {
                    if (model.Ids == null || !model.Ids.Contains(id))
                        func(id, model);
                }
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteTags(TagsFilterModel model)
        {
            Command(model, (id, c) => TagService.Delete(id));
            return Json(true);
        }

        #endregion

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteTag(int id)
        {
            TagService.Delete(id);
            return Json(true);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult InplaceTag(TagsModel model)
        {
            var tag = TagService.Get(model.Id);
            if (tag == null || string.IsNullOrWhiteSpace(model.UrlPath))
                return JsonError();

            tag.UrlPath = model.UrlPath.Trim();
            tag.Enabled = model.Enabled;
            tag.SortOrder = model.SortOrder;
            tag.VisibilityForUsers = model.VisibilityForUsers;

            TagService.Update(tag);

            return JsonOk();
        }

        #endregion

        #region Add | Edit tag

        public ActionResult Add()
        {
            var model = new TagModel()
            {
                IsEditMode = false,
                DefaultMeta = true,
                Enabled = true,
                VisibilityForUsers = true
            };

            SetMetaInformation(T("Admin.Tags.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.TagsCtrl);

            return View("AddEdit", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Add(TagModel model)
        {
            if (ModelState.IsValid)
            {
                var handler = new AddUpdateTag(model);
                var categoryId = handler.Execute();

                if (categoryId != 0)
                {
                    ShowMessage(NotifyType.Success, T("Admin.ChangesSuccessfullySaved"));
                    return RedirectToAction("Edit", new { id = categoryId });
                }
            }

            ShowErrorMessages();

            SetMetaInformation(T("Admin.Tags.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.TagsCtrl);

            return View("AddEdit", model);
        }

        public ActionResult Edit(int id)
        {
            var tag = TagService.Get(id);
            if (tag == null)
                return RedirectToAction("Index");


            var model = new TagModel()
            {
                IsEditMode = true,

                Id = tag.Id,
                Name = tag.Name,
                Enabled = tag.Enabled,
                Description = tag.Description,
                BriefDescription = tag.BriefDescription,
                UrlPath = tag.UrlPath,
                VisibilityForUsers = tag.VisibilityForUsers,
                SortOrder = tag.SortOrder
            };

            var meta = MetaInfoService.GetMetaInfo(tag.Id, MetaType.Tag);
            if (meta == null)
            {
                model.DefaultMeta = true;
            }
            else
            {
                model.DefaultMeta = false;
                model.SeoTitle = meta.Title;
                model.SeoH1 = meta.H1;
                model.SeoKeywords = meta.MetaKeywords;
                model.SeoDescription = meta.MetaDescription;
            }

            SetMetaInformation(T("Admin.Tags.Index.Title") + " - " + tag.Name);
            SetNgController(NgControllers.NgControllersTypes.TagsCtrl);

            return View("AddEdit", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(TagModel model)
        {
            if (ModelState.IsValid)
            {
                var handler = new AddUpdateTag(model);
                var result = handler.Execute();

                if (result != 0)
                {
                    ShowMessage(NotifyType.Success, T("Admin.ChangesSuccessfullySaved"));
                    return RedirectToAction("Edit", new { id = model.Id });
                }
            }

            ShowErrorMessages();

            SetMetaInformation(T("Admin.Tags.Index.Title") + " - " + model.Name);
            SetNgController(NgControllers.NgControllersTypes.TagsCtrl);

            return View("Index", model);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SetTagActive(int id, bool active)
        {
            var tag = TagService.Get(id);
            if (tag == null)
                return Json(false);

            tag.Enabled = active;
            TagService.Update(tag);

            return Json(true);
        }

        #endregion
    }
}
