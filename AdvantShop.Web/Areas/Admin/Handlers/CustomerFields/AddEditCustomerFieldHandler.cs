using System.Linq;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;
using AdvantShop.Web.Admin.Models.CustomerFields;

namespace AdvantShop.Web.Admin.Handlers.CustomerFields
{
    public class AddEditCustomerFieldHandler
    {
        private readonly AdminCustomerFieldModel _model;
        private bool _editMode;

        public AddEditCustomerFieldHandler(AdminCustomerFieldModel model, bool editMode)
        {
            _model = model;
            _editMode = editMode;
        }

        public CustomerField Execute()
        {
            var dbModel = new CustomerField
            {
                Name = _model.Name.DefaultOrEmpty().Trim(),
                FieldType = _model.FieldType,
                SortOrder = _model.SortOrder,
                Required = _model.Required,
                ShowInClient = _model.ShowInClient,
                Enabled = _model.Enabled,
            };

            if (!_editMode)
            {
                dbModel.Id = CustomerFieldService.AddCustomerField(dbModel);
            }
            else
            {
                dbModel.Id = _model.Id;
                CustomerFieldService.UpdateCustomerField(dbModel);
            }

            if (_editMode)
            {
                if (!_model.HasValues)
                    CustomerFieldService.DeleteCustomerFieldValues(dbModel.Id);
                else
                {
                    var prevFieldValues = CustomerFieldService.GetCustomerFieldValues(_model.Id);
                    foreach (var fV in prevFieldValues.Where(x => _model.FieldValues.FirstOrDefault(y => x.Id != 0 && y.Id == x.Id) == null))
                    {
                        CustomerFieldService.DeleteCustomerFieldValue(fV.Id);
                    }
                }
            }

            if (_model.HasValues)
            {
                int prevSort = 0;
                foreach (var fieldValueModel in _model.FieldValues)
                {
                    if (fieldValueModel.Value.IsNullOrEmpty())
                        continue;

                    var fieldValue = new CustomerFieldValue
                    {
                        Id = fieldValueModel.Id,
                        CustomerFieldId = dbModel.Id,
                        Value = fieldValueModel.Value.DefaultOrEmpty().Trim(),
                        SortOrder = prevSort += 10,
                    };
                    if (fieldValueModel.Id == 0)
                    {
                        CustomerFieldService.AddCustomerFieldValue(fieldValue);
                    }
                    else
                    {
                        CustomerFieldService.UpdateCustomerFieldValue(fieldValue);
                    }
                }
            }

            return dbModel;
        }
    }
}
