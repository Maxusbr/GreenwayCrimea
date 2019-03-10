using System.Collections.Generic;
using System.Linq;
using AdvantShop.Catalog;

namespace AdvantShop.Web.Admin.Handlers.Products
{
    public class AddPropertyWithValue
    {
        private readonly int _productId;
        private readonly int? _propertyId;
        private readonly int? _propertyValueId;
        private readonly string _propertyName;
        private readonly string _propertyValue;
        private readonly List<PropertyValue> _productPropertyValues;

        public AddPropertyWithValue(int productId, int? propertyId, int? propertyValueId, string propertyName, string propertyValue)
        {
            _productId = productId;
            _propertyId = propertyId;
            _propertyValueId = propertyValueId;
            _propertyName = propertyName;
            _propertyValue = propertyValue;

            _productPropertyValues = PropertyService.GetPropertyValuesByProductId(_productId);
        }

        public int Execute()
        {
            var propertyId = GetPropertyId();
            if (propertyId == 0)
                return 0;

            var propertyValueId = GetPropertyValueId(propertyId);
            if (propertyValueId == 0)
                return 0;

            var propertyValue = _productPropertyValues.FirstOrDefault(x => x.PropertyValueId == propertyValueId);
            if (propertyValue == null)
            {
                PropertyService.AddProductProperyValue(propertyValueId, _productId);
            }

            return propertyValueId;
        }

        private int GetPropertyId()
        {
            if (_propertyId != null)
            {
                var property = PropertyService.GetPropertyById(_propertyId.Value);
                if (property != null)
                    return property.PropertyId;
            }

            if (!string.IsNullOrWhiteSpace(_propertyName))
                return PropertyService.AddProperty(new Property()
                {
                    Name = _propertyName.Trim(),
                    UseInFilter = true,
                    UseInDetails = true,
                    Type = 1
                });

            return 0;
        }

        private int GetPropertyValueId(int propertyId)
        {
            if (_propertyValueId != null)
            {
                var value = PropertyService.GetPropertyValueById(_propertyValueId.Value);
                if (value != null)
                    return value.PropertyValueId;
            }

            if (!string.IsNullOrWhiteSpace(_propertyValue))
            {
                var propertyValue = _productPropertyValues.FirstOrDefault(x => x.Value == _propertyValue && x.PropertyId == propertyId);
                if (propertyValue != null)
                    return propertyValue.PropertyValueId;

                return PropertyService.AddPropertyValue(new PropertyValue()
                {
                    PropertyId = propertyId,
                    Value = _propertyValue.Trim()
                });
            }

            return 0;
        }
    }
}
