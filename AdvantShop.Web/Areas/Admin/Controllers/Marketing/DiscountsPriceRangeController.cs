using System;
using System.Web.Mvc;
using AdvantShop.Catalog;
using AdvantShop.Configuration;
using AdvantShop.Core.Caching;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Customers;
using AdvantShop.Orders;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.DiscountsPriceRanges;
using AdvantShop.Web.Admin.Models.DiscountsPriceRanges;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Marketing
{
    [Auth(RoleAction.Marketing)]
    public partial class DiscountsPriceRangeController : BaseAdminController
    {
        public ActionResult Index()
        {
            SetMetaInformation(T("Admin.DiscountsPriceRange.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.DiscountsPriceRangeCtrl);

            return View();
        }

        public JsonResult GetItems(DiscountsPriceRangeFilterModel model)
        {
            var handler = new GetDiscountsPriceRange(model);
            var result = handler.Execute();

            return Json(result);
        }

        #region Commands

        private void Command(DiscountsPriceRangeFilterModel model, Func<int, DiscountsPriceRangeFilterModel, bool> func)
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
                var handler = new GetDiscountsPriceRange(model);
                var ids = handler.GetItemsIds<int>();

                foreach (int id in ids)
                {
                    if (model.Ids == null || !model.Ids.Contains(id))
                        func(id, model);
                }
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteItems(DiscountsPriceRangeFilterModel model)
        {
            Command(model, (id, c) =>
            {
                OrderPriceDiscountService.Delete(id);
                return true;
            });
            return Json(true);
        }

        #endregion
        
        #region CRUD

        [HttpGet]
        public JsonResult GetItem(int orderPriceDiscountId)
        {
            return Json(OrderPriceDiscountService.Get(orderPriceDiscountId));
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UpdateItem(DiscountsPriceRangeModel model)
        {
            if (ModelState.IsValid)
            {
                var orderPriceDiscount = new OrderPriceDiscount()
                {
                    OrderPriceDiscountId = model.OrderPriceDiscountId,
                    PriceRange = model.PriceRange,
                    PercentDiscount = model.PercentDiscount
                };
                OrderPriceDiscountService.Update(orderPriceDiscount);

                return Json(new { result = true });
            }

            var errors = "";

            foreach (var modelState in ViewData.ModelState.Values)
                foreach (var error in modelState.Errors)
                    errors += " " + error.ErrorMessage;

            return Json(new { result = false, errors = errors });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteItem(int orderPriceDiscountId)
        {
            OrderPriceDiscountService.Delete(orderPriceDiscountId);
            return Json(new { result = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddItem(DiscountsPriceRangeModel model)
        {
            if (ModelState.IsValid)
            {
                var orderPriceDiscount = new OrderPriceDiscount()
                {
                    PriceRange = model.PriceRange,
                    PercentDiscount = model.PercentDiscount
                };
                OrderPriceDiscountService.Add(orderPriceDiscount);

                if (orderPriceDiscount.OrderPriceDiscountId != 0)
                    return Json(new { result = true });
            }

            var errors = "";

            foreach (var modelState in ViewData.ModelState.Values)
                foreach (var error in modelState.Errors)
                    errors += " " + error.ErrorMessage;

            return Json(new { result = false, errors = errors });
        }

        #endregion

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult EnableDiscounts(bool state)
        {
            SettingsCheckout.EnableDiscountModule = state;
            CacheManager.Remove(CacheNames.GetOrderPriceDiscountCacheObjectName());

            return Json(new { result = true });
        }

        #region Discount by datetime

        public JsonResult GetDiscountByDatetimeData()
        {
            return Json(new
            {
                DiscountByTimeService.Enabled,
                FromDateTime = DiscountByTimeService.FromDateTime.ToShortTimeString(),
                ToDateTime = DiscountByTimeService.ToDateTime.ToShortTimeString(),
                DiscountByTimeService.DiscountByTime,
                DiscountByTimeService.PopupText,
                DiscountByTimeService.ShowPopup
            });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult SaveDiscountByDatetimeData(bool enabled, string dateFrom, string dateTo, float percent, bool showPopup, string popupText)
        {
            DiscountByTimeService.Enabled = enabled;
            DiscountByTimeService.FromDateTime = dateFrom.TryParseDateTime();
            DiscountByTimeService.ToDateTime = dateTo.TryParseDateTime();
            DiscountByTimeService.DiscountByTime = percent;
            DiscountByTimeService.PopupText = popupText;
            DiscountByTimeService.ShowPopup = showPopup;

            CacheManager.Clean();

            return Json(new {result = true});
        }
        
        #endregion
    }
}
