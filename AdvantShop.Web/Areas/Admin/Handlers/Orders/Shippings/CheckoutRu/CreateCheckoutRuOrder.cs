using System;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;
using AdvantShop.Shipping;
using AdvantShop.Shipping.CheckoutRu;
using AdvantShop.Web.Infrastructure.ActionResults;

namespace AdvantShop.Web.Admin.Handlers.Orders.Shippings
{
    public class CreateCheckoutRuOrder
    {
        private readonly int _orderId;

        public CreateCheckoutRuOrder(int orderId)
        {
            _orderId = orderId;
        }

        public CommandResult Execute()
        {
            var order = OrderService.GetOrder(_orderId);
            if (order == null)
                return new CommandResult() {Error = "Order is null"};
            
            var shippingMethod = ShippingMethodService.GetShippingMethod(order.ShippingMethodId);
            if (shippingMethod.ShippingType != "CheckoutRu")
                return new CommandResult() { Error = "Order shipping method is not 'CheckoutRu' type" };
            
            try
            {
                var response = (new CheckoutRu(shippingMethod, null)).CreateOrder(order);

                return !response.error
                    ? new CommandResult() {Result = true, Message = "Заказ добавлен в систему под номером " + response.order.id }
                    : new CommandResult() {Error = "Ошибка при добавлении заказа, ответ сервера : " + response.errorMessage };
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return new CommandResult() { Error = "Не удалось создать заказ: " + ex.Message };
            }
        }
    }
}
