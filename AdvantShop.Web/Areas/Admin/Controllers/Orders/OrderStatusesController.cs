using System;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.OrderStatuses;
using AdvantShop.Web.Admin.Models.OrderStatuses;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Orders
{
    [Auth(RoleAction.Orders)]
    [SaasFeature(Saas.ESaasProperty.OrderStatuses)]
    public partial class OrderStatusesController : BaseAdminController
    {
        public ActionResult Index()
        {
            SetMetaInformation(T("Admin.OrderStatuses.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.OrderStatusesCtrl);

            return View();
        }

        public JsonResult GetStatuses(OrderStatusesFilterModel model)
        {
            var handler = new GetOrderStatusesHandler(model);
            var result = handler.Execute();

            return Json(result);
        }

        #region Commands

        private void Command(OrderStatusesFilterModel model, Func<int, OrderStatusesFilterModel, bool> func)
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
                var handler = new GetOrderStatusesHandler(model);
                var ids = handler.GetItemsIds("OrderStatusId");

                foreach (int id in ids)
                {
                    if (model.Ids == null || !model.Ids.Contains(id))
                        func(id, model);
                }
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteOrderSatuses(OrderStatusesFilterModel model)
        {
            Command(model, (id, c) => OrderStatusService.DeleteOrderStatus(id));
            return Json(true);
        }

        #endregion

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteOrderStatus(int orderStatusId)
        {
            var result = OrderStatusService.DeleteOrderStatus(orderStatusId);
            return Json(result);
        }


        #region Add | Edit status

        [HttpGet]
        public JsonResult GetOrderStatus(int orderStatusId)
        {
            var status = OrderStatusService.GetOrderStatus(orderStatusId);
            return Json(status);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddOrderStatus(OrderStatusModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { result = false });

            try
            {
                var status = new OrderStatus()
                {
                    StatusName = model.StatusName.DefaultOrEmpty(),
                    IsDefault = model.IsDefault,
                    IsCanceled = model.IsCanceled,
                    IsCompleted = model.IsCompleted,
                    Color = model.Color.Replace("#", ""),
                    Hidden = model.Hidden,
                    Command = model.Command,
                    SortOrder = model.SortOrder,
                };

                OrderStatusService.AddOrderStatus(status);
            }
            catch (Exception ex)
            {
                Debug.Log.Error("", ex);
                return Json(new { result = false });
            }

            return Json(new { result = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UpdateOrderStatus(OrderStatusModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { result = false });

            try
            {
                var status = OrderStatusService.GetOrderStatus(model.OrderStatusId);

                status.StatusName = model.StatusName.DefaultOrEmpty();
                status.IsDefault = model.IsDefault;
                status.IsCanceled = model.IsCanceled;
                status.IsCompleted = model.IsCompleted;
                status.Color = model.Color.Replace("#", "");
                status.Hidden = model.Hidden;
                status.Command = model.Command;
                status.SortOrder = model.SortOrder;

                OrderStatusService.UpdateOrderStatus(status);
            }
            catch (Exception ex)
            {
                Debug.Log.Error("", ex);
                return Json(new { result = false });
            }

            return Json(new { result = true });
        }

        public JsonResult GetCommands()
        {
            return Json(Enum.GetValues(typeof(OrderStatusCommand)).Cast<OrderStatusCommand>().Select(x => new
            {
                label = x.Localize(),
                value = (int)x,
            }));
        }

        #endregion
    }
}
