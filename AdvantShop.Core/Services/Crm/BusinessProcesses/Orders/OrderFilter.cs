using System.Collections.Generic;
using AdvantShop.Orders;

namespace AdvantShop.Core.Services.Crm.BusinessProcesses
{
    public class OrderFilter : IBizObjectFilter
    {
        public OrderFilter()
        {
            Comparers = new List<OrderFieldComparer>();
        }

        public List<OrderFieldComparer> Comparers { get; set; }

        public bool Check(IBizObject bizObject)
        {
            var order = (Order)bizObject;

            foreach (var comparer in Comparers)
            {
                if (!comparer.CheckField(order))
                    return false;
            }
            // если не заданы условия - заказ подходит в любом случае
            return true;
        }
    }
}
