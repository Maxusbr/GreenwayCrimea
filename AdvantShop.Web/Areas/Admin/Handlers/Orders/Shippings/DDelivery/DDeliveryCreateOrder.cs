using System;
using System.Linq;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;
using AdvantShop.Shipping;

using AdvantShop.Web.Infrastructure.ActionResults;
using AdvantShop.Shipping.DDelivery;
using AdvantShop.Core.Services.Shipping.DDelivery;

namespace AdvantShop.Web.Admin.Handlers.Orders.Shippings
{
    public class DDeliveryCreateOrder
    {
        private readonly int _orderId;

        public DDeliveryCreateOrder(int orderId)
        {
            _orderId = orderId;
        }

        public CommandResult Execute()
        {
            var order = OrderService.GetOrder(_orderId);
            if (order == null)
                return new CommandResult() { Error = "Order is null" };

            var shippingMethod = ShippingMethodService.GetShippingMethod(order.ShippingMethodId);
            if (shippingMethod.ShippingType != "DDelivery")
                return new CommandResult() { Error = "Order shipping method is not 'DDelivery' type" };


            var dDeliveryService = new DDeliveryService();

            var ddeliveryOrderNumber = dDeliveryService.GetDDeliveryOrderNumber(order.OrderID);
            if (!string.IsNullOrEmpty(ddeliveryOrderNumber))
            {
                return new CommandResult()
                {
                    Result = false,
                    Message = "Заказ уже существует в системе DDelivery, номер " + ddeliveryOrderNumber,
                    Error = "Заказ уже существует в системе DDelivery, номер " + ddeliveryOrderNumber
                };
            }

            try
            {
                var preOrder = new PreOrder()
                {
                    CountryDest = order.OrderCustomer.Country,
                    CityDest = order.OrderCustomer.City,
                    ZipDest = order.OrderCustomer.Zip,
                    ShippingOption = new BaseShippingOption(shippingMethod),
                    Currency = order.OrderCurrency,
                    Items = order.OrderItems.Select(x => new PreOrderItem(x)).ToList()
                };                

                var dDelivery = new DDelivery(shippingMethod, preOrder);
                var result = dDelivery.CreateOrder(order);

                if (result.Success && result.Data != null)
                {
                    dDeliveryService.AddDDeliveryOrder(order.OrderID, result.Data.OrderId);
                }
                
                return new CommandResult() { Result = result.Success, Message = result.Message, Obj = result.Data, Error = !result.Success ? result.Message : "" };

            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return new CommandResult() { Error = "Не удалось создать черновик заказа: " + ex.Message };
            }
        }
    }
}
