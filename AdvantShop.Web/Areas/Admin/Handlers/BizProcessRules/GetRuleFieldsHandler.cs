using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Common.Extensions;
using AdvantShop.Core.Services.Crm.BusinessProcesses;
using AdvantShop.Customers;

namespace AdvantShop.Web.Admin.Handlers.BizProcessRules
{
    public class GetRuleFieldsHandler
    {
        public class FieldSelectItem
        {
            public int type { get; set; }
            public string name { get; set; }
            public int? objId { get; set; }
            public string fieldType { get; set; }

            public static explicit operator FieldSelectItem(Enum fieldType)
            {
                return new FieldSelectItem
                {
                    type = Convert.ToInt32(fieldType),
                    name = fieldType.Localize(),
                    fieldType = fieldType.FieldType().ToString().ToLower()
                };
            }

            public static explicit operator FieldSelectItem(CustomerField customerField)
            {
                var item = new FieldSelectItem
                {
                    type = (int)EOrderFieldType.CustomerField,
                    name = customerField.Name,
                    objId = customerField.Id
                };
                item.fieldType = customerField.FieldType.FieldType().ToString().ToLower();
                return item;
            }
        }

        private readonly EBizProcessEventType _eventType;

        public GetRuleFieldsHandler(EBizProcessEventType eventType)
        {
            _eventType = eventType;
        }

        public List<FieldSelectItem> Execute()
        {
            var fields = new List<FieldSelectItem>();
            switch (_eventType)
            {
                case EBizProcessEventType.OrderCreated:
                case EBizProcessEventType.OrderStatusChanged:
                    fields = new List<FieldSelectItem>(Enum.GetValues(typeof(EOrderFieldType)).Cast<EOrderFieldType>()
                        .Where(x => x != EOrderFieldType.None && x != EOrderFieldType.CustomerField)
                        .Select(x => (FieldSelectItem)x));
                    fields.InsertRange((int)EOrderFieldType.CustomerField, GetCustomerFieldsList());
                    break;
                case EBizProcessEventType.LeadCreated:
                case EBizProcessEventType.LeadStatusChanged:
                    fields = new List<FieldSelectItem>(Enum.GetValues(typeof(ELeadFieldType)).Cast<ELeadFieldType>()
                        .Where(x => x != ELeadFieldType.None && x != ELeadFieldType.CustomerField)
                        .Select(x => (FieldSelectItem)x));
                    fields.InsertRange((int)ELeadFieldType.CustomerField, GetCustomerFieldsList());
                    break;
                case EBizProcessEventType.CallMissed:
                    fields = new List<FieldSelectItem>(Enum.GetValues(typeof(ECallFieldType)).Cast<ECallFieldType>()
                        .Where(x => x != ECallFieldType.None && x != ECallFieldType.CustomerField)
                        .Select(x => (FieldSelectItem)x));
                    fields.InsertRange((int)ECallFieldType.CustomerField, GetCustomerFieldsList());
                    break;
                case EBizProcessEventType.ReviewAdded:
                    fields = new List<FieldSelectItem>();
                    break;
                case EBizProcessEventType.MessageReply:
                    fields = new List<FieldSelectItem>();
                    break;
                default:
                    throw new NotImplementedException("No implementation for event type " + _eventType);
            }
            return fields;
        }

        private List<FieldSelectItem> GetCustomerFieldsList()
        {
            return new List<FieldSelectItem>(CustomerFieldService.GetCustomerFields().Select(x => (FieldSelectItem)x));
        }
    }
}