using AdvantShop.Catalog;
using AdvantShop.Core.Common.Extensions;

namespace AdvantShop.Handlers.Inplace
{
    public class InplacePropertyHandler
    {
        public bool AddProperty(int productId, string name, string value, int propertyId = 0, int propertyValueId = 0)
        {
            if (productId == 0)
                return false;

            if (propertyValueId == 0 && PropertyService.IsExistPropertyValueInProduct(productId, propertyId, value))
                return false;

            if (propertyValueId != 0 && PropertyService.IsExistPropertyValueInProduct(productId, propertyValueId))
                return false;

            Property property = null;

            if (propertyId != 0)
                property = PropertyService.GetPropertyById(propertyId);
            else
                property = PropertyService.GetPropertyByName(name);

            if (property == null)
            {
                property = new Property()
                {
                    Name = name,
                    UseInDetails = true,
                    UseInFilter = true,
                    Type = 1
                };

                property.PropertyId = PropertyService.AddProperty(property);
            }


            if (value.IsNotEmpty())
            {
                PropertyValue propertyValue = null;

                if (propertyValueId != 0)
                    propertyValue = PropertyService.GetPropertyValueById(propertyValueId);
                else
                    propertyValue = PropertyService.GetPropertyValueByName(property.PropertyId, value);

                if (propertyValue == null || property.PropertyId == 0)
                {
                    propertyValue = new PropertyValue()
                    {
                        Value = value,
                        PropertyId = property.PropertyId
                    };

                    propertyValue.PropertyValueId = PropertyService.AddPropertyValue(propertyValue);
                }

                PropertyService.AddProductProperyValue(propertyValue.PropertyValueId, productId);
            }

            return true;
        }

        public bool DeleteProperty(int productId, int id)
        {
            PropertyService.DeleteProductPropertyValue(productId, id);
            return true;
        }

        public object UpdateProperty(int productId, int propertyValueId, string propertyValue)
        {
            var oldValue = PropertyService.GetPropertyValueById(propertyValueId);
            if (oldValue == null)
                return new
                {
                    result = false
                }; ;

            if (oldValue.Value == propertyValue)
                return new
                {
                    result = true
                };

            PropertyService.DeleteProductPropertyValue(productId, propertyValueId);


            var newValue = PropertyService.GetPropertyValueByName(oldValue.PropertyId, propertyValue);
            if (newValue == null)
            {
                newValue = new PropertyValue() { PropertyId = oldValue.PropertyId, Value = propertyValue, SortOrder = 0 };
                newValue.PropertyValueId = PropertyService.AddPropertyValue(newValue);
            }

            PropertyService.AddProductProperyValue(newValue.PropertyValueId, productId);

            return new
            {
                result = true,
                obj = new
                {
                    id = newValue.PropertyValueId
                }
            };
        }
    }
}