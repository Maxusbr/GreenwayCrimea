using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Services.Shipping.Grastin.Api;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.Shipping;

namespace AdvantShop.Web.Admin.Handlers.Orders.Shippings
{
    public class GrastinSendRequestForMark
    {
        private readonly int _orderId;

        public List<string> Errors { get; set; }

        public GrastinSendRequestForMark(int orderId)
        {
            _orderId = orderId;
        }

        public string Execute()
        {
            var order = OrderService.GetOrder(_orderId);
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

                    var request = new MarkContainer()
                    {
                        Orders =
                            new List<MarkOrder>()
                            {
                                new MarkOrder()
                                {
                                    OrderNumber = string.Format("{0}{1}", grastinMethod.OrderPrefix, order.Number),
                                }
                            },
                    };

                    var path = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
                    FileHelpers.CreateDirectory(path);

                    var result = service.GetMark(request, path);
                    Errors = service.LastActionErrors;

                    return result;
                }
            }

            return null;
        }
    }
}
