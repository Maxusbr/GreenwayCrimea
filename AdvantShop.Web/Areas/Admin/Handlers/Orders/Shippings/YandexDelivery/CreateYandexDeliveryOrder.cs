using System;
using System.Linq;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;
using AdvantShop.Shipping;
using AdvantShop.Shipping.ShippingYandexDelivery;
using AdvantShop.Web.Infrastructure.ActionResults;

namespace AdvantShop.Web.Admin.Handlers.Orders.Shippings
{
    public class CreateYandexDeliveryOrder
    {
        private readonly int _orderId;

        public CreateYandexDeliveryOrder(int orderId)
        {
            _orderId = orderId;
        }

        public CommandResult Execute()
        {
            var order = OrderService.GetOrder(_orderId);
            if (order == null)
                return new CommandResult() {Error = "Order is null"};
            
            var shippingMethod = ShippingMethodService.GetShippingMethod(order.ShippingMethodId);
            if (shippingMethod.ShippingType != "YandexDelivery")
                return new CommandResult() { Error = "Order shipping method is not 'YandexDelivery' type" };
            
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

                var yandexDelivery = new YandexDelivery(shippingMethod, preOrder);
                var result = yandexDelivery.CreateOrder(order);
                
                return result
                    ? new CommandResult() {Result = true, Message = "Черновик заказа успешно создан"}
                    : new CommandResult() {Error = "Не удалось создать черновик заказа"};
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return new CommandResult() { Error = "Не удалось создать черновик заказа: " + ex.Message };
            }
        }
    }
}
