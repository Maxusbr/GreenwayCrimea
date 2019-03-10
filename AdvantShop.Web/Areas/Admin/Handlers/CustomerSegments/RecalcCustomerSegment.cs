using AdvantShop.Core.Services.CustomerSegments;
using AdvantShop.Web.Admin.Models.CustomerSegments;

namespace AdvantShop.Web.Admin.Handlers.CustomerSegments
{
    /// <summary>
    /// Перерасчет пользователей сегмента
    /// </summary>
    public class RecalcCustomerSegment
    {
        private readonly int _segmentId;

        public RecalcCustomerSegment(int segmentId)
        {
            _segmentId = segmentId;
        }

        public void Execute()
        {
            CustomerSegmentService.DeleteCustomersRelation(_segmentId);

            var handler =
                new GetCustomersBySegment(new CustomersBySegmentFilterModel() {ItemsPerPage = 1000000, Id = _segmentId}, true);

            var customers = handler.Execute();

            if (customers.DataItems == null || customers.DataItems.Count <= 0)
                return;

            foreach (var customer in customers.DataItems)
            {
                CustomerSegmentService.AddCustomer(customer.CustomerId, _segmentId);
            }
        }
    }
}
