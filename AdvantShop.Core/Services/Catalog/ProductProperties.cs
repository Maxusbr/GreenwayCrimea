//--------------------------------------------------
// Project: AdvantShop.NET
// Web site: http:\\www.advantshop.net
//--------------------------------------------------

using System;

namespace AdvantShop.Catalog
{
    [Serializable]
    public class ProductProperty
    {
        public int PropertyId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public bool DisplayInBrief { get; set; }
        public bool DisplayInFilter { get; set; }
        public bool DisplayInDetails { get; set; }

        public ProductProperty()
        {
            Name = string.Empty;
            Value = string.Empty;
            DisplayInBrief = false;
            DisplayInFilter = true;
            DisplayInDetails = true;
        }
    }
}