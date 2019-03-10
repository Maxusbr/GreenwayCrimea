using AdvantShop.Customers;
using AdvantShop.Web.Admin.Models.CustomerFieldValues;

namespace AdvantShop.Web.Admin.Handlers.CustomerFieldValues
{
    public class GetCustomerFieldValueModel
    {
        private readonly CustomerFieldValue _customerFieldValue;

        public GetCustomerFieldValueModel(CustomerFieldValue customerFieldValue)
        {
            _customerFieldValue = customerFieldValue;
        }

        public AdminCustomerFieldValueModel Execute()
        {
            var model = new AdminCustomerFieldValueModel
            {
                Id = _customerFieldValue.Id,
                CustomerFieldId = _customerFieldValue.CustomerFieldId,
                Value = _customerFieldValue.Value,
                SortOrder = _customerFieldValue.SortOrder,
            };

            return model;
        }
    }
}
