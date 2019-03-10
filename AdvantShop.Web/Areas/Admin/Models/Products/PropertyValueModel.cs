using System.Collections.Generic;
using System.Linq;
using AdvantShop.Catalog;

namespace AdvantShop.Web.Admin.Models.Products
{
    public class PropertyValueModel
    {
        public int PropertyValueId { get; set; }

        public int PropertyId { get; set; }

        public string Value { get; set; }

        public float RangeValue { get; set; }

        public int SortOrder { get; set; }

        public PropertyValueModel(PropertyValue propertyValue)
        {
            PropertyId = propertyValue.PropertyId;
            PropertyValueId = propertyValue.PropertyValueId;
            RangeValue = propertyValue.RangeValue;
            Value = propertyValue.Value;
            SortOrder = propertyValue.SortOrder;
        }
    }

    public class ProductPropertyModel
    {
        public ProductPropertyModel(Property property)
        {
            PropertyId = property.PropertyId;
            Name = property.Name;
            GroupId = property.GroupId;
            PropertyValues =
                PropertyService.GetValuesByPropertyId(property.PropertyId)
                    .Select(x => new PropertyValueModel(x))
                    .ToList();
        }

        public int? GroupId { get; set; }
        public int PropertyId { get; set; }
        public string Name { get; set; }

        public List<PropertyValueModel> SelectedPropertyValues { get; set; }
        public List<PropertyValueModel> PropertyValues { get; set; } 
    }

    public class ProductPropertyGroupModel
    {
        public ProductPropertyGroupModel()
        {
            Properties = new List<ProductPropertyModel>();
        }

        public string Title { get; set; }

        public int? GroupId { get; set; }
        public string Name { get; set; }
        
        public List<ProductPropertyModel> Properties { get; set; }
    }


    public class ProductProperiesModel
    {
        public List<ProductPropertyGroupModel> Groups { get; set; }
        public string CategoryName { get; set; }
        public List<SelectItemModel> Properties { get; set; }
    }

}
