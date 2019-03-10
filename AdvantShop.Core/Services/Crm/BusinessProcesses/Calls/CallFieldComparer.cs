using System.Linq;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Services.IPTelephony;
using AdvantShop.Customers;

namespace AdvantShop.Core.Services.Crm.BusinessProcesses
{
    public enum ECallFieldType
    {
        [Localize("Core.Crm.ECallFieldType.None")]
        None = 0,
        [Localize("Core.Crm.ECallFieldType.LastName"), FieldType(EFieldType.Text)]
        LastName = 1,
        [Localize("Core.Crm.ECallFieldType.FirstName"), FieldType(EFieldType.Text)]
        FirstName = 2,
        [Localize("Core.Crm.ECallFieldType.Patronymic"), FieldType(EFieldType.Text)]
        Patronymic = 3,
        [Localize("Core.Crm.ECallFieldType.CustomerGroup"), FieldType(EFieldType.Select)]
        CustomerGroup = 4,
        [Localize("Core.Crm.ECallFieldType.CustomerField")]
        CustomerField = 5,
        [Localize("Core.Crm.ECallFieldType.Email"), FieldType(EFieldType.Text)]
        Email = 6,
        [Localize("Core.Crm.ECallFieldType.Country"), FieldType(EFieldType.Text)]
        Country = 7,
        [Localize("Core.Crm.ECallFieldType.Region"), FieldType(EFieldType.Text)]
        Region = 8,
        [Localize("Core.Crm.ECallFieldType.City"), FieldType(EFieldType.Text)]
        City = 9,
        [Localize("Core.Crm.ECallFieldType.Phone"), FieldType(EFieldType.Text)]
        Phone = 10,
    }

    public class CallFieldComparer : IBizObjectFieldComparer<Call>
    {
        public ECallFieldType FieldType { get; set; }

        public FieldComparer FieldComparer { get; set; }

        public bool CheckField(Call call)
        {
            switch (FieldType)
            {
                case ECallFieldType.LastName:
                    return call.Customers.Any(x => FieldComparer.Check(x.LastName));
                case ECallFieldType.FirstName:
                    return call.Customers.Any(x => FieldComparer.Check(x.FirstName));
                case ECallFieldType.Patronymic:
                    return call.Customers.Any(x => FieldComparer.Check(x.Patronymic));
                case ECallFieldType.CustomerGroup:
                    return call.Customers.Any(x => FieldComparer.Check(x.CustomerGroupId));
                case ECallFieldType.CustomerField:
                    if (!FieldComparer.FieldObjId.HasValue)
                        return true;
                    foreach (var customer in call.Customers)
                    {
                        var customerField = CustomerFieldService.GetCustomerFieldsWithValue(customer.Id).FirstOrDefault(x => x.Id == FieldComparer.FieldObjId.Value);
                        if (customerField == null)
                            return false;
                        if (customerField.FieldType == CustomerFieldType.Date)
                        {
                            var date = customerField.Value.TryParseDateTime(true);
                            return date.HasValue && FieldComparer.Check(date.Value);
                        }
                        if (customerField != null && FieldComparer.Check(customerField.Value))
                            return true;
                    }
                    return false;
                case ECallFieldType.Email:
                    return call.Customers.Any(x => FieldComparer.Check(x.EMail));
                case ECallFieldType.Country:
                    return call.Customers.Any(x => x.Contacts.Any(contact => FieldComparer.Check(contact.Country)));
                case ECallFieldType.Region:
                    return call.Customers.Any(x => x.Contacts.Any(contact => FieldComparer.Check(contact.Region)));
                case ECallFieldType.City:
                    return call.Customers.Any(x => x.Contacts.Any(contact => FieldComparer.Check(contact.City)));
                case ECallFieldType.Phone:
                    return FieldComparer.Check(call.Phone) || call.Customers.Any(x => FieldComparer.Check(x.Phone) || (x.StandardPhone.HasValue && FieldComparer.Check(x.StandardPhone.Value.ToString())));
                default:
                    return false;
            }
        }

        private string _fieldName;
        public string FieldName
        {
            get
            {
                if (_fieldName != null)
                    return _fieldName;
                switch (FieldType)
                {
                    case ECallFieldType.CustomerField:
                        CustomerField customerField;
                        if (FieldComparer != null && FieldComparer.FieldObjId.HasValue &&
                            (customerField = CustomerFieldService.GetCustomerField(FieldComparer.FieldObjId.Value)) != null)
                        {
                            _fieldName = customerField.Name;
                        }
                        else
                            _fieldName = FieldType.Localize();
                        break;
                    default:
                        _fieldName = FieldType.Localize();
                        break;
                }
                return _fieldName;
            }
        }

        private string _fieldValueObjectName;
        public string FieldValueObjectName
        {
            get
            {
                if (_fieldValueObjectName != null)
                    return _fieldValueObjectName;

                if (FieldComparer == null || !FieldComparer.ValueObjId.HasValue)
                {
                    _fieldValueObjectName = string.Empty;
                    return _fieldValueObjectName;
                }

                var fieldValueObjId = FieldComparer.ValueObjId.Value;
                switch (FieldType)
                {
                    case ECallFieldType.CustomerGroup:
                        var customerGroup = CustomerGroupService.GetCustomerGroup(fieldValueObjId);
                        _fieldValueObjectName = customerGroup != null ? customerGroup.GroupName : string.Empty;
                        break;
                    default:
                        _fieldValueObjectName = string.Empty;
                        break;
                }

                return _fieldValueObjectName;
            }
        }

        public bool IsValid()
        {
            if (FieldComparer == null)
                return false;
            if (!FieldComparer.ValueObjId.HasValue)
                return true;

            var fieldValueObjId = FieldComparer.ValueObjId.Value;
            switch (FieldType)
            {
                case ECallFieldType.CustomerGroup:
                    return CustomerGroupService.GetCustomerGroup(fieldValueObjId) != null;
            }
            return true;
        }
    }
}
