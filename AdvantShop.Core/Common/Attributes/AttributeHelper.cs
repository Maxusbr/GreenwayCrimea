using System;
using System.Linq;

namespace AdvantShop.Core.Common.Attributes
{
    public static class AttributeHelper
    {
        public static TReturn GetAttributeValue<TAttribute, TReturn>(object obj)
        {
            var attributeValue = default(TReturn);
            if (obj == null) return attributeValue;
            return GetAttributeValue<TAttribute, TReturn>(obj.GetType());
        }

        public static TReturn GetAttributeValue<TAttribute, TReturn>(Type obj)
        {
            var attributeValue = default(TReturn);

            if (obj == null) return attributeValue;
            var attributes = Attribute.GetCustomAttributes(obj).Where(x => x is TAttribute).ToList();

            if (attributes.Count <= 0) return attributeValue;
            var attribute = attributes[0] as IAttribute<TReturn>;

            if (attribute != null)
            {
                attributeValue = attribute.Value;
            }
            return attributeValue;
        }

        public static TReturn GetAttributeValueField<TAttribute, TReturn>(object obj)
        {
            var attributeValue = default(TReturn);

            if (obj == null) return attributeValue;
            var fi = obj.GetType().GetField(obj.ToString());

            if (fi == null) return attributeValue;
            var attributes = fi.GetCustomAttributes(typeof(TAttribute), false) as TAttribute[];

            if (attributes == null || attributes.Length <= 0) return attributeValue;
            var attribute = attributes[0] as IAttribute<TReturn>;

            if (attribute != null)
            {
                attributeValue = attribute.Value;
            }
            return attributeValue;
        }
    }
}