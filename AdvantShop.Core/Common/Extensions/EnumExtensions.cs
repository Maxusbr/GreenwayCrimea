using System;
using System.Collections.Generic;
using System.Linq;
using AdvantShop.Core.Common.Attributes;
using AdvantShop.Core.Services.Crm.BusinessProcesses;
using AdvantShop.ExportImport;

namespace AdvantShop.Core.Common.Extensions
{
    public static class EnumExtensions
    {
        [Obsolete]
        public static string ResourceKey(this Enum enumValue)
        {
            return AttributeHelper.GetAttributeValueField<LocalizeAttribute, string>(enumValue);  // ResourceKeyAttribute
        }

        public static string Localize(this Enum enumValue)
        {
            return AttributeHelper.GetAttributeValueField<LocalizeAttribute, string>(enumValue);
        }

        public static string StrName(this Enum enumValue)
        {
            var attrValue = AttributeHelper.GetAttributeValueField<StringNameAttribute, string>(enumValue);
            return string.IsNullOrEmpty(attrValue) ? enumValue.ToString().ToLower() : attrValue;
        }

        public static CsvFieldStatus Status(this ProductFields enumValue)
        {
            var attrValue = AttributeHelper.GetAttributeValueField<CsvFieldsStatusAttribute, CsvFieldStatus>(enumValue);
            return attrValue;
        }

        public static CsvFieldStatus Status(this CategoryFields enumValue)
        {
            var attrValue = AttributeHelper.GetAttributeValueField<CsvFieldsStatusAttribute, CsvFieldStatus>(enumValue);
            return attrValue;
        }

        public static CsvFieldStatus Status(this ECustomerFields enumValue)
        {
            var attrValue = AttributeHelper.GetAttributeValueField<CsvFieldsStatusAttribute, CsvFieldStatus>(enumValue);
            return attrValue;
        }

        public static string ColorValue(this Enum enumValue)
        {
            return AttributeHelper.GetAttributeValueField<ColorAttribute, string>(enumValue);
        }

        public static EFieldType FieldType(this Enum enumValue)
        {
            return AttributeHelper.GetAttributeValueField<FieldTypeAttribute, EFieldType>(enumValue);
        }

        public static Dictionary<TEnum, string> ToDictionary<TEnum>()
        {
            var values = (from TEnum e in Enum.GetValues(typeof(TEnum))
                          select new { Id = e, Name = e.ToString() }).ToDictionary(x => x.Id, x => x.Name);
            return values;
        }
    }
}