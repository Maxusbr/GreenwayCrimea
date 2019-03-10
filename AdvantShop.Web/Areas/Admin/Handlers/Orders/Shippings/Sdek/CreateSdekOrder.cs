using System;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;
using AdvantShop.Shipping;
using AdvantShop.Shipping.Sdek;
using AdvantShop.Web.Infrastructure.ActionResults;

namespace AdvantShop.Web.Admin.Handlers.Orders.Shippings
{
    public class CreateSdekOrder
    {
        private readonly int _orderId;

        public CreateSdekOrder(int orderId)
        {
            _orderId = orderId;
        }

        public CommandResult Execute()
        {
            var order = OrderService.GetOrder(_orderId);
            if (order == null)
                return new CommandResult() {Error = "Order is null"};
            
            var shippingMethod = ShippingMethodService.GetShippingMethod(order.ShippingMethodId);
            if (shippingMethod.ShippingType != "Sdek")
                return new CommandResult() { Error = "Order shipping method is not 'Sdek' type" };
            
            try
            {
                var tariffId = shippingMethod.Params[SdekTemplate.Tariff].TryParseInt();

                var result = (new Sdek(shippingMethod, null)).SendNewOrders(order, tariffId);
                
                return result.Status
                    ? new CommandResult() {Result = result.Status, Message = result.Message }
                    : new CommandResult() {Error = result.Message };
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return new CommandResult() { Error = "Не удалось создать заказ: " + ex.Message };
            }
        }
    }
}
