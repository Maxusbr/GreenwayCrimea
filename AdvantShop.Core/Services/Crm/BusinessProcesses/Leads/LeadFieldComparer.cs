using System.Linq;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;
using AdvantShop.Orders;

namespace AdvantShop.Core.Services.Crm.BusinessProcesses
{
    public enum ELeadFieldType
    {
        [Localize("Core.Crm.ELeadFieldType.None")]
        None = 0,
        [Localize("Core.Crm.ELeadFieldType.LastName"), FieldType(EFieldType.Text)]
        LastName = 1,
        [Localize("Core.Crm.ELeadFieldType.FirstName"), FieldType(EFieldType.Text)]
        FirstName = 2,
        [Localize("Core.Crm.ELeadFieldType.Patronymic"), FieldType(EFieldType.Text)]
        Patronymic = 3,
        [Localize("Core.Crm.ELeadFieldType.CustomerGroup"), FieldType(EFieldType.Select)]
        CustomerGroup = 4,
        [Localize("Core.Crm.ELeadFieldType.CustomerField")]
        CustomerField = 5,
        [Localize("Core.Crm.ELeadFieldType.Email"), FieldType(EFieldType.Text)]
        Email = 6,
        [Localize("Core.Crm.ELeadFieldType.Phone"), FieldType(EFieldType.Text)]
        Phone = 7,
        [Localize("Core.Crm.ELeadFieldType.Country"), FieldType(EFieldType.Text)]
        Country = 8,
        [Localize("Core.Crm.ELeadFieldType.Region"), FieldType(EFieldType.Text)]
        Region = 9,
        [Localize("Core.Crm.ELeadFieldType.City"), FieldType(EFieldType.Text)]
        City = 10,
        [Localize("Core.Crm.ELeadFieldType.Source"), FieldType(EFieldType.Select)]
        Source = 11,
        [Localize("Core.Crm.ELeadFieldType.LeadSum"), FieldType(EFieldType.Number)]
        LeadSum = 12,
        [Localize("Core.Crm.ELeadFieldType.IsFromAdminArea"), FieldType(EFieldType.Checkbox)]
        IsFromAdminArea = 13,
        [Localize("Core.Crm.ELeadFieldType.Description"), FieldType(EFieldType.Text)]
        Description = 14,
    }

    public class LeadFieldComparer : IBizObjectFieldComparer<Lead>
    {
        public ELeadFieldType FieldType { get; set; }

        public FieldComparer FieldComparer { get; set; }

        public bool CheckField(Lead lead)
        {
            switch (FieldType)
            {
                case ELeadFieldType.LastName:
                    return FieldComparer.Check(lead.LastName) || (lead.Customer != null && FieldComparer.Check(lead.Customer.LastName));
                case ELeadFieldType.FirstName:
                    return FieldComparer.Check(lead.FirstName) || (lead.Customer != null && FieldComparer.Check(lead.Customer.FirstName));
                case ELeadFieldType.Patronymic:
                    return FieldComparer.Check(lead.Patronymic) || (lead.Customer != null && FieldComparer.Check(lead.Customer.Patronymic));
                case ELeadFieldType.CustomerGroup:
                    return lead.Customer != null && FieldComparer.Check(lead.Customer.CustomerGroupId);
                case ELeadFieldType.CustomerField:
                    if (!FieldComparer.FieldObjId.HasValue)
                        return true;
                    var customerField = lead.CustomerId.HasValue
                        ? CustomerFieldService.GetCustomerFieldsWithValue(lead.CustomerId.Value).FirstOrDefault(x => x.Id == FieldComparer.FieldObjId.Value)
                        : null;
                    if (customerField == null)
                        return false;
                    if (customerField.FieldType == CustomerFieldType.Date)
                    {
                        var date = customerField.Value.TryParseDateTime(true);
                        return date.HasValue && FieldComparer.Check(date.Value);
                    }
                    return FieldComparer.Check(customerField.Value);
                case ELeadFieldType.Email:
                    return FieldComparer.Check(lead.Email) || (lead.Customer != null && FieldComparer.Check(lead.Customer.EMail));
                case ELeadFieldType.Phone:
                    return FieldComparer.Check(lead.Phone) || (lead.Customer != null && 
                        (FieldComparer.Check(lead.Customer.Phone) || (lead.Customer.StandardPhone.HasValue && FieldComparer.Check(lead.Customer.StandardPhone.Value.ToString()))));
                case ELeadFieldType.Country:
                    return lead.Customer != null && lead.Customer.Contacts.Any(x => FieldComparer.Check(x.Country));
                case ELeadFieldType.Region:
                    return lead.Customer != null && lead.Customer.Contacts.Any(x => FieldComparer.Check(x.Region));
                case ELeadFieldType.City:
                    return lead.Customer != null && lead.Customer.Contacts.Any(x => FieldComparer.Check(x.City));
                case ELeadFieldType.Source:
                    return FieldComparer.Check(lead.OrderSourceId);
                case ELeadFieldType.LeadSum:
                    return FieldComparer.Check(lead.Sum);
                case ELeadFieldType.IsFromAdminArea:
                    return FieldComparer.Check(lead.IsFromAdminArea);
                case ELeadFieldType.Description:
                    return FieldComparer.Check(lead.Description);
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
                    case ELeadFieldType.CustomerField:
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
                    case ELeadFieldType.CustomerGroup:
                        var customerGroup = CustomerGroupService.GetCustomerGroup(fieldValueObjId);
                        _fieldValueObjectName = customerGroup != null ? customerGroup.GroupName : string.Empty;
                        break;
                    case ELeadFieldType.Source:
                        var orderSource = OrderSourceService.GetOrderSource(fieldValueObjId);
                        _fieldValueObjectName = orderSource != null ? orderSource.Name : string.Empty;
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
                case ELeadFieldType.CustomerGroup:
                    return CustomerGroupService.GetCustomerGroup(fieldValueObjId) != null;
                case ELeadFieldType.Source:
                    return OrderSourceService.GetOrderSource(fieldValueObjId) != null;
            }
            return true;
        }
    }
}
