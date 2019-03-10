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
    public class DDeliveryCanselOrder
    {
        private readonly int _orderId;

        public DDeliveryCanselOrder(int orderId)
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
            
            var ddeliveryService = new DDeliveryService();

            var ddeliveryOrderNumber = ddeliveryService.GetDDeliveryOrderNumber(order.OrderID);
            if (string.IsNullOrEmpty(ddeliveryOrderNumber))
            {
                return new CommandResult()
                {
                    Result = false,
                    Message = "Заказ не найден",
                    Error = "Заказ не найден"
                };
            }

            try
            {              
                var dDelivery = new DDelivery(shippingMethod, null);
                var result = dDelivery.CanselOrder(ddeliveryOrderNumber);

                if (result.Success && result.Data != null)
                {
                    ddeliveryService.DeleteDDeliveryOrder(order.OrderID, ddeliveryOrderNumber);

                }
                
                return new CommandResult() { Result = result.Success, Message = result.Message, Obj = result.Data, Error = !result.Success ? result.Message : "" };

            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return new CommandResult() { Error = "Не удалось отменить заказ: " + ex.Message };
            }            
        }
    }
}
