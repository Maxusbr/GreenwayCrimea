using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Services.Shipping.Grastin.Api;
using AdvantShop.Orders;
using AdvantShop.Shipping;
using AdvantShop.Web.Admin.Models.Grastin;

namespace AdvantShop.Web.Admin.Handlers.Orders.Shippings
{
    public class GrastinGetFormSendRequestForIntake
    {
        private readonly int _orderId;

        public GrastinGetFormSendRequestForIntake(int orderId)
        {
            _orderId = orderId;
        }

        public SendRequestForIntake Execute()
        {
            var order = OrderService.GetOrder(_orderId);
            if (order == null)
                return null;

            var model = new SendRequestForIntake()
            {
                OrderId = order.OrderID,
                Times = new List<SelectListItem>()
                {
                    new SelectListItem() { Text = "15-17", Value = "15-17"},
                    new SelectListItem() { Text = "16-18", Value = "16-18"},
                    new SelectListItem() { Text = "17-19", Value = "17-19"},
                    new SelectListItem() { Text = "18-20", Value = "18-20"},
                    new SelectListItem() { Text = "19-21", Value = "19-21"},
                },
                Volumes = new List<SelectListItem>()
                {
                    new SelectListItem() { Text = "До 0.3 м3", Value = "До 0.3 м3"},
                    new SelectListItem() { Text = "До 0.6 м3", Value = "До 0.6 м3"},
                    new SelectListItem() { Text = "Более 0.7 м3", Value = "Более 0.7 м3"},
                },
            };

            var shippingMethod = ShippingMethodService.GetShippingMethod(order.ShippingMethodId);
            if (shippingMethod != null &&
                shippingMethod.ShippingType ==
                ((ShippingKeyAttribute)
                    typeof (Shipping.Grastin.Grastin).GetCustomAttributes(typeof (ShippingKeyAttribute), false).First())
                    .Value)
            {
                var grastinMethod = new Shipping.Grastin.Grastin(shippingMethod, null);

                var service = new GrastinApiService(grastinMethod.ApiKey);

                var deliveryRegions = service.GetDeliveryRegions();
                model.Regions = deliveryRegions != null
                    ? deliveryRegions.Select(x => new SelectListItem()
                    {
                        Text = x.Name,
                        Value = x.Id,
                        Selected = x.Name == grastinMethod.WidgetFromCity
                    }).ToList()
                    : new List<SelectListItem>();
            }

            return model;
        }
    }
}
