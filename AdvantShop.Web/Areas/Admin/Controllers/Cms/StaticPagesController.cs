using System;
using System.Web;
using System.Web.Mvc;
using AdvantShop.CMS;
using AdvantShop.Core.Controls;
using AdvantShop.Customers;
using AdvantShop.Helpers;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.StaticPages;
using AdvantShop.Web.Admin.Models.StaticPages;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Cms
{
    [Auth(RoleAction.Cms)]
    public partial class StaticPagesController : BaseAdminController
    {
        #region List

        public ActionResult Index()
        {
            SetMetaInformation(T("Admin.StaticPages.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.StaticPagesCtrl);

            return View();
        }

        public JsonResult GetStaticPages(StaticPagesFilterModel model)
        {
            return Json(new GetStaticPages(model).Execute());
        }

        #region Inplace

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult InplaceStaticPage(StaticPagesFilterModel model)
        {
            if (model.StaticPageId == 0)
                return JsonError();
            
            var staticpage = StaticPageService.GetStaticPage(model.StaticPageId);

            staticpage.Enabled = model.Enabled ?? false;

            if (model.SortOrder != null)
                staticpage.SortOrder = model.SortOrder.Value;

            StaticPageService.UpdateStaticPage(staticpage);

            return JsonOk();
        }

        #endregion

        #region Commands

        private void Command(StaticPagesFilterModel command, Func<int, StaticPagesFilterModel, bool> func)
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
                foreach (int id in new GetStaticPages(command).GetItemsIds())
                {
                    if (command.Ids == null || !command.Ids.Contains(id))
                        func(id, command);
                }
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteStaticPages(StaticPagesFilterModel command)
        {
            Command(command, (id, c) =>
            {
                StaticPageService.DeleteStaticPage(id);
                return true;
            });
            return Json(true);
        }

        #endregion

        #endregion

        #region Add/Edit

        public ActionResult Add()
        {
            var model = new AdminStaticPageModel()
            {
                StaticPageId = -1,
                IsEditMode = false,
                DefaultMeta = true,
                Enabled = true,
                IndexAtSiteMap = true
            };

            SetMetaInformation(T("Admin.StaticPages.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.StaticPageCtrl);

            return View("AddEdit", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Add(AdminStaticPageModel model)
        {
            if (ModelState.IsValid)
            {
                var staticPageId = new AddUpdateStaticPage(model).Execute();
                if (staticPageId != 0)
                {
                    ShowMessage(NotifyType.Success, T("Admin.ChangesSuccessfullySaved"));
                    return RedirectToAction("Edit", new { id = staticPageId });
                }
            }

            ShowErrorMessages();

            SetMetaInformation(T("Admin.StaticPages.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.StaticPageCtrl);

            return View("AddEdit", model);
        }

        public ActionResult Edit(int id)
        {
            var staticPage = StaticPageService.GetStaticPage(id);
            if (staticPage == null)
                return Error404();

            var model = new GetStaticPageModel(staticPage).Execute();

            SetMetaInformation(T("Admin.StaticPages.Index.Title") + " - " + staticPage.PageName);
            SetNgController(NgControllers.NgControllersTypes.StaticPageCtrl);

            return View("AddEdit", model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(AdminStaticPageModel model)
        {
            if (ModelState.IsValid)
            {
                var result = new AddUpdateStaticPage(model).Execute();
                if (result != 0)
                {
                    ShowMessage(NotifyType.Success, T("Admin.ChangesSuccessfullySaved"));
                    return RedirectToAction("Edit", new { id = model.StaticPageId });
                }
            }

            ShowErrorMessages();

            SetMetaInformation(T("Admin.StaticPages.Index.Title") + " - " + model.PageName);
            SetNgController(NgControllers.NgControllersTypes.StaticPageCtrl);

            return View("AddEdit", model);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteStaticPage(int staticPageId)
        {
            StaticPageService.DeleteStaticPage(staticPageId);
            return JsonOk();
        }

        #endregion
        
        // Fast add static page in Menu page modal
        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddStaticPage(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return JsonError();

            name = HttpUtility.HtmlEncode(name);

            var page = new StaticPage()
            {
                PageName = name,
                UrlPath = StringHelper.TransformUrl(StringHelper.Translit(name)),
                Enabled = true,
                IndexAtSiteMap = true,
                Meta = null
            };

            StaticPageService.AddStaticPage(page);

            return Json(new
            {
                result = true,
                id = page.StaticPageId,
                url = Url.RouteUrl("StaticPage", new { url = page.UrlPath })
            });
        }
    }
}
