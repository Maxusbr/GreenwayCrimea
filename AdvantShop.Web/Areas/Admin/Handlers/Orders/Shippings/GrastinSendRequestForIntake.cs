using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Services.Shipping.Grastin.Api;
using AdvantShop.Orders;
using AdvantShop.Shipping;
using AdvantShop.Web.Admin.Models.Grastin;

namespace AdvantShop.Web.Admin.Handlers.Orders.Shippings
{
    public class GrastinSendRequestForIntake
    {
        private readonly SendRequestForIntake _model;

        public List<string> Errors { get; set; }

        public GrastinSendRequestForIntake(SendRequestForIntake model)
        {
            _model = model;
        }

        public bool Execute()
        {
            var order = OrderService.GetOrder(_model.OrderId);
            if (order != null)
            {
                var shippingMethod = ShippingMethodService.GetShippingMethod(order.ShippingMethodId);
                if (shippingMethod != null &&
                    shippingMethod.ShippingType ==
                    ((ShippingKeyAttribute)
                        typeof (Shipping.Grastin.Grastin).GetCustomAttributes(typeof (ShippingKeyAttribute), false)
                            .First())
                        .Value)
                {
                    var grastinMethod = new Shipping.Grastin.Grastin(shippingMethod, null);

                    var service = new GrastinApiService(grastinMethod.ApiKey);

                    var request = new RequestForIntakeContainer()
                    {
                        RegionId = _model.RegionId,
                        Time = _model.Time,
                        Volume = _model.Volume,
                    };

                    var result = service.AddRequestForIntake(request);
                    Errors = service.LastActionErrors;

                    return result;
                }
            }

            return false;
        }
    }
}
