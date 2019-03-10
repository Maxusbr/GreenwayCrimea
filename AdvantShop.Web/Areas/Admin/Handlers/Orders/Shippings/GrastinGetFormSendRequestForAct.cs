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
    public class GrastinGetFormSendRequestForAct
    {
        private readonly int _orderId;

        public GrastinGetFormSendRequestForAct(int orderId)
        {
            _orderId = orderId;
        }

        public SendRequestForAct Execute()
        {
            var order = OrderService.GetOrder(_orderId);
            if (order == null)
                return null;

            var model = new SendRequestForAct()
            {
                OrderId = order.OrderID,
                Seats = 1
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

                var contracts = service.GetContracts();
                model.Contracts = contracts != null
                    ? contracts.Select(x => new SelectListItem()
                    {
                        Text = x.Name,
                        Value = x.Id,
                    }).ToList()
                    : new List<SelectListItem>();
            }

            return model;
        }
    }
}
