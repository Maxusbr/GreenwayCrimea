using System.Collections.Generic;
using System.Linq;
using AdvantShop.Catalog;

namespace AdvantShop.Web.Admin.Handlers.Products
{
    public class AddPropertyValue
    {
        private readonly int _productId;
        private readonly int _propertyId;
        private readonly int? _propertyValueId;
        private readonly string _value;
        private readonly bool _isNew;
        private readonly List<PropertyValue> _productPropertyValues;

        public AddPropertyValue(int productId, int propertyId, int? propertyValueId, string value, bool isNew)
        {
            _productId = productId;
            _propertyId = propertyId;
            _propertyValueId = propertyValueId;
            _value = value != null ? value.Trim() : "";
            _isNew = isNew;

            _productPropertyValues = PropertyService.GetPropertyValuesByProductId(_productId);
        }

        public int Execute()
        {
            var property = PropertyService.GetPropertyById(_propertyId);
            if (property == null)
                return 0;

            var propertyValueId = GetPropertyValueId();
            if (propertyValueId == 0)
                return 0;

            var propertyValue = _productPropertyValues.FirstOrDefault(x => x.PropertyValueId == propertyValueId);
            if (propertyValue == null)
            {
                PropertyService.AddProductProperyValue(propertyValueId, _productId);
            }

            return propertyValueId;
        }

        private int GetPropertyValueId()
        {
            if (_propertyValueId != null)
            {
                var value = PropertyService.GetPropertyValueById(_propertyValueId.Value);
                if (value != null)
                    return value.PropertyValueId;
            }

            if (_isNew && !string.IsNullOrWhiteSpace(_value))
            {
                var propertyValue = _productPropertyValues.FirstOrDefault(x => x.Value == _value);
                if (propertyValue != null)
                    return propertyValue.PropertyValueId;

                return
                    PropertyService.AddPropertyValue(new PropertyValue()
                    {
                        PropertyId = _propertyId,
                        Value = _value
                    });

            }
            return 0;
        }
    }
}
