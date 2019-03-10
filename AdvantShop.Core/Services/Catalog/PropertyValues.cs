//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using Newtonsoft.Json;

namespace AdvantShop.Catalog
{
    public class PropertyValue
    {
        public int PropertyValueId { get; set; }

        public int PropertyId { get; set; }

        public string Value { get; set; }

        public float RangeValue { get; set; }

        public int SortOrder { get; set; }

        [JsonIgnore]
        public Property Property { get; set; }
    }
}