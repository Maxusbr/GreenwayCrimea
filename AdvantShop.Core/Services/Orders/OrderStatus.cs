using System;
using AdvantShop.Core.Common.Attributes;

namespace AdvantShop.Orders
{
    public enum OrderStatusCommand
    {
        [Localize("Core.Orders.OrderStatusCommand.None")]
        None,

        [Localize("Core.Orders.OrderStatusCommand.Increment")]
        Increment,

        [Localize("Core.Orders.OrderStatusCommand.Decrement")]
        Decrement
    }

    public interface IOrderStatus
    {
        int StatusID { get; set; }
        string StatusName { get; set; }
        bool IsDefault { get; set; }
        bool IsCanceled { get; set; }
    }

    public class OrderStatus : IOrderStatus
    {
        public int StatusID { get; set; }
        public string StatusName { get; set; }
        public OrderStatusCommand Command { get; set; }
        public bool IsDefault { get; set; }
        public bool IsCanceled { get; set; }
        public bool IsCompleted { get; set; }
        public bool Hidden { get; set; }
        public string Color { get; set; }
        public int SortOrder { get; set; }
    }

    public class OrderStatusHistory
    {
        public DateTime Date { get; set; }
        public int OrderID { get; set; }
        public string PreviousStatus { get; set; }
        public string NewStatus { get; set; }
        public Guid? CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string Basis { get; set; }
    }
}