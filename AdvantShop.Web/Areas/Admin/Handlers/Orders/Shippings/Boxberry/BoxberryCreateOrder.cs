using System;
using System.Linq;
using AdvantShop.Diagnostics;
using AdvantShop.Orders;
using AdvantShop.Shipping;

using AdvantShop.Web.Infrastructure.ActionResults;
using AdvantShop.Shipping.Boxberry;

namespace AdvantShop.Web.Admin.Handlers.Orders.Shippings
{
    public class BoxberryCreateOrder
    {
        private readonly int _orderId;

        public BoxberryCreateOrder(int orderId)
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
                var preOrder = new PreOrder()
                {
                    CountryDest = order.OrderCustomer.Country,
                    CityDest = order.OrderCustomer.City,
                    ZipDest = order.OrderCustomer.Zip,
                    ShippingOption = new BaseShippingOption(shippingMethod),
                    Currency = order.OrderCurrency,
                    Items = order.OrderItems.Select(x => new PreOrderItem(x)).ToList()

            };
                var boxberry = new Boxberry(shippingMethod, preOrder);
                var result = boxberry.CreateOrUpdateOrder(order);

                if (result != null && !string.IsNullOrEmpty(result.Error))
                {
                    return new CommandResult() { Result = false, Error = result.Error };
                }
                else if (result != null && !string.IsNullOrEmpty(result.TrackNumber))
                {
                    order.TrackNumber = result.TrackNumber;
                    OrderService.UpdateOrderMain(order);

                    return new CommandResult() { Result = true, Message = "Черновик заказа успешно создан.", Obj = result.TrackNumber };
                }
            }
            catch (Exception ex)
            {
                Debug.Log.Error(ex);
                return new CommandResult() { Error = "Не удалось создать черновик заказа: " + ex.Message };
            }

            return new CommandResult() { Error = "Не удалось создать черновик заказа." };
        }
    }
}
