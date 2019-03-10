using System;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;
using AdvantShop.Shipping;

using AdvantShop.Web.Infrastructure.ActionResults;
using AdvantShop.Shipping.Boxberry;

namespace AdvantShop.Web.Admin.Handlers.Orders.Shippings
{
    public class BoxberryDeleteOrder
    {
        private readonly int _orderId;

        public BoxberryDeleteOrder(int orderId)
        {
            _orderId = orderId;
        }

        public CommandResult Execute()
        {
            var order = OrderService.GetOrder(_orderId);
            if (order == null)
                return new CommandResult() { Error = "Order is null" };

            var shippingMethod = ShippingMethodService.GetShippingMethod(order.ShippingMethodId);
            if (shippingMethod.ShippingType != "Boxberry")
                return new CommandResult() { Error = "Order shipping method is not 'Boxberry' type" };

            try
            {
                if (string.IsNullOrEmpty(order.TrackNumber))
                {
                    return new CommandResult() { Result = false, Error = "Не найден заказ в системе Boxberry, нет номера отслеживания" };
                }

                var result = (new Boxberry(shippingMethod, null)).DeleteOrder(order.TrackNumber);

                if (result != null && !string.IsNullOrEmpty(result.Error))
                {
		    order.TrackNumber = string.Empty;
                    OrderService.UpdateOrderMain(order);
                    return new CommandResult() { Result = false, Error = result.Error };
                }
                else if (result != null && !string.IsNullOrEmpty(result.Result) && result.Result == "ok")
                {
                    order.TrackNumber = string.Empty;
                    OrderService.UpdateOrderMain(order);
                    return new CommandResult() { Result = true, Message = "Заказ успешно удален из системы Boxberry" };
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return new CommandResult() { Error = "Ошибка " + ex.Message };
            }

            return new CommandResult() { Result = false, Error = "Не удалось удалить заказ из системы Boxberry" };
        }
    }
}
