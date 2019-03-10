using System;
using System.Web.Mvc;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.News;
using AdvantShop.SEO;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.News;
using AdvantShop.Web.Admin.Models.News;
using AdvantShop.Web.Admin.ViewModels.News;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Cms
{
    [Auth(RoleAction.Cms)]
    public partial class NewsCategoryController : BaseAdminController
    {

        public ActionResult Index()
        {
            var model = new NewsCategoryViewModel();
            SetMetaInformation(T("Admin.NewsCategory.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.NewsCategoryCtrl);

            return View(model);
        }
        
        #region Add/Edit/Get/Delete

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddNewsCategory(NewsCategory model)
        {
            if (string.IsNullOrEmpty(model.Name))
                return Json(new { result = false });

            try
            {
                var newscat = new NewsCategory()
                {
                    Name = model.Name,
                    SortOrder = model.SortOrder,
                    UrlPath = model.UrlPath ?? string.Empty,
                    Meta = model.Meta
                };

                NewsService.InsertNewsCategory(newscat);
            }
            catch (Exception ex)
            {
                Debug.Log.Error("", ex);
                return Json(new { result = false });
            }

            return Json(new { result = true });
        }

        public JsonResult EditNewsCategory(NewsCategory model)
        {
            if (string.IsNullOrEmpty(model.Name))
                return Json(new { result = false });

            try
            {
                var newsCategory = new NewsCategory()
                {
                    NewsCategoryId = model.NewsCategoryId,
                    Name = model.Name,
                    UrlPath = model.UrlPath,
                    SortOrder = model.SortOrder,
                    Meta = model.Meta
                };
                NewsService.UpdateNewsCategory(newsCategory);
            }
            catch (Exception ex)
            {
                Debug.Log.Error("", ex);
                return Json(new { result = false });
            }

            return Json(new { result = true });
        }

        public JsonResult GetNewsCategoryItem(int ID)
        {
            var newsCategory = NewsService.GetNewsCategoryById(ID);
            var meta = MetaInfoService.GetMetaInfo(ID, MetaType.NewsCategory);
            return Json(new { newsCategory, meta });
        }


        public JsonResult GetNewsCategory(NewsCategoryFilterModel model)
        {
            var hendler = new GetNewsCategory(model);
            var result = hendler.Execute();

            return Json(result);
        }

        public JsonResult DeleteNewsCategory(NewsCategoryFilterModel model)
        {
            Command(model, (id, c) =>
            {
                NewsService.DeleteNewsCategory(id);
                return true;
            });

            return Json(true);
        }

        #endregion    
        
        #region Inplace

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public JsonResult InplaceNewsCategory(NewsCategoryFilterModel model)
        {
            int id = 0;
            Int32.TryParse(model.NewsCategoryId, out id);
            if(id == 0)
            {
                return Json(new { result = false });
            }
            var newsCategory = NewsService.GetNewsCategoryById(id);

            newsCategory.Name = model.Name ?? string.Empty;
            newsCategory.UrlPath = model.UrlPath ?? string.Empty;
            newsCategory.SortOrder = model.SortOrder;

            NewsService.UpdateNewsCategory(newsCategory);

            return Json(new { result = true });
        }

        #endregion

        #region Command

        private void Command(NewsCategoryFilterModel model, Func<int, NewsCategoryFilterModel, bool> func)
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
                var handler = new GetNewsCategory(model);
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
