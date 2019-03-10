using System.Linq;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Handlers.CustomerFieldValues;
using AdvantShop.Web.Admin.Models.CustomerFields;

namespace AdvantShop.Web.Admin.Handlers.CustomerFields
{
    public class GetCustomerFieldModel
    {
        private readonly CustomerField _customerField;

        public GetCustomerFieldModel(CustomerField customerField)
        {
            _customerField = customerField;
        }

        public AdminCustomerFieldModel Execute()
        {
            var model = new AdminCustomerFieldModel
            {
                Id = _customerField.Id,
                Name = _customerField.Name,
                FieldType = _customerField.FieldType,
                SortOrder = _customerField.SortOrder,
                Required = _customerField.Required,
                ShowInClient = _customerField.ShowInClient,
                Enabled = _customerField.Enabled
            };

            if (model.HasValues)
            {
                model.FieldValues = CustomerFieldService.GetCustomerFieldValues(model.Id)
                    .Select(x => new GetCustomerFieldValueModel(x).Execute()).ToList();
            }

            return model;
        }
    }
}
