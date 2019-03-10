using System;
using System.Linq;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Customers;
using AdvantShop.Orders;
using AdvantShop.Payment;
using AdvantShop.Shipping;

namespace AdvantShop.Core.Services.Crm.BusinessProcesses
{
    public enum EOrderFieldType
    {
        [Localize("Core.Crm.EOrderFieldType.None")]
        None = 0,
        [Localize("Core.Crm.EOrderFieldType.LastName"), FieldType(EFieldType.Text)]
        LastName = 1,
        [Localize("Core.Crm.EOrderFieldType.FirstName"), FieldType(EFieldType.Text)]
        FirstName = 2,
        [Localize("Core.Crm.EOrderFieldType.Patronymic"), FieldType(EFieldType.Text)]
        Patronymic = 3,
        [Localize("Core.Crm.EOrderFieldType.CustomerGroup"), FieldType(EFieldType.Select)]
        CustomerGroup = 4,
        [Localize("Core.Crm.EOrderFieldType.CustomerField")]
        CustomerField = 5,
        [Localize("Core.Crm.EOrderFieldType.Email"), FieldType(EFieldType.Text)]
        Email = 6,
        [Localize("Core.Crm.EOrderFieldType.Phone"), FieldType(EFieldType.Text)]
        Phone = 7,
        [Localize("Core.Crm.EOrderFieldType.Country"), FieldType(EFieldType.Text)]
        Country = 8,
        [Localize("Core.Crm.EOrderFieldType.Region"), FieldType(EFieldType.Text)]
        Region = 9,
        [Localize("Core.Crm.EOrderFieldType.City"), FieldType(EFieldType.Text)]
        City = 10,
        [Localize("Core.Crm.EOrderFieldType.OrderSource"), FieldType(EFieldType.Select)]
        OrderSource = 11,
        [Localize("Core.Crm.EOrderFieldType.OrderSum"), FieldType(EFieldType.Number)]
        OrderSum = 12,
        [Localize("Core.Crm.EOrderFieldType.PaymetMethod"), FieldType(EFieldType.Select)]
        PaymentMethod = 13,
        [Localize("Core.Crm.EOrderFieldType.ShippingMethod"), FieldType(EFieldType.Select)]
        ShippingMethod = 14,
        [Localize("Core.Crm.EOrderFieldType.IsFromLead"), FieldType(EFieldType.Checkbox)]
        IsFromLead = 15,
        [Localize("Core.Crm.EOrderFieldType.IsFromAdminArea"), FieldType(EFieldType.Checkbox)]
        IsFromAdminArea = 16,
    }

    public class OrderFieldComparer : IBizObjectFieldComparer<Order>
    {
        public EOrderFieldType FieldType { get; set; }

        public FieldComparer FieldComparer { get; set; }

        public bool CheckField(Order order)
        {
            switch (FieldType)
            {
                case EOrderFieldType.LastName:
                    return FieldComparer.Check(order.OrderCustomer.LastName);
                case EOrderFieldType.FirstName:
                    return FieldComparer.Check(order.OrderCustomer.FirstName);
                case EOrderFieldType.Patronymic:
                    return FieldComparer.Check(order.OrderCustomer.Patronymic);
                case EOrderFieldType.CustomerGroup:
                    var customer = CustomerService.GetCustomer(order.OrderCustomer.CustomerID);
                    return customer != null && FieldComparer.Check(customer.CustomerGroupId);
                case EOrderFieldType.CustomerField:
                    if (!FieldComparer.FieldObjId.HasValue)
                        return false;
                    var customerField = CustomerFieldService.GetCustomerFieldsWithValue(order.OrderCustomer.CustomerID)
                        .FirstOrDefault(x => x.Id == FieldComparer.FieldObjId.Value);
                    if (customerField == null)
                        return false;
                    if (customerField.FieldType == CustomerFieldType.Date)
                    {
                        var date = customerField.Value.TryParseDateTime(true);
                        return date.HasValue && FieldComparer.Check(date.Value);
                    }
                    return FieldComparer.Check(customerField.Value);
                case EOrderFieldType.Email:
                    return FieldComparer.Check(order.OrderCustomer.Email);
                case EOrderFieldType.Phone:
                    return FieldComparer.Check(order.OrderCustomer.Phone) ||
                        (order.OrderCustomer.StandardPhone.HasValue && FieldComparer.Check(order.OrderCustomer.StandardPhone.Value.ToString()));
                case EOrderFieldType.Country:
                    return FieldComparer.Check(order.OrderCustomer.Country);
                case EOrderFieldType.Region:
                    return FieldComparer.Check(order.OrderCustomer.Region);
                case EOrderFieldType.City:
                    return FieldComparer.Check(order.OrderCustomer.City);
                case EOrderFieldType.OrderSource:
                    return FieldComparer.Check(order.OrderSourceId);
                case EOrderFieldType.OrderSum:
                    return FieldComparer.Check(order.Sum);
                case EOrderFieldType.PaymentMethod:
                    return FieldComparer.Check(order.PaymentMethodId);
                case EOrderFieldType.ShippingMethod:
                    return FieldComparer.Check(order.ShippingMethodId);
                case EOrderFieldType.IsFromLead:
                    return FieldComparer.Check(order.LeadId.HasValue);
                case EOrderFieldType.IsFromAdminArea:
                    return FieldComparer.Check(order.IsFromAdminArea);
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
                    case EOrderFieldType.CustomerField:
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
                    case EOrderFieldType.CustomerGroup:
                        var customerGroup = CustomerGroupService.GetCustomerGroup(fieldValueObjId);
                        _fieldValueObjectName = customerGroup != null ? customerGroup.GroupName : string.Empty;
                        break;
                    case EOrderFieldType.OrderSource:
                        var orderSource = OrderSourceService.GetOrderSource(fieldValueObjId);
                        _fieldValueObjectName = orderSource != null ? orderSource.Name : string.Empty;
                        break;
                    case EOrderFieldType.PaymentMethod:
                        var paymetMethod = PaymentService.GetPaymentMethod(fieldValueObjId);
                        _fieldValueObjectName = paymetMethod != null ? paymetMethod.Name : string.Empty;
                        break;
                    case EOrderFieldType.ShippingMethod:
                        var shippingMethod = ShippingMethodService.GetShippingMethod(fieldValueObjId);
                        _fieldValueObjectName = shippingMethod != null ? shippingMethod.Name : string.Empty;
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
                case EOrderFieldType.CustomerGroup:
                    return CustomerGroupService.GetCustomerGroup(fieldValueObjId) != null;
                case EOrderFieldType.OrderSource:
                    return OrderSourceService.GetOrderSource(fieldValueObjId) != null;
                case EOrderFieldType.PaymentMethod:
                    return PaymentService.GetPaymentMethod(fieldValueObjId) != null;
                case EOrderFieldType.ShippingMethod:
                    return ShippingMethodService.GetShippingMethod(fieldValueObjId) != null;
            }
            return true;
        }
    }
}
