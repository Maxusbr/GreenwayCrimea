using System;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Customers;
using AdvantShop.Orders;

namespace AdvantShop.Core.Services.Orders
{
    public enum OrderHistoryParameterType
    {
        None = 0,
        Status = 1,
        Customer = 2,
        OrderItem = 3
    }

    public class OrderHistory
    {
        public int OrderId { get; set; }
        public string Parameter { get; set; }
        public OrderHistoryParameterType ParameterType { get; set; }
        public string ParameterValue { get; set; }
        public string ParameterDescription { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string ManagerName { get; set; }
        public Guid? ManagerId { get; set; }
        public Role CustomerRole { get; set; }
        public DateTime ModificationTime { get; set; }

        public OrderHistory()
        {
        }

        public OrderHistory(OrderChangedBy changedBy)
        {
            if (changedBy == null && CustomerContext.CurrentCustomer != null)
                changedBy = new OrderChangedBy(CustomerContext.CurrentCustomer);

            ManagerName = changedBy != null ? changedBy.Name : string.Empty;
            ManagerId = changedBy != null ? changedBy.CustomerId : default(Guid?);
            ModificationTime = changedBy != null ? changedBy.ModificationTime : DateTime.Now;
        }
    }

    public class OnRefreshTotalOrder
    {
        [Compare("Core.Orders.OnRefreshTotalOrder.Sum")]
        public float Sum { get; set; }

        [Compare("Core.Orders.OnRefreshTotalOrder.TaxCost")]
        public float TaxCost { get; set; }
        
        [Compare("Core.Orders.OnRefreshTotalOrder.BonusCost")]
        public float BonusCost { get; set; }
    }
}
