using System.Collections.Generic;
using AdvantShop.Core.Services.Crm.BusinessProcesses;
using AdvantShop.Orders;
using AdvantShop.Web.Admin.Models.Tasks;

namespace AdvantShop.Web.Admin.Handlers.BizProcesses
{
    public class BizProcessOrderHandler<TRule> : BizProcessHandler<TRule> where TRule : BizProcessOrderRule
    {
        private readonly Order _order;

        public BizProcessOrderHandler(List<TRule> rules, Order order) : base(rules, order)
        {
            _order = order;
        }

        public override TaskModel GenerateTask()
        {
            TaskModel = new TaskModel
            {
                OrderId = _order.OrderID
            };

            return base.GenerateTask();
        }

        public override void ProcessBizObject()
        {
            if (!_order.ManagerId.HasValue && Employee != null)
            {
                _order.ManagerId = Employee.AssociatedManagerId;
                OrderService.UpdateOrderManager(_order.OrderID, Employee.AssociatedManagerId);
            }
        }
    }
}
