using System;
using System.Drawing;
using System.Linq;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Scheduler;
using AdvantShop.Core.Modules;
using AdvantShop.Orders;
using Quartz;

namespace AdvantShop.Module.MoySklad.Domain
{
    [DisallowConcurrentExecution]
    public class UpdateOrderStatusJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            if (!ModuleSettingsProvider.GetSettingValue<bool>("UpdateOrdersStatuses", MoySklad.GetModuleStringId())) return;

            if (!context.CanStart()) return;
            context.WriteLastRun();

            Order order;
            OrderStatus status;
            var statuses = OrderStatusService.GetOrderStatuses();
            foreach (var orderMS in MoySkladApiService.GetEnumeratorCustomerOrders(true))
            {
                if (orderMS.State != null && orderMS.ExternalCode.IsNotEmpty() && orderMS.ExternalCode.IsInt())
                {
                    order = OrderService.GetOrder(orderMS.ExternalCode.TryParseInt());
                    if (order != null && !order.OrderStatus.StatusName.Equals(orderMS.State.Name, StringComparison.InvariantCultureIgnoreCase))
                    {
                        status = statuses.FirstOrDefault(x => x.StatusName.Equals(orderMS.State.Name, StringComparison.InvariantCultureIgnoreCase));
                        if (status == null)
                        {
                            status = new OrderStatus
                            {
                                StatusName = orderMS.State.Name,
                                Color = ColorTranslator.ToHtml(Color.FromArgb(orderMS.State.Color)).Replace("#", ""),

                            };
                            status.StatusID = OrderStatusService.AddOrderStatus(status);

                            statuses.Add(status);
                        }

                        OrderStatusService.ChangeOrderStatus(order.OrderID, status.StatusID, "Обновление из MoySklad");
                    }
                }
            }
        }
    }
}
