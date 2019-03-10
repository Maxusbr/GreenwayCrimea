//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System.Collections.Generic;

namespace AdvantShop.Catalog
{
    public class Property
    {
        public int PropertyId { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Название свойства, которое будет выводиться в клиентской части
        /// </summary>
        public string NameDisplayed { get; set; }

        public string Description { get; set; }

        public string Unit { get; set; }

        public int Type { get; set; }

        public bool UseInFilter { get; set; }

        public bool UseInDetails { get; set; }

        public bool UseInBrief { get; set; }

        public bool Expanded { get; set; }

        public int SortOrder { get; set; }

        public int? GroupId { get; set; }

        private PropertyGroup _group;
        public PropertyGroup Group {
            get
            {
                if (_group == null && GroupId != null)
                {
                    _group = PropertyGroupService.Get((int)GroupId);
                }
                return _group;
            }
            set { _group = value; }
        }


        private List<PropertyValue> _valuesList;

        [Newtonsoft.Json.JsonIgnore]
        public List<PropertyValue> ValuesList
        {
            get { return _valuesList ?? (_valuesList = PropertyService.GetValuesByPropertyId(PropertyId)); }
        }
    }

    public class UsedProperty
    {
        public int PropertyId { set; get; }
        public string Name { set; get; }
        public bool Expanded { get; set; }
        public string Unit { get; set; }
        public int Type { get; set; }
		public string Description { get; set; }

        public List<PropertyValue> ValuesList { set; get; }
    }

    public class PropertyComparer : IEqualityComparer<UsedProperty>
    {
        public bool Equals(UsedProperty x, UsedProperty y)
        {
            return (x.PropertyId == y.PropertyId);
        }

        public int GetHashCode(UsedProperty obj)
        {
            return obj.PropertyId.GetHashCode();
        }
    }

    public class RelatedProperty
    {
        public int PropertyId { get; set; }

        public int PropertyValueId { get; set; }

        public string Name { get; set; }
    }
}
