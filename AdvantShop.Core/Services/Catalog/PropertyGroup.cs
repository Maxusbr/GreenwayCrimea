using System.Collections.Generic;

namespace AdvantShop.Catalog
{
    public class PropertyGroup
    {
        public int PropertyGroupId { get; set; }
        public string Name { get; set; }
        public string NameDisplayed { get; set; }
        public int SortOrder { get; set; }
    }

    public class PropertyGroupView
    {
        public int PropertyGroupId { get; set; }
        public string GroupName { get; set; }

        public int PropertyId { get; set; }
        public string PropertyName { get; set; }
        public int Type { get; set; }
    }

    public class UsedPropertyGroupView
    {
        public int PropertyGroupId { set; get; }
        public string GroupName { set; get; }
        public List<Property> Properties { set; get; }
    }

    public class PropertyGroupComparer : IEqualityComparer<UsedPropertyGroupView>
    {
        public bool Equals(UsedPropertyGroupView x, UsedPropertyGroupView y)
        {
            return (x.PropertyGroupId == y.PropertyGroupId);
        }

        public int GetHashCode(UsedPropertyGroupView obj)
        {
            return obj.PropertyGroupId.GetHashCode();
        }
    }
}