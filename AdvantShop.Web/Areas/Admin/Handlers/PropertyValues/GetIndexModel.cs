using AdvantShop.Catalog;
using AdvantShop.Web.Admin.ViewModels.PropertyValues;

namespace AdvantShop.Web.Admin.Handlers.PropertyValues
{
    public class GetIndexModel
    {
        private readonly int _propertyId;

        public GetIndexModel(int propertyId)
        {
            _propertyId = propertyId;
        }

        public PropertyValuesViewModel Execute()
        {
            var property = PropertyService.GetPropertyById(_propertyId);
            if (property == null)
                return null;

            var model = new PropertyValuesViewModel()
            {
                PropertyId = property.PropertyId,
                PropertyName = property.Name,
                PropertyGroupId = property.GroupId ?? -1
            };

            return model;
        }
    }
}
