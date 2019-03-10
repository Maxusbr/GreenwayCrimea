using System;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.Sizes;
using AdvantShop.Web.Admin.Models.Sizes;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Catalog
{
    [Auth(RoleAction.Catalog)]
    public class SizesController : BaseAdminController
    {
        public ActionResult Index()
        {
            SetMetaInformation(T("Admin.Sizes.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.SizesCtrl);

            return View();
        }

        public JsonResult GetSizes(SizesFilterModel model)
        {
            return Json(new GetSizesHandler(model).Execute());
        }

        #region Commands

        private void Command(SizesFilterModel command, Action<int, SizesFilterModel> func)
        {
            if (command.SelectMode == SelectModeCommand.None)
            {
                foreach (var id in command.Ids)
                    func(id, command);
            }
            else
            {
                var ids = new GetSizesHandler(command).GetItemsIds("SizeId");
                foreach (int id in ids)
                {
                    if (command.Ids == null || !command.Ids.Contains(id))
                        func(id, command);
                }
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteSizes(SizesFilterModel command)
        {
            Command(command, (id, c) =>
            {
                if (!SizeService.IsSizeUsed(id))
                    SizeService.DeleteSize(id);
            });
            return Json(true);
        }

        #endregion

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteSize(int sizeId)
        {
            if (!SizeService.IsSizeUsed(sizeId))
                SizeService.DeleteSize(sizeId);
            return Json(true);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult InplaceSize(SizeModel model)
        {
            var size = SizeService.GetSize(model.SizeId);
            if (size == null)
                return JsonError();

            size.SizeName = model.SizeName.DefaultOrEmpty();
            size.SortOrder = model.SortOrder;

            SizeService.UpdateSize(size);

            return JsonOk();
        }

        #region Add | Update size

        [HttpGet]
        public JsonResult GetSize(int sizeId)
        {
            var size = SizeService.GetSize(sizeId);
            return Json(size);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddSize(SizeModel model)
        {
            if (string.IsNullOrEmpty(model.SizeName))
                return Json(new { result = false });

            try
            {
                var size = new Size()
                {
                    SizeName = model.SizeName.DefaultOrEmpty(),
                    SortOrder = model.SortOrder
                };

                SizeService.AddSize(size);
            }
            catch (Exception ex)
            {
                Debug.Log.Error("", ex);
                return Json(new { result = false });
            }

            return Json(new { result = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UpdateSize(SizeModel model)
        {
            if (string.IsNullOrEmpty(model.SizeName))
                return Json(new { result = false });

            try
            {
                var size = SizeService.GetSize(model.SizeId);

                size.SizeName = model.SizeName.DefaultOrEmpty();
                size.SortOrder = model.SortOrder;

                SizeService.UpdateSize(size);
            }
            catch (Exception ex)
            {
                Debug.Log.Error("", ex);
                return Json(new { result = false });
            }

            return Json(new { result = true });
        }

        #endregion

    }
}
