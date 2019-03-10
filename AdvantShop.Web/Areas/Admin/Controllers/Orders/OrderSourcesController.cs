using System;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Orders;
using AdvantShop.Customers;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;
using AdvantShop.Web.Admin.Filters;
using AdvantShop.Web.Admin.Handlers.OrderSources;
using AdvantShop.Web.Admin.Models.OrderSources;
using AdvantShop.Web.Infrastructure.Admin;
using AdvantShop.Web.Infrastructure.Controllers;
using AdvantShop.Web.Infrastructure.Filters;

namespace AdvantShop.Web.Admin.Controllers.Orders
{
    [Auth(RoleAction.Orders)]
    public partial class OrderSourcesController : BaseAdminController
    {
        public ActionResult Index()
        {
            SetMetaInformation(T("Admin.OrderSources.Index.Title"));
            SetNgController(NgControllers.NgControllersTypes.OrderSourcesCtrl);

            return View();
        }


        public JsonResult GetOrderSources(OrderSourcesFilterModel model)
        {
            return Json(new GetOrderSourcesHandler(model).Execute());
        }

        #region Commands

        private void Command(OrderSourcesFilterModel model, Func<int, OrderSourcesFilterModel, bool> func)
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
                var handler = new GetOrderSourcesHandler(model);
                var ids = handler.GetItemsIds("Id");

                foreach (int id in ids)
                {
                    if (model.Ids == null || !model.Ids.Contains(id))
                        func(id, model);
                }
            }
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteOrderSources(OrderSourcesFilterModel model)
        {
            Command(model, (id, c) => OrderSourceService.DeleteOrderSource(id));
            return Json(true);
        }

        #endregion

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult DeleteOrderSource(int id)
        {
            var result = OrderSourceService.DeleteOrderSource(id);
            return Json(result);
        }

        

        #region Add | Edit source

        [HttpGet]
        public JsonResult GetOrderSource(int id)
        {
            var source = OrderSourceService.GetOrderSource(id);
            return Json(source);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult AddOrderSource(OrderSourceModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { result = false });

            try
            {
                var source = new OrderSource()
                {
                    Name = model.Name.DefaultOrEmpty(),
                    Main = model.Main,
                    Type = model.Type,
                    SortOrder = model.SortOrder,
                };

                OrderSourceService.AddOrderSource(source);
            }
            catch (Exception ex)
            {
                Debug.Log.Error("", ex);
                return Json(new { result = false });
            }

            return Json(new { result = true });
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public JsonResult UpdateOrderSource(OrderSourceModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { result = false });

            try
            {
                var source = OrderSourceService.GetOrderSource(model.Id);

                source.Name = model.Name.DefaultOrEmpty();
                source.Main = model.Main;
                source.Type = model.Type;
                source.SortOrder = model.SortOrder;

                OrderSourceService.UpdateOrderSource(source);
            }
            catch (Exception ex)
            {
                Debug.Log.Error("", ex);
                return Json(new { result = false });
            }

            return Json(new { result = true });
        }

        #endregion


        public JsonResult GetTypes()
        {
            return Json(Enum.GetValues(typeof(OrderType)).Cast<OrderType>().Select(x => new
            {
                label = x.Localize(),
                value = (int)x,
            }));
        }
    }
}
