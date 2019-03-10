using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Services.Shipping.Grastin.Api;
using AdvantShop.FilePath;
using AdvantShop.Helpers;
using AdvantShop.Orders;
using AdvantShop.Shipping;
using AdvantShop.Web.Admin.Models.Grastin;

namespace AdvantShop.Web.Admin.Handlers.Orders.Shippings
{
    public class GrastinSendRequestForAct
    {
        private readonly SendRequestForAct _model;

        public List<string> Errors { get; set; }

        public GrastinSendRequestForAct(SendRequestForAct model)
        {
            _model = model;
        }

        public string Execute()
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

                    var request = new ActContainer()
                    {
                        ContractId = _model.ContractId,
                        //OrderNumber = string.Format("{0}{1}", grastinMethod.OrderPrefix, order.Number),
                        Orders =
                            new List<ActOrder>()
                            {
                                new ActOrder()
                                {
                                    OrderNumber = string.Format("{0}{1}", grastinMethod.OrderPrefix, order.Number),
                                    Seats = _model.Seats,
                                }
                            },
                    };

                    var path = FoldersHelper.GetPathAbsolut(FolderType.PriceTemp);
                    FileHelpers.CreateDirectory(path);

                    var result = service.GetAct(request, path);
                    Errors = service.LastActionErrors;

                    return result;
                }
            }

            return null;
        }
    }
}
