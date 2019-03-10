//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------
using AdvantShop.Orders;

namespace AdvantShop.Core.Modules.Interfaces
{
    public interface ISendOrderNotifications
    {
        void SendOnOrderAdded(IOrder order);

        void SendOnOrderChangeStatus(IOrder order);

        /// <summary>
        /// Внимание! Данный метод может быть вызван несколько раз при одном изменении заказа
        /// </summary>
        void SendOnOrderUpdated(IOrder order);

        void SendOnOrderDeleted(int orderId);

        void SendOnPayOrder(int orderId, bool payed);

        bool HaveSmsTemplate(int orderStatusId);
    }
}